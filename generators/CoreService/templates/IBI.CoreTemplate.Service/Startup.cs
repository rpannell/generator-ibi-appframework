using AutoMapper;
using IBI.<%= Name %>.Service.Core.Context;
using IBI.<%= Name %>.Service.Core.Utils;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace IBI.<%= Name %>.Service
{
    public class Startup
    {
        #region Constructors

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion Constructors

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion Properties

        #region Methods

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"))
                         .AddFile("Logs/<%= Name %>-{Date}.txt");
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //enable swagger (help page)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "<%= Name %> API v1.0");
            });

            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //setup the repositories
            services
                .AddAutoMapper()
                .AddEntityFrameworkSqlServer()
                .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<MainContext>()
                .AddDbContext<MainContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]))
                //add DI for services and Repo
                .AddNamespaceServices("IBI.<%= Name %>.Service.Services.Interfaces", "IBI.<%= Name %>.Service.Services", "Service")
                .AddNamespaceServices("IBI.<%= Name %>.Service.Repositories.Interfaces", "IBI.<%= Name %>.Service.Repositories", "Repository");

            //add basic authentication if we are in debug mode, else all others will need to be using the token
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        // base-address of your identityserver
                        options.Authority = Configuration["ApplicationSettings:IdentityServerAuthority"];

                        // name of the API resource
                        options.ApiName = Configuration["ApplicationSettings:IdentityServerAuthorityClient"];
                        options.ApiSecret = Configuration["ApplicationSettings:IdentityServerAuthorityPassword"];
                    });

            //setup help page
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "<%= Name %> API v1.0", Version = "v1.0" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //initialize auto mapper
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
        }

        #endregion Methods
    }
}