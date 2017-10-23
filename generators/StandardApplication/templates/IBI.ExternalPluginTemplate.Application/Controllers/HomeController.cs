using IBI.<%= Name %>.Application.Utils.Core.Controllers;
using System.Web.Mvc;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
            
        }

        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        #endregion Methods
    }
}