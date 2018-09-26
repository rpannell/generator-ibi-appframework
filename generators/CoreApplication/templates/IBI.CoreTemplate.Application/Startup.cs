using AutoMapper;
using IBI.<%= Name %>.Application.Models;
using IBI.<%= Name %>.Application.Utils.Cache;
using IBI.<%= Name %>.Application.Utils.Core;
using IBI.<%= Name %>.Application.Utils.DataProtection;
using IBI.<%= Name %>.Application.Utils.Services;
using IBI.<%= Name %>.Application.Utils.Services.Interfaces;
using IBI.<%= Name %>.Application.Utils.Session;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace IBI.<%= Name %>.Application
{
    public class Startup
    {
        #region Constructors

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        #endregion Constructors

        #region Properties

        public IConfigurationRoot Configuration { get; }

        #endregion Properties

        #region Methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                         .AddFile("Logs/<%= Name %>-{Date}.txt"); ;
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<PluginSettings>(Configuration.GetSection("PluginSettings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                    .AddTransient<IMenuService, MenuService>()
                    .AddWebserviceSession(opts =>
                    {
                        opts.InstanceName = "<%= Name %>";
                        opts.WebServiceURL = Configuration["PluginSettings:RedisServiceURL"];
                    })
                    .AddDistributedWebserviceCache(opts =>
                    {
                        opts.InstanceName = "<%= Name %>";
                        opts.WebServiceURL = Configuration["PluginSettings:RedisServiceURL"];
                    })
                    .AddNamespaceServices("IBI.<%= Name %>.Application.Services.Interfaces", "IBI.<%= Name %>.Application.Services", "Service");

            services.AddAuthentication(o =>
                    {
                        o.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                        o.DefaultSignInScheme = "Cookies-<%= Name %>"; //CookieAuthenticationDefaults.AuthenticationScheme;
                        o.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    })
                    .AddCookie("Cookies-<%= Name %>", options =>
                    {
                        // if true - Javascript will not be able to read cookie.
                        options.Cookie.HttpOnly = false;
                        // required or else it will result in an endless-login / redirect loop if it's called from an iframe in sharepoint
                        options.Cookie.SameSite = SameSiteMode.None;
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                        options.Cookie.Name = "Cookies-<%= Name %>";
                        options.SessionStore = new CookieStore(Configuration["PluginSettings:RedisServiceURL"], "<%= Name %>");
                        options.AccessDeniedPath = "/Home/AccessDenied";
                    })
                    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, o =>
                    {
                        o.SignInScheme = "Cookies-<%= Name %>";
                        o.Authority = Configuration["ApplicationSettings:IdentityServerAuthority"];
                        o.SignedOutRedirectUri = Configuration["ApplicationSettings:IdentityServerPostLogoutRedirectUri"];
                        o.ClientId = Configuration["ApplicationSettings:IdentityServerAuthorityClient"];
                        o.ClientSecret = Configuration["ApplicationSettings:IdentityServerAuthorityPassword"];
                        o.Resource = "openid profile api1";
                        o.ResponseType = "code id_token token";
                        o.Scope.Clear();
                        o.Scope.Add("openid");
                        o.Scope.Add("profile");
                        o.Scope.Add("api1");
                        o.Scope.Add("offline_access");
                        o.GetClaimsFromUserInfoEndpoint = true;
                        o.SaveTokens = true;
                        o.RequireHttpsMetadata = false;
                        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            NameClaimType = JwtClaimTypes.Name,
                            RoleClaimType = JwtClaimTypes.Role,
                        };
                    });

            services.AddSession(opts =>
                    {
                        opts.Cookie.Name = ".<%= Name %>.Session";
                        opts.IdleTimeout = TimeSpan.FromHours(10);
                    })
                    .AddAutoMapper()
                    .AddAuthorization(opts =>
                    {
                        opts.AddPolicy("<%= Name %>", policy => policy.RequireClaim("<%= Name %>", <%= Name %>Template.PLUGINROLES.Split(',').ToArray()));
                    })
                    .AddMvc()
                    .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //needs to be after AddMvc to work correctly
            services.AddDataProtection()
                    .PersistKeyToWebService(Configuration["PluginSettings:RedisServiceURL"], "<%= Name %>-Keys");
        }

        #endregion Methods
    }
}