using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    /// <summary>
    /// Context of a failed basic authentication
    /// </summary>
    public class AuthenticationFailedContext : ResultContext<BasicAuthenticationOptions>
    {
        #region Constructors

        /// <summary>
        /// Authentication Failed Context constructor
        /// </summary>
        /// <param name="context">HttpContext of the request containing the Authorize header</param>
        /// <param name="scheme">The auth scheme</param>
        /// <param name="options">Basic auth options</param>
        public AuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            BasicAuthenticationOptions options)
            : base(context, scheme, options) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The exception thrown while authenticating the request
        /// </summary>
        public Exception Exception { get; set; }

        #endregion Properties
    }
}