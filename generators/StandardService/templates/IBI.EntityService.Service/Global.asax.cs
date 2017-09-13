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
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IWindsorContainer _container;

        protected void Application_Start()
        {
            ConfigureWindsor(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configure(c => WebApiConfig.Register(c, _container));
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(new QueryStringMapping("json", "true", "application/json"));
            log4net.Config.XmlConfigurator.Configure();
        }

        private void ConfigureWindsor(HttpConfiguration configuration)
        {
            _container = new WindsorContainer();
            _container.Install(FromAssembly.This());
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel, true));
            var dependencyResolver = new WindsorDependencyResolver(_container);
            configuration.DependencyResolver = dependencyResolver;
        }

        protected void Application_End()
        {
            _container.Dispose();
            base.Dispose();
        }
    }
}