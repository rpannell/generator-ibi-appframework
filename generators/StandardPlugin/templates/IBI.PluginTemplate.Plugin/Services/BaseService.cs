using IBI.FrameworkSecurity.Client;
using IBI.Plugin.Utilities.Cache.Redis;
using InterlineBrands.Platform.Core.Settings;
using System.Web;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin.Services
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

        public string URL = string.Empty;        
#if DEBUG
        public RedisCacheProvider ServiceCache = new RedisCacheProvider("<%= Name %>-Local");
#else
        public RedisCacheProvider ServiceCache = new RedisCacheProvider("<%= Name %>");
#endif		
        public string UserName = string.Empty;
        public string Roles = string.Empty;

        #endregion Fields

        #region Constructors

        public BaseService(IPluginSettings pluginSettings)
        {
            this.URL = pluginSettings["<%= Name %>ServiceURL"];
			this.UserName = Impersonation.Instance.CurrentImpersonatedUser;
            //create the comma delimited list of roles
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                foreach (var pluginRole in <%= Name %>.PLUGINROLES.Split(','))
                {
                    if (((InterlineBrands.Platform.Core.IPluginPrinciple)HttpContext.Current.User).IsInRole(pluginRole.Trim(), <%= Name %>.PLUGINNAME))
                    {
                        if (this.Roles != string.Empty) Roles += ",";
                        this.Roles += pluginRole.Trim();
                    }
                }
            }
        }

        #endregion Constructors
    }
}