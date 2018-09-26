using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Session
{
    public class SessionService : Interfaces.ISessionService
    {
        #region Fields

        private readonly HttpClient client = new HttpClient();
        private readonly WebserviceSessionOptions settings;

        #endregion Fields

        #region Constructors

        public SessionService(IOptions<WebserviceSessionOptions> settings)
        {
            this.settings = settings.Value;
            client.BaseAddress = new System.Uri(this.settings.WebServiceURL);
        }

        #endregion Constructors

        #region Methods

        public async Task<byte[]> Get(string key, CancellationToken token = default(CancellationToken))
        {
            string result = string.Empty;
            var response = new HttpResponseMessage();
            response = await client.GetAsync(string.Format("api/Session/{0}", key), token);
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
            }

            return System.Convert.FromBase64String(result);
        }

        public async Task Refresh(string key, CancellationToken token = default(CancellationToken))
        {
            string result = string.Empty;
            var response = new HttpResponseMessage();
            response = await client.GetAsync(string.Format("api/Session/Refresh/{0}", key), token);
        }

        public async Task Set(string key, byte[] data, CancellationToken token = default(CancellationToken))
        {
            string result = string.Empty;
            var response = await this.client.PostAsync(string.Format("api/Session/"), new StringContent(JsonConvert.SerializeObject(new { Key = key, Value = Convert.ToBase64String(data) }), Encoding.UTF8, "application/json"), token);
            if (response.IsSuccessStatusCode)
            {
            }
        }

        #endregion Methods
    }
}