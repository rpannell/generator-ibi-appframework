using IBI.<%= Name %>.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IBI.<%= Name %>.Application.Services
{
    public class BaseService
    {
        #region Fields

        public IDistributedCache cache;
        public string Roles = string.Empty;
        public string Token = string.Empty;
        public string URL = string.Empty;
        public string UserName = string.Empty;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly PluginSettings pluginSettings;

        #endregion Fields

        #region Constructors

        public BaseService(IDistributedCache cache, IOptions<PluginSettings> settings, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;

            //this.UserName = Impersonation.Instance.CurrentImpersonatedUser;
            //create the comma delimited list of roles
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var user = httpContextAccessor.HttpContext.User;
                this.cache = cache;
                this.pluginSettings = settings.Value;
                this.URL = this.pluginSettings.WebServiceURL;
                this.UserName = this.GetUserName();
                this.Roles = this.GetUserRoles();
                this.Token = this.GetApiToken();
            }
        }

        #endregion Constructors

        #region Methods

        public string GetApiToken()
        {
            return this.httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
        }

        public string GetUserName()
        {
            var user = this.httpContextAccessor.HttpContext.User;
            var userName = user.FindFirstValue("ImpersonatingName");
            return string.IsNullOrEmpty(userName) ? user.Identity.Name : userName;
        }

        public string GetUserRoles()
        {
            var roles = string.Empty;
            foreach (var pluginRole in <%= Name %>Template.PLUGINROLES.Split(','))
            {
                if (this.httpContextAccessor.HttpContext.User.HasClaim(x => x.Type == <%= Name %>Template.PLUGINNAME && x.Value == pluginRole.Trim()))
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