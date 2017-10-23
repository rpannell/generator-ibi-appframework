using IBI.Plugin.Utilities.Logging.Interfaces;
using System.Web.Mvc;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin.Controllers
{
    public class <%= Name %>Controller : BaseController
    {
        private readonly ILogAdapter Log = <%= Name %>Logger<<%= Name %>Controller>.GetLogger();

        public <%= Name %>Controller()
        {
            
        }

        public ActionResult Index()
        {
            Log.Debug("Index Entered");
            return View();
        }
    }
}