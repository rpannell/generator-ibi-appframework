using System.Linq;
using System.Security.Claims;
using System.Web;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils
{
    public static class CurrentUserInfo
    {
        #region Properties

        public static string CurrentUser
        {
            get { return Impersonator != null && Impersonator != string.Empty ? Impersonator : UserName; }
        }

        /// <summary>
        /// Returns the email address of the person logged int
        /// </summary>
        /// <returns>string</returns>
        public static string Email
        {
            get
            {
                return GetIdentity().IsAuthenticated
                              ? GetIdentity().Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value
                              : string.Empty;
            }
        }

        /// <summary>
        /// Gets the username of the user who is impersonating the person that is logged in
        /// </summary>
        /// <returns>string</returns>
        public static string Impersonator
        {
            get { return GetIdentity().FindFirst("Impersonator") != null ? GetIdentity().FindFirst("Impersonator").Value : string.Empty; }
        }

        /// <summary>
        /// Gets the Impersonator Active Directory Id of the user who is impersonating the
        /// person that is logged int
        /// </summary>
        /// <returns>int</returns>
        public static int ImpersonatorUserActiveDirectoryId
        {
            get { return GetIdentity().FindFirst("Impersonator") != null ? int.Parse(GetIdentity().FindFirst("Impersonator").Value) : 0; }
        }

        /// <summary>
        /// Gets the ActiveDirectoryId of the person logged int
        /// </summary>
        /// <returns>int</returns>
        public static int UserActiveDirectoryId
        {
            get { return int.Parse(GetIdentity().IsAuthenticated ? GetIdentity().FindFirst("UserId").Value : "0"); }
        }

        /// <summary>
        /// Returns the username of the person logged int
        /// </summary>
        /// <returns>string</returns>
        public static string UserName
        {
            get { return GetIdentity().Name; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Checkes to see if the user has a specific Plugin Role for a specific plugin
        /// The default for the plugin name is <%= Name %>
        /// </summary>
        /// <param name="roleName">The name of the plugin role</param>
        /// <param name="pluginName">The name of the plugin Default: <%= Name %></param>
        /// <returns>boolean</returns>
        public static bool HasPluginRole(string roleName, string pluginName = "<%= Name %>")
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && GetIdentity().HasClaim(x => x.Type == pluginName.Trim() && x.Value == roleName.Trim());
        }

        /// <summary>
        /// Checks to see if the logged in user has a specific windows role
        /// </summary>
        /// <param name="roleName">The name of the role</param>
        /// <returns>boolean</returns>
        public static bool HasWindowsRole(string roleName)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated && GetIdentity().HasClaim(x => x.Type == ClaimTypes.Role && x.Value == roleName.Trim());
        }

        /// <summary>
        /// private function to get the Identity of the currently logged in user
        /// </summary>
        /// <returns></returns>
        private static ClaimsIdentity GetIdentity()
        {
            return (ClaimsIdentity)HttpContext.Current.User.Identity;
        }

        #endregion Methods
    }
}