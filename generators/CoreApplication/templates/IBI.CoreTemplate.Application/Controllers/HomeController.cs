using IBI.<%= Name %>.Application.Models;
using IBI.<%= Name %>.Application.Utils.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IBI.<%= Name %>.Application.Controllers
{
    public class HomeController : BaseController
    {
        #region Fields

        private readonly IHttpContextAccessor httpContext;
        private readonly ILogger logger;
        private readonly PluginSettings pluginSettings;

        #endregion Fields

        #region Constructors

        public HomeController(IOptions<PluginSettings> settings, IHttpContextAccessor httpContext, ILogger<HomeController> logger) : base(httpContext)
        {
            this.logger = logger;
            this.pluginSettings = settings.Value;
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