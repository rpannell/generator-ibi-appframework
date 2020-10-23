using AutoMapper;
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
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;

namespace IBI.<%= Name %>.Application
{
    public class Startup
    {
        #region Constructors

        /// <summary>
        /// Startup constructor
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion Constructors

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseAzureAppConfiguration();
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                /*
                    Used for converting pluing to applications
                    ensuring the old links in emails, etc will work
                 */
                endpoints.MapAreaControllerRoute("old", "<%= Name %>", "<%= Name %>/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<Utils.Hubs.NotificationHub>("/notificationHub");
            });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy",
                                            "default-src 'self' 'unsafe-eval' 'unsafe-inline' data: *; ");

                await next();
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                    .AddRazorRuntimeCompilation();
            services.AddAzureAppConfiguration();
            services.AddOptions()
                    .AddSignalR();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                    .AddTransient<IMenuService, MenuService>()
                    .AddWebserviceSession(opts =>
                    {
                        opts.InstanceName = "<%= Name %>";
                        opts.WebServiceURL = Configuration["Webservice:Redis"];
                    })
                    .AddDistributedWebserviceCache(opts =>
                    {
                        opts.InstanceName = "<%= Name %>";
                        opts.WebServiceURL = Configuration["Webservice:Redis"];
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
                        options.SessionStore = new CookieStore(Configuration["Webservice:Redis"], "<%= Name %>");
                        options.AccessDeniedPath = "/Home/AccessDenied";
                    })
                    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, o =>
                    {
                        o.SignInScheme = "Cookies-<%= Name %>";
                        o.Authority = this.Configuration["Authorization:Authority"];
                        o.SignedOutRedirectUri = this.Configuration["ApplicationSettings:IdentityServerPostLogoutRedirectUri"];
                        o.ClientId = this.Configuration["Authorization:MainAuthorityClient"];
                        o.ClientSecret = this.Configuration["Authorization:MainAuthorityPassword"];
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
            services.AddAntiforgery(o => o.SuppressXFrameOptionsHeader = true);
            services.AddSession(opts =>
                    {
                        opts.Cookie.Name = ".<%= Name %>.Session";
                        opts.IdleTimeout = TimeSpan.FromHours(10);
                    })
                    .AddAutoMapper(typeof(Startup))
                    .AddAuthorization(opts =>
                    {
                        opts.AddPolicy("<%= Name %>", policy => policy.RequireClaim("<%= Name %>", <%= Name %>Template.PLUGINROLES.Split(',').ToArray()));
                    })
                    .AddMvc()
                    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //needs to be after AddMvc to work correctly
            services.AddDataProtection()
                    .PersistKeyToWebService(Configuration["Webservice:Redis"], "<%= Name %>-Keys");
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        #endregion Methods
    }
}