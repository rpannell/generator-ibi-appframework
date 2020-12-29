using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;

namespace IBI.<%= Name %>.Application.Utils
{
    /// <summary>
    /// Enhance the telemetry data we send to application insights
    /// </summary>
    public class TelemetryEnrichment : TelemetryInitializerBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public TelemetryEnrichment(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        /// <summary>
        /// Initialize Telemetry and add any necessary updated information
        /// </summary>
        /// <param name="platformContext">HttpContext</param>
        /// <param name="requestTelemetry">The request telementry</param>
        /// <param name="telemetry">Current telementry</param>
        protected override void OnInitializeTelemetry(HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry)
        {
            telemetry.Context.User.AuthenticatedUserId =
                platformContext.User?.Identity.Name ?? string.Empty;
        }
    }
}
