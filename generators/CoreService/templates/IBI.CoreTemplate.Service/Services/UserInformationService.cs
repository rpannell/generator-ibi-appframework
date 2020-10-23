using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace IBI.<%= Name %>.Service.Services
{
    /// <summary>
    /// Service to get the information of the currently logged in user
    /// </summary>
    public class UserInformationService : Interfaces.IUserInformationService
    {
        #region Fields

        private readonly IHttpContextAccessor httpContextAccessor;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Service to get the information of the currently logged in user
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserInformationService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Returns the ActiveDirectoryId of the currently logged in user
        /// </summary>
        /// <returns>int</returns>
        public int GetCurrentUserActiveDirectoryId()
        {
            var user = httpContextAccessor.HttpContext.User;
            var userId = "";
            if (!(user.Identity != null && user.HasClaim(x => x.Type == "UserId")))
            {
                return 0;
            }

            if (user.HasClaim(x => x.Type == "ImpersonatingUserId"))
            {
                userId = user.FindFirstValue("ImpersonatingUserId");
            }
            if (string.IsNullOrEmpty(userId))
            {
                userId = user.FindFirstValue("UserId");
            }

            return !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : -1;
        }

        /// <summary>
        /// Returns the username of the currently logged in user
        /// </summary>
        /// <returns>string</returns>
        public string GetCurrentUserName()
        {
            return this.httpContextAccessor.HttpContext.User.Identity.Name;
        }

        /// <summary>
        /// returns true or false if the user has a role or not
        /// </summary>
        /// <param name="role">The name of the role</param>
        /// <param name="plugin">The plugin name (defaults to <%= Name %>)</param>
        /// <returns>boolean</returns>
        public bool IsInRole(string role, string plugin = "<%= Name %>")
        {
            return this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated && this.httpContextAccessor.HttpContext.User.HasClaim(x => x.Type.ToLower() == plugin.ToLower() && x.Value.ToLower() == role.ToLower());
        }

        /// <summary>
        /// Returns current user email address
        /// </summary>
        /// <returns>string</returns>
        public string GetCurrentUserEmailAddress()
        {
            var user = httpContextAccessor.HttpContext.User;
            var emailAddress = string.Empty;
            if (!(user.Identity != null && user.HasClaim(x => x.Type == "UserId")))
            {
                return string.Empty;
            }

            if (user.HasClaim(x => x.Type == "ImpersonatingEmail"))
            {
                emailAddress = user.FindFirstValue("ImpersonatingEmail");
            }

            if (string.IsNullOrEmpty(emailAddress))
            {
                emailAddress = user.FindFirstValue("email");
            }

            return !string.IsNullOrEmpty(emailAddress) ? emailAddress : string.Empty;
        }

        #endregion Methods
    }
}