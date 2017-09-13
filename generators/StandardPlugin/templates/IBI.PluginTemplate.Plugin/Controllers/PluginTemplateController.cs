using IBI.Plugin.Utilities.Logging.Interfaces;
using System.Web.Mvc;

namespace IBI.<%= Name %>.Plugin.Controllers
{
    public class <%= Name %>Controller : BaseController
    {
        private readonly ILogAdapter Log = <%= Name %>Logger<<%= Name %>Controller>.GetLogger();

        public ActionResult Index()
        {
            Log.Debug("Index Entered");
            return View();
        }
    }
}