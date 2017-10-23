using System.Collections.Generic;
using System.Security.Principal;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Authentication
{
    /// <summary>
    /// The custom principal that contains the username and roles
    /// that are passed into the service via the authorization headers
    /// </summary>
    public class CustomPrincipal : GenericPrincipal
    {
        #region Constructors
        /// <summary>
        /// Create a CustomPrincipal
        /// </summary>
        /// <param name="gi">The generic identity that contains the username</param>
        /// <param name="role">Comma delimited list of roles</param>
        public CustomPrincipal(GenericIdentity gi, string role)
            : base(gi, null)
        {
            this.Roles = new List<string>();
            foreach (var r in role.Split(','))
            {
                this.Roles.Add(r.Trim());
            }
        }

        #endregion Constructors

        #region Properties

        public List<string> Roles { get; set; }

        #endregion Properties

        /// <summary>
        /// Check if the roles exists in the list of roles
        /// </summary>
        /// <param name="role">The name of the role</param>
        /// <returns>True if it exists, false if not</returns>
        public override bool IsInRole(string role)
        {
            return this.Roles.Exists(x => x.ToLower() == role.ToLower());
        }
    }
}