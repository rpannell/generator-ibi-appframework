using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBI.<%= Name %>.Service.Controllers
{
    [AllowAnonymous(), Route("/"), ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        #region Methods

        public ActionResult Index()
        {
            return Redirect("/swagger");
        }

        #endregion Methods
    }
}