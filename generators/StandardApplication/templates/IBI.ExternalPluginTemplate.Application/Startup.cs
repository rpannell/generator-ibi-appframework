using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(IBI.<%= Name %>.Application.Startup))]

namespace IBI.<%= Name %>.Application
{
    public class Startup
    {
        #region Methods

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies",
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                SignInAsAuthenticationType = "Cookies",
                Authority = ConfigurationManager.AppSettings["IdentityServerAuthority"],
                RedirectUri = ConfigurationManager.AppSettings["IdentityServerRedirectUri"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["IdentityServerPostLogoutRedirectUri"],
                ClientId = ConfigurationManager.AppSettings["IdentityServerAuthorityClient"],
                ClientSecret = ConfigurationManager.AppSettings["IdentityServerAuthorityPassword"],
                Resource = "openid profile api1 offline_access",
                ResponseType = "code id_token token",
                Scope = "openid profile api1 offline_access",
                AuthenticationType = "oidc",
                Notifications = new OpenIdConnectAuthenticationNotifications()
                {
                    AuthenticationFailed = (context) =>
                    {
                        if (context.Exception.Message.StartsWith("OICE_20004") || context.Exception.Message.Contains("IDX10311"))
                        {
                            context.SkipToNextMiddleware();
                            return Task.FromResult(0);
                        }
                        return Task.FromResult(0);
                    },
                }
            });
        }

        #endregion Methods
    }
}