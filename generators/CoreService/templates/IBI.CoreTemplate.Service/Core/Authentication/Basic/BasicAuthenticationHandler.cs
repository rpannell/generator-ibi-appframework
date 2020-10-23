using IBI.<%= Name %>.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    /// <summary>
    /// Handler for Basic Authentication
    /// </summary>
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        #region Fields

        private readonly IActiveDirectoryService activeDirectoryService;

        private class User
        {
            #region Properties

            public string Password { get; set; }
            public string Username { get; set; }

            #endregion Properties
        }

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Handler to work with the basic authentication if needed
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        /// <param name="activeDirectoryService"></param>
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IActiveDirectoryService activeDirectoryService)
            : base(options, logger, encoder, clock)
        {
            this.activeDirectoryService = activeDirectoryService;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Authenticate
        /// </summary>
        /// <returns><see cref="AuthenticateResult"/></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");
            var user = new User();

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                user.Username = username;
                user.Password = password;
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            var adUser = await this.activeDirectoryService.GetUserByUserName(user.Username);
            if (adUser != null)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, adUser.ActiveDirectoryLogin),
                    new Claim(ClaimTypes.GivenName, adUser.ActiveDirectoryFriendlyName),
                    new Claim(ClaimTypes.Email, adUser.EmailAddress),
                    new Claim("UserId", adUser.ActiveDirectoryID.ToString()),
                };
                foreach (var role in user.Password.Split(',')) { claims.Add(new Claim("<%= Name %>", role.Trim())); }
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            return AuthenticateResult.NoResult();
        }

        #endregion Methods
    }
}