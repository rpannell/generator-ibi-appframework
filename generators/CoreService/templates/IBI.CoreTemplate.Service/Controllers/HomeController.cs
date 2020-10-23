using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBI.<%= Name %>.Service.Controllers
{
    /// <summary>
    /// Starting controller
    /// </summary>
    [AllowAnonymous(), Route("/"), ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        #region Methods

        /// <summary>
        /// Starting action to redirect to the swagger
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }

        #endregion Methods
    }
}