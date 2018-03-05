using Castle.Windsor;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Configures the WebApi settings
    /// Created by Genie <%= TodaysDate %> by verion <%= Version %>
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Configures the WebApi settings
        /// </summary>
        /// <param name="config"></param>
        /// <param name="container"></param>
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            // Web API configuration and services
            //set Newtonsoft as the JSON body handler
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;

            //remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            RegisterControllerActivator(container);

            //map the routes that are based on attributes
            config.MapHttpAttributeRoutes();

            //create the generic routes
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

        /// <summary>
        /// Register the ControllerActivator based on Castle Windsor
        /// </summary>
        /// <param name="container">The Castle Windsor container</param>
        private static void RegisterControllerActivator(IWindsorContainer container)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator),
                new Core.Dependency.WindsorCompositionRoot(container));
        }
    }
}