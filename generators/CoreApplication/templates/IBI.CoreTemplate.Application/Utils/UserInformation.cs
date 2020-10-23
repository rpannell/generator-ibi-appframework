using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IBI.<%= Name %>.Application.Utils
{
    public static class UserInformation
    {
        #region Methods

        public static string GetApiToken(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
        }

        public static string GetImpersonatingPayeeId(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;
            return user.FindFirstValue("ImpersonatingPayeeId") ?? "-1";
        }

        public static string GetUserName(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;
            var userName = user.FindFirstValue("ImpersonatingName");
            return !string.IsNullOrEmpty(userName) ? user.Identity.Name : string.Empty;
        }

        public static string GetUserRoles(IHttpContextAccessor httpContextAccessor)
        {
            var roles = string.Empty;
            foreach (var pluginRole in <%= Name %>Template.PLUGINROLES.Split(','))
            {
                if (httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == <%= Name %>Template.PLUGINNAME && x.Value == pluginRole.Trim()))
                {
                    if (roles != string.Empty) roles += ",";
                    roles += pluginRole.Trim();
                }
            }

            return roles;
        }

        #endregion Methods
    }
}