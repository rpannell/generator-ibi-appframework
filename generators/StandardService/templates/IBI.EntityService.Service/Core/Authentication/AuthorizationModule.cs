using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Authentication
{
    /// <summary>
    /// Module that deals with Authorization of all request
    /// </summary>
    public class AuthorizationModule : IHttpModule
    {
        private const string Realm = "IBI.<%= Name %>.Service";

        /// <summary>
        /// Initializes the request
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            // Register event handlers
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        /// <summary>
        /// Adds the principal to the current Thread and current context
        /// </summary>
        /// <param name="principal"></param>
        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        /// <summary>
        /// Takes the Authorization header information and Authenticates
        /// the user base on the username / password
        /// 
        /// The password in this comma delimited list of roles the user has
        /// </summary>
        /// <param name="credentials">string that represents the UserName:Password</param>
        /// <returns>True if authticated else false</returns>
        private static bool AuthenticateUser(string credentials)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            credentials = encoding.GetString(Convert.FromBase64String(credentials));

            //get the username and password
            var credentialsArray = credentials.Split(':');
            var username = credentialsArray[0];
            var password = credentialsArray[1];

            //if we have a username and password, create the principal
            if (username != string.Empty && password != string.Empty)
            {
                //add the roles removing the trailing spaces
                var roles = new List<string>();
                foreach (var role in password.Split(','))
                {
                    roles.Add(role.Trim());
                }

                //set the principal on the request
                SetPrincipal(new GenericPrincipal(new GenericIdentity(username), roles.ToArray()));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Handles creating the Authentication headers whent he request is asked
        /// to be authenticated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            //get the Authorization header
            var request = HttpContext.Current.Request;
            var authHeader = request.Headers["Authorization"];
            if (authHeader != null)
            {
                //if the header isn't null, get the value
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                // RFC 2617 sec 1.2, "scheme" name is case-insensitive
                // check to see if this is a basic authicate request
                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }


        /// <summary>
        /// If the request was unauthorized, add the WWW-Authenticate header
        /// to the response.
        /// </summary>
        /// <param name="sender">Who sent the request</param>
        /// <param name="e">The event</param>
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
    }
}