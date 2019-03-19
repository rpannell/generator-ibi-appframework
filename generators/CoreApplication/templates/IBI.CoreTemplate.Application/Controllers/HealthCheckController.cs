using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IBI.<%= Name %>.Application.Controllers
{
    [AllowAnonymous()]
    public class HealthCheckController : Controller
    {
        #region Methods

        [HttpGet("HealthCheck/"), AllowAnonymous()]
        public ActionResult Index()
        {
            return new JsonResult(new { Worked = true });
        }

        #endregion Methods
    }
}