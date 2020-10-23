using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Starting program
    /// </summary>
    public class Program
    {
        #region Methods

        /// <summary>
        /// Create the Web Host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);

            builder
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext();

                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfiguration
                        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                        .WriteTo.File("Logs/<%= Name %>-.log", rollingInterval: RollingInterval.Day);
                    }
                    else
                    {
                        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                        telemetryConfiguration.InstrumentationKey = hostingContext.Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];

                        loggerConfiguration
                                    .WriteTo.File("Logs/<%= Name %>-.log", rollingInterval: RollingInterval.Day, flushToDiskInterval: TimeSpan.FromSeconds(10))
                                    .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces);
                    };
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                              .UseContentRoot(Directory.GetCurrentDirectory());
                })
               .ConfigureAppConfiguration((context, config) =>
               {
                   var builtConfig = config.Build();

                   var env = context.HostingEnvironment;

                   config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                   if (env.IsDevelopment())
                   {
                       var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                       if (appAssembly != null)
                       {
                           config.AddUserSecrets(appAssembly, optional: true);
                       }
                   }
                   else
                   {
                       config.AddAzureAppConfiguration(options =>
                       {
                           options.Connect(builtConfig["AzureAppConfiguration"])
                           .ConfigureKeyVault(kv =>
                           {
                               kv.SetCredential(new ManagedIdentityCredential());
                           })
                           .ConfigureRefresh(configuration =>
                           {
                               configuration.Register(KeyFilter.Any, "Authority", true);
                               configuration.Register(KeyFilter.Any, "<%= Name %>", true);
                               configuration.Register(KeyFilter.Any, "Redis", true);
                               configuration.SetCacheExpiration(TimeSpan.FromMinutes(30));
                           })
                           .Select(KeyFilter.Any, "Authority")
                           .Select(KeyFilter.Any, "Redis")
                           .Select(KeyFilter.Any, "<%= Name %>");
                       });
                   }

                   config.AddEnvironmentVariables();
               });

            return builder;
        }

        /// <summary>
        /// Starting function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Methods
    }
}