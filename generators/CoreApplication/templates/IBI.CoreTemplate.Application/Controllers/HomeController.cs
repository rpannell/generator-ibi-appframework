using IBI.<%= Name %>.Application.Utils.Core.Controllers;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IBI.<%= Name %>.Application.Controllers
{
    public class HomeController : BaseController
    {
        #region Fields

        private readonly TelemetryClient telemetryClient;
        private readonly IHttpContextAccessor httpContext;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        public HomeController(IHttpContextAccessor httpContext, ILogger<HomeController> logger, TelemetryClient telemetryClient) : base(httpContext)
        {
            this.logger = logger;
            this.httpContext = httpContext;
            this.telemetryClient = telemetryClient;
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

        #region Error Page

        /// <summary>
        /// Page to handle uncaught exceptions
        /// </summary>
        /// <returns></returns>
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            this.telemetryClient.TrackException(exceptionHandlerPathFeature.Error);
            this.telemetryClient.TrackEvent("Error.ServerError", new Dictionary<string, string>
            {
                ["originalPath"] = exceptionHandlerPathFeature.Path,
                ["error"] = exceptionHandlerPathFeature.Error.Message
            });
            return View();
        }

        #endregion Error Page
    }
}