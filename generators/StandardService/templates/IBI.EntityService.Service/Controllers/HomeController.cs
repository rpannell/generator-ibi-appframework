using System.Web.Mvc;

namespace IBI.<%= Name %>.Service.Controllers
{
    public class HomeController : Controller
    {
        #region Methods

        public ActionResult Index()
        {
            return Redirect("/Swagger/");
        }

        #endregion Methods
    }
}