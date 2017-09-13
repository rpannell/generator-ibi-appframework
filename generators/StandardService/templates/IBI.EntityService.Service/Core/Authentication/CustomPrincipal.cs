using System.Collections.Generic;
using System.Security.Principal;


namespace IBI.<%= Name %>.Service.Core.Authentication
{
    public class CustomPrincipal : GenericPrincipal
    {
        #region Constructors

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

        public override bool IsInRole(string role)
        {
            return this.Roles.Exists(x => x.ToLower() == role.ToLower());
        }
    }
}