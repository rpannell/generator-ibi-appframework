using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Cache
{
    public class CacheService
    {
        #region Fields

        private const string Resource = "api/Cache/";

        private readonly HttpClient client = new HttpClient();

        #endregion Fields

        #region Constructors

        public CacheService(string webServiceUrl)
        {
            client.BaseAddress = new System.Uri(webServiceUrl);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion Constructors

        #region Methods

        public async Task Delete(string key, CancellationToken token = default(CancellationToken))
        {
            var response = await this.client.DeleteAsync(string.Format("{1}{0}", key, Resource), token);
            if (response.IsSuccessStatusCode)
            {
                var item = response.Content.ReadAsStringAsync().Result;
            }
        }

        public void Delete(string key)
        {
            var response = this.client.DeleteAsync(string.Format("{1}{0}", key, Resource));
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync().Result;
            }
        }

        public async Task<byte[]> Get(string key, CancellationToken token = default(CancellationToken))
        {
            string result = string.Empty;
            var response = await client.GetAsync(string.Format("{1}{0}", key, Resource), token);
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
            }

            return System.Convert.FromBase64String(result);
        }

        public byte[] Get(string key)
        {
            string result = string.Empty;
            var response = client.GetAsync(string.Format("{1}{0}", key, Resource));
            if (response.Result.IsSuccessStatusCode)
            {
                result = response.Result.Content.ReadAsStringAsync().Result.Replace("\"", string.Empty);
            }

            return System.Convert.FromBase64String(result);
        }

        public void Refresh(string key)
        {
            var response = this.client.PutAsync(string.Format("{1}{0}", key, Resource),
                                                            new StringContent(JsonConvert.SerializeObject(new { KeyName = key }), Encoding.UTF8, "application/json")).Result;
        }

        public async Task Refresh(string key, CancellationToken token = default(CancellationToken))
        {
            await this.client.PutAsync(string.Format("{1}{0}", key, Resource),
                                                            new StringContent(JsonConvert.SerializeObject(new { KeyName = key }), Encoding.UTF8, "application/json"),
                                                            token);
        }

        public async Task Set(string key, byte[] data, CancellationToken token = default(CancellationToken))
        {
            await this.client.PostAsync(string.Format("{0}", Resource),
                                                            new StringContent(JsonConvert.SerializeObject(new { KeyName = key, Value = Convert.ToBase64String(data) }), Encoding.UTF8, "application/json"),
                                                            token);
        }

        public void Set(string key, byte[] data)
        {
            var response = this.client.PostAsync(string.Format("{0}", Resource),
                                                            new StringContent(JsonConvert.SerializeObject(new { KeyName = key, Value = Convert.ToBase64String(data) }), Encoding.UTF8, "application/json")).Result;
        }

        #endregion Methods
    }
}