using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using IBI.<%= Name %>.Service.Core.Dependency;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Created by Genie <%= TodaysDate %> by verion <%= Version %>
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;

        /// <summary>
        /// Setups everything in memory needed for the application to run
        /// </summary>
        protected void Application_Start()
        {
            ConfigureWindsor(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(c => WebApiConfig.Register(c, _container));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //sets the application/json header for every request made, unless it already has another one set
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));
            //configures the log4net based on the web.config settings
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Configures the Castle Windsor information
        /// </summary>
        /// <param name="configuration"></param>
        private void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }

        /// <summary>
        /// Removes from the memory when the application ends
        /// </summary>
        protected void Application_End()
        {
            _container.Dispose();
            base.Dispose();
        }
    }
}