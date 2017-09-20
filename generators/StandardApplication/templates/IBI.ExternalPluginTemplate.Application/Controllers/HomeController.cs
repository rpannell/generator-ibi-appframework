using IBI.<%= Name %>.Application.Utils.Core.Controllers;
using System.Web.Mvc;

namespace IBI.<%= Name %>.Application.Controllers
{
    public class HomeController : BaseController
    {
        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion Methods
    }
}