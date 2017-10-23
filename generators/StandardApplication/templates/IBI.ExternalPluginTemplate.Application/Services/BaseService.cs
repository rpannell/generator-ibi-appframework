using IBI.FrameworkSecurity.Client;
using IBI.Plugin.Utilities.Cache.Redis;
using System.Configuration;
using System.Web;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Services
{
    /// <summary>
    /// Used as a base service that all other services will extend for a specific entity
    ///
    /// Give the service access to Get, Update, Add, Delete functions
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseService
    {
        #region Fields

        public string Roles = string.Empty;
        public RedisCacheProvider ServiceCache = new RedisCacheProvider("<%= Name %>");
        public string URL = string.Empty;
        public string UserName = string.Empty;

        #endregion Fields

        #region Constructors

        public BaseService()
        {
            this.URL = ConfigurationManager.AppSettings["<%= Name %>ServiceURL"];
            this.UserName = Impersonation.Instance.CurrentImpersonatedUser;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                foreach (var pluginRole in <%= Name %>.APPLICATIONROLES.Split(','))
                {
                    if (Utils.CurrentUserInfo.HasPluginRole(pluginRole.Trim()))
                    {
                        if (Roles != string.Empty) Roles += ",";
                        this.Roles += pluginRole.Trim();
                    }
                }
            }
        }

        #endregion Constructors
    }
}