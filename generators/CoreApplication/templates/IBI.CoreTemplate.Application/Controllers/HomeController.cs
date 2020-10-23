using IBI.<%= Name %>.Application.Utils.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IBI.<%= Name %>.Application.Controllers
{
    public class HomeController : BaseController
    {
        #region Fields

        private readonly IHttpContextAccessor httpContext;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public HomeController(IHttpContextAccessor httpContext, ILogger<HomeController> logger) : base(httpContext)
        {
            this.logger = logger;
            this.httpContext = httpContext;
        }

        #endregion Constructors

        #region Methods

        [AllowAnonymous()]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        #endregion Methods
    }
}