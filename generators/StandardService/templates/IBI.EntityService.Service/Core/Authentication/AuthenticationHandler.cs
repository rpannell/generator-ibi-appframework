using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Authentication
{
    public class AuthenticationHandler : DelegatingHandler
    {
        #region Methods

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var tokens = request.Headers.GetValues("Authorization").FirstOrDefault();
                if (tokens != null)
                {
                    tokens = tokens.Replace("Basic ", "");
                    var data = Convert.FromBase64String(tokens);
                    var decodedString = Encoding.UTF8.GetString(data);
                    var tokensValues = decodedString.Split(':');

                    if (tokensValues[0] != string.Empty && tokensValues[1] != string.Empty)
                    {
                        var principal = new CustomPrincipal(new GenericIdentity(tokensValues[0]), tokensValues[1]);
                        Thread.CurrentPrincipal = (IPrincipal)principal;
                        HttpContext.Current.User = (IPrincipal)principal;
                    }
                    else
                    {
                        //The user is unauthorized and return 401 status
                        var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        var tsc = new TaskCompletionSource<HttpResponseMessage>();
                        tsc.SetResult(response);
                        return tsc.Task;
                    }
                }
                else
                {
                    //Bad Request because Authentication header is set but value is null
                    return base.SendAsync(request, cancellationToken);
                }
                return base.SendAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                //User did not set Authentication header
                return base.SendAsync(request, cancellationToken);
            }
        }

        private bool IsLogin(string username, string password)
        {
            using (var pc = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["WindowsDomain"]))
            {
                var isvalid = pc.ValidateCredentials(username, password);
                return isvalid;
            }
        }

        #endregion Methods
    }
}