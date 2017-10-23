using System.Web.Mvc;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Controllers
{
    public class HomeController : Controller
    {
        #region Methods
        /// <summary>
        /// Redirects users to the swagger page of the web service
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Redirect("Swagger/");
        }

        #endregion Methods
    }
}