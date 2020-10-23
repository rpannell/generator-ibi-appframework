using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Session
{
    public class CookieStore : ITicketStore
    {
        #region Fields

        private const string Resource = "/api/CookieStore/";
        private readonly string CookieUrl = string.Empty;
        private readonly string InstanceName = string.Empty;
        private HttpClient httpClient;

        #endregion Fields

        // override default so that do not interfere with anyone else's server

        #region Constructors

        public CookieStore(string connectionString, string instanceName)
        {
            this.CookieUrl = connectionString;
            this.InstanceName = instanceName;
            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.CookieUrl);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion Constructors

        #region Methods

        public Task RemoveAsync(string key)
        {
            this.Delete(string.Format("{0}:{1}", this.InstanceName, key));
            return Task.FromResult(0);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            this.Renew(key, ticket);

            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            var str = this.Get(string.Format("{0}:{1}", this.InstanceName, key));
            var ba = Convert.FromBase64String(string.IsNullOrEmpty(str) ? "" : str);
            var ticket = DeserializeFromBytes(ba);
            return Task.FromResult(ticket);
        }

        public Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            if (ticket.Principal.HasClaim(x => x.Type == "UserId"))
            {
                var userId = ticket.Principal.FindFirstValue("UserId");
                var guid = Guid.NewGuid();
                var key = string.Format("{0}:{1}", userId, guid);
                this.Set(Convert.ToInt32(userId), guid, ticket);
                return Task.FromResult(key);
            }
            return Task.FromResult(string.Empty);
        }

        private static AuthenticationTicket DeserializeFromBytes(byte[] source)
        {
            try
            {
                return source == null || source.Length == 0 ? null : TicketSerializer.Default.Deserialize(source);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static byte[] SerializeToBytes(AuthenticationTicket source)
        {
            return TicketSerializer.Default.Serialize(source);
        }

        #endregion Methods

        private string Delete(string key)
        {
            var response = this.httpClient.DeleteAsync(string.Format("{0}{1}", Resource, key));
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync().Result;
                return item;
            }
            return string.Empty;
        }

        private string Get(string key)
        {
            var response = this.httpClient.GetAsync(string.Format("{0}{1}", Resource, key));
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync().Result;
                return (string)JsonConvert.DeserializeObject(item, typeof(string));
            }
            return string.Empty;
        }

        private string Renew(string key, AuthenticationTicket source)
        {
            var value = Convert.ToBase64String(SerializeToBytes(source));
            var response = this.httpClient.PutAsync(string.Format("{0}{1}", Resource, key),
                                                new StringContent(JsonConvert.SerializeObject(new { InstanceName = this.InstanceName, Value = value, CacheTime = TimeSpan.FromHours(10) }), Encoding.UTF8, "application/json"));
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync().Result;
                return item;
            }
            return string.Empty;
        }

        private string Set(int userId, Guid guid, AuthenticationTicket source)
        {
            var value = Convert.ToBase64String(SerializeToBytes(source));
            var response = this.httpClient.PostAsync(string.Format("{0}", Resource), new StringContent(JsonConvert.SerializeObject(new { InstanceName = this.InstanceName, UserId = userId, Guid = guid, Value = value, CacheTime = TimeSpan.FromHours(10) }), Encoding.UTF8, "application/json"));
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync().Result;
                return item;
            }
            return string.Empty;
        }
    }
}