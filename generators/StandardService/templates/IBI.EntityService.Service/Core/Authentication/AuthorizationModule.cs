using System;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace IBI.<%= Name %>.Service.Core.Authentication
{
    public class AuthorizationModule : IHttpModule
    {
        private const string Realm = "IBI.<%= Name %>.Service";

        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateUser(string credentials)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            credentials = encoding.GetString(Convert.FromBase64String(credentials));

            var credentialsArray = credentials.Split(':');
            var username = credentialsArray[0];
            var password = credentialsArray[1];

            /* REPLACE THIS WITH REAL AUTHENTICATION
            ----------------------------------------------*/
            if (username != string.Empty && password != string.Empty)
            {
                var identity = new GenericIdentity(username);
                SetPrincipal(new GenericPrincipal(identity, null));
                return true;
            }

            return false;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        // If the request was unauthorized, add the WWW-Authenticate header
        // to the response.
        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        {
        }

        //private static bool IsValid(string username, string password)
        //{
        //    using (var pc = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["WindowsDomain"]))
        //    {
        //        var isvalid = pc.ValidateCredentials(username, password);
        //        return isvalid;
        //    }
        //}
    }
}