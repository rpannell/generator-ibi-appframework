using IBI.<%= Name %>.Service.Core.Authentication;
using System.Web;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Utils
{
    /// <summary>
    /// Helps to get some information about the user who is logged into the web service
    /// </summary>
    public class UserInfo
    {
        #region Methods

        /// <summary>
        /// Checks to see if the user has a particular role
        /// </summary>
        /// <param name="roleName">The name of the role</param>
        /// <returns>True if the user has the role, false if not</returns>
        public bool IsUserInRole(string roleName)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return ((CustomPrincipal)(HttpContext.Current.User)).IsInRole(roleName);
            }
            return false;
        }

        #endregion Methods
    }
}