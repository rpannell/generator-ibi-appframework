using Azure.Identity;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace IBI.<%= Name %>.Application
{
    public class Program
    {
        #region Methods

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
                    else if (hostingContext.HostingEnvironment.IsEnvironment("Test") || hostingContext.HostingEnvironment.IsProduction())
                    {
                        var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
                        telemetryConfiguration.InstrumentationKey = hostingContext.Configuration["ApplicationInsights:InstrumentationKey"];

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
                   else if (context.HostingEnvironment.IsEnvironment("Test"))
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
                               configuration.Register(KeyFilter.Any, "ProToGo", true);
                               configuration.Register(KeyFilter.Any, "Redis", true);
                               configuration.Register(KeyFilter.Any, "WebFramework", true);
                               configuration.SetCacheExpiration(TimeSpan.FromMinutes(30));
                           })
                           .Select(KeyFilter.Any, "Authority")
                           .Select(KeyFilter.Any, "WebFramework")
                           .Select(KeyFilter.Any, "Redis")
                           .Select(KeyFilter.Any, "ProToGo");
                       });
                   }
                   else if (context.HostingEnvironment.IsProduction())
                   {
                       //get the config data from the configured azure key vault
                       var azureServiceTokenProvider = new AzureServiceTokenProvider();
                       var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                       Console.WriteLine($"KeyVault: {builtConfig["KeyVaultName"]}");

                       config.AddAzureKeyVault(new AzureKeyVaultConfigurationOptions()
                       {
                           Vault = $"https://{builtConfig["KeyVaultName"]}.vault.azure.net/",
                           Client = keyVaultClient,
                           Manager = new DefaultKeyVaultSecretManager(),
                           //ReloadInterval = System.TimeSpan.FromMinutes(30)
                       });
                   }

                   config.AddEnvironmentVariables();
               });

            return builder;
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        #endregion Methods
    }
}