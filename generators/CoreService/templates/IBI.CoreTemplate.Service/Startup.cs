using AutoMapper;
using IBI.<%= Name %>.Service.Core.Authentication.Basic;
using IBI.<%= Name %>.Service.Core.Context;
using IBI.<%= Name %>.Service.Core.Utils;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Startup settings for the application
    /// </summary>
    public class Startup
    {
        #region Constructors

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration">The current configuration created in the program builder</param>
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Current configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Current application pipeline
        /// </summary>
        /// <param name="app">Current app</param>
        /// <param name="env">Current Environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //enable swagger (help page)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "<%= Name %> API v1.0");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        /// <summary>
        /// Configure the services used for each request
        /// </summary>
        /// <param name="services">Current service collection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            //setup the repositories
            services
                .AddAutoMapper(typeof(Startup))
                .AddEntityFrameworkSqlServer()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddSingleton<Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer, Core.Utils.TelemetryEnrichment>()
                .AddScoped<MainContext>()
                .AddDbContext<MainContext>(options => options.UseSqlServer(Configuration["Database:<%= Name %>"]))
                //add DI for services and Repo
                .AddNamespaceServices("IBI.<%= Name %>.Service.Services.Interfaces", "IBI.<%= Name %>.Service.Services", "Service")
                .AddNamespaceServices("IBI.<%= Name %>.Service.Repositories.Interfaces", "IBI.<%= Name %>.Service.Repositories", "Repository");

            //add basic authentication if we are in debug mode, else all others will need to be using the token
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
#if DEBUG
                    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null)
#endif
                    .AddIdentityServerAuthentication("Bearer", options =>
                    {
                        // base-address of your identityserver
                        options.Authority = Configuration["Authorization:Authority"];

                        // name of the API resource
                        options.ApiName = Configuration["Authorization:MainApiClient"];
                        options.ApiSecret = Configuration["Authorization:MainApiPassword"];
                    });
            /*
             * Force the need to have a claim with one of these roles before the user is authorized to get
             * from this service
             */
            var useableRoles = "Basic,Admin";

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                {
                    policy.RequireClaim("<%= Name %>", useableRoles.Split(','));
#if DEBUG
                    policy.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme, BasicAuthenticationDefaults.AuthenticationScheme);
#else
                    policy.AddAuthenticationSchemes(IdentityServerAuthenticationDefaults.AuthenticationScheme);
#endif

                    policy.RequireAuthenticatedUser();
                });
            });

            //setup help page
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "<%= Name %> API v1.0", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. <br/><br/>
                      Enter 'Bearer' [space] and then your token in the text input below.
                      <br/><br/>Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
#if DEBUG
                c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
                {
                    Description = @"Standard username found the active_directory table<br/><br/> with the comma list of plugin roles",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Basic"
                });
#endif
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
#if DEBUG
                    ,{
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {  Type = ReferenceType.SecurityScheme, Id = "Basic" },
                            Scheme = "Basic",
                            Name = "Basic",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
#endif
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        #endregion Methods
    }
}