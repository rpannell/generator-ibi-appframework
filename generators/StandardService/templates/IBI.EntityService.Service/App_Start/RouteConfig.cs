using System.Web.Mvc;
using System.Web.Routing;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Configure the routes
    /// </summary>
    public class RouteConfig
    {
        #region Methods

        /// <summary>
        /// Configure the routes
        /// </summary>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Healthcheck",
                url: "Healthcheck",
                defaults: new { controller = "Home", action = "Healthcheck", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        #endregion Methods
    }
}