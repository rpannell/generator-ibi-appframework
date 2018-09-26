using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;

namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    public class BasicAuthenticationChallengeContext : PropertiesContext<BasicAuthenticationOptions>
    {
        #region Constructors

        public BasicAuthenticationChallengeContext(
            HttpContext context,
            AuthenticationScheme scheme,
            BasicAuthenticationOptions options,
            AuthenticationProperties properties)
            : base(context, scheme, options, properties) { }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Any failures encountered during the authentication process.
        /// </summary>
        public Exception AuthenticateFailure { get; set; }

        /// <summary>
        /// If true, will skip any default logic for this challenge.
        /// </summary>
        public bool Handled { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Skips any default logic for this challenge.
        /// </summary>
        public void HandleResponse() => Handled = true;

        #endregion Methods
    }
}