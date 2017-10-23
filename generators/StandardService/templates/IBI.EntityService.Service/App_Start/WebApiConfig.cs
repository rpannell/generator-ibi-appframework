using Castle.Windsor;
using System.Web.Http;
using System.Web.Http.Dispatcher;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            // Web API configuration and services
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            RegisterControllerActivator(container);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "defaultapi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        private static void RegisterControllerActivator(IWindsorContainer container)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new Core.Dependency.WindsorCompositionRoot(container));
        }
    }
}