using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    /// <summary>
    /// Context of a successful basic authentication
    /// </summary>
    public class AuthenticationSucceededContext : ResultContext<BasicAuthenticationOptions>
    {
        #region Constructors

        /// <summary>
        /// Authentication Succeeded Context constructor
        /// </summary>
        /// <param name="userId">The user id which successfully authenticated</param>
        /// <param name="context">HttpContext of the request containing the Authorize header</param>
        /// <param name="scheme">The auth scheme</param>
        /// <param name="options">Basic auth options</param>
        public AuthenticationSucceededContext(
            string userId,
            HttpContext context,
            AuthenticationScheme scheme,
            BasicAuthenticationOptions options)
            : base(context, scheme, options)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The user id which successfully authenticated
        /// </summary>
        public string UserId { get; }

        #endregion Properties
    }
}