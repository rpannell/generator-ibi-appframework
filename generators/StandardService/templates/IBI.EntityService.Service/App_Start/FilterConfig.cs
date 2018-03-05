using System.Web.Mvc;

namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Created by Genie <%= TodaysDate %> by verion <%= Version %>
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Register Global Filters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}