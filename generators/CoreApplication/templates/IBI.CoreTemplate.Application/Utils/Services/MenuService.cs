using IBI.<%= Name %>.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace IBI.<%= Name %>.Application.Utils.Services
{
    public class MenuService : Interfaces.IMenuService
    {
        #region Fields

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Utils.RestClient<string> menuClient;
        private readonly Utils.RestClient<string> scriptClient;
        private readonly PluginSettings settings;

        #endregion Fields

        #region Constructors

        public MenuService(IHttpContextAccessor httpContextAccessor, IOptions<PluginSettings> settings)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.settings = settings.Value;
            if (this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var token = this.GetApiToken();

                this.menuClient = new RestClient<string>(this.settings.FrameworkUIUrl, "Menu", this.GetApiToken());
                this.scriptClient = new RestClient<string>(this.settings.FrameworkUIUrl, "MenuScript", this.GetApiToken());
            }
        }

        #endregion Constructors

        #region Methods

        public string GetMenu()
        {
            try
            {
                var response = this.menuClient.CreateUrlResponse(HttpMethod.Get, string.Empty, null, new RestClient<string>.UrlParameters("pluginName", <%= Name %>Template.PLUGINNAME));
                if (response.Result.IsSuccessStatusCode)
                {
                    var item = response.Result.Content.ReadAsStringAsync();
                    return item.Result;
                }
            }
            catch (Exception)
            {
            }
            return "<aside id=\"left-panel\"></aside>";
        }

        public string GetMenuJavaScript()
        {
            try
            {
                var response = this.scriptClient.CreateUrlResponse(HttpMethod.Get, string.Empty);
                if (response.Result.IsSuccessStatusCode)
                {
                    var item = response.Result.Content.ReadAsStringAsync();
                    return item.Result;
                }
            }
            catch (Exception)
            {
            }
            return "";
        }

        private string GetApiToken()
        {
            var t = this.httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            return t;
        }

        #endregion Methods
    }
}