using Microsoft.AspNetCore.DataProtection.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;

namespace IBI.<%= Name %>.Application.Utils.DataProtection
{
    public class WebServiceXMLRepository : IXmlRepository
    {
        #region Fields

        private readonly HttpClient httpClient;
        private readonly string KeyName = string.Empty;
        private readonly string KeyStorageURL = string.Empty;

        #endregion Fields

        #region Constructors

        public WebServiceXMLRepository(string connectionString, string keyName)
        {
            this.KeyStorageURL = connectionString;
            this.KeyName = keyName;

            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.KeyStorageURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion Constructors

        #region Methods

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var collection = new List<XElement>();
            var response = this.httpClient.GetAsync(string.Format("api/DataProtection/{0}", this.KeyName));
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync().Result;
                var listOfKeys = (List<string>)JsonConvert.DeserializeObject(item, typeof(List<string>));
                foreach (var s in listOfKeys ?? new List<string>())
                {
                    collection.Add(XElement.Parse(s));
                }
            }
            return collection.AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            this.httpClient.PostAsync("api/DataProtection/", new StringContent(JsonConvert.SerializeObject(new { this.KeyName, Value = element.ToString(SaveOptions.DisableFormatting) }), System.Text.Encoding.UTF8, "application/json"));
        }

        #endregion Methods
    }
}