using IBI.<%= Name %>.Application.Utils.RestClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

namespace IBI.<%= Name %>.Application.Utils.Services
{
    public class MenuService : Interfaces.IMenuService
    {
        #region Fields

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration configuration;
        private readonly RestClient<string> menuClient;
        private readonly RestClient<string> scriptClient;

        #endregion Fields

        #region Constructors

        public MenuService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
            if (this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var token = this.GetApiToken();

                this.menuClient = new RestClient<string>(configuration["Webservice:UI"], "Menu", this.GetApiToken());
                this.scriptClient = new RestClient<string>(configuration["Webservice:UI"], "MenuScript", this.GetApiToken());
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