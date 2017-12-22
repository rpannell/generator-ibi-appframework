using System.Net;
using System.Web.Mvc;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        #region Methods
        
        /// <summary>
        /// Perform any necessary checks to check the health of the service
        /// </summary>
        /// <returns></returns>
        [Route("Healthcheck")]
        public ActionResult Healthcheck()
        {
            //TODO:  add any necessary pieces for the health check
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

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