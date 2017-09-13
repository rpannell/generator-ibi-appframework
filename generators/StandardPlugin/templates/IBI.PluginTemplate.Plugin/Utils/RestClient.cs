using IBI.<%= Name %>.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace IBI.<%= Name %>.Plugin.Utils
{
    public class RestClient<T>
    {
        #region Private Variables

        private string resourceUrl;
        private string url = string.Empty;
        private string user = string.Empty;
        private string password = string.Empty;

        private HttpClient httpClient;

        private enum ResourceType { Post, Get, Delete, GetPage, Put, GetAll };

        #endregion Private Variables

        #region Constructor

        public RestClient(string url, string resource, string username, string role)
        {
            //ensure the resource is setup correctly
            resource = !resource.StartsWith("api/") ? string.Format("api/{0}", resource) : resource;
            resource = !resource.EndsWith("/") ? string.Format("{0}/", resource) : resource;

            resourceUrl = resource;

            this.url = url;
            this.user = username;
            this.password = role;
            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(this.url);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", this.user, this.password))));
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #endregion Constructor

        public List<T> GetListFromAction(string action, params object[] args)
        {
            var actionName = this.resourceUrl.EndsWith("/") ? this.resourceUrl : string.Format("{0}/", this.resourceUrl);
            actionName = string.Format("{0}{1}/", actionName, action);
            foreach (var arg in args)
            {
                actionName = string.Format("{0}{1}/", actionName, arg);
            }
            var results = (List<T>)null;
            var response = this.httpClient.GetAsync(actionName);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(List<T>));
            }

            return results;
        }

        public T GetFromAction(string action, params object[] args)
        {
            var actionName = this.resourceUrl.EndsWith("/") ? this.resourceUrl : string.Format("{0}/", this.resourceUrl);
            actionName = string.Format("{0}{1}/", actionName, action);
            foreach (var arg in args)
            {
                actionName = string.Format("{0}{1}/", actionName, arg);
            }
            var results = default(T);
            var response = this.httpClient.GetAsync(actionName);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(T));
            }

            return results;
        }

        #region Get Actions

        /*
         * getAll return a list of folders for a given oData query
         * i.e query = ?$filter=startswith(tolower(Active_Directory_group),'moorestown\idw_team')
         */

        public virtual List<T> GetAll(string query = "")
        {
            var results = (List<T>)null;
            var response = this.httpClient.GetAsync(this.resourceUrl);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(List<T>));
            }

            return results;
        }

        public virtual PaginationResult<T> GetPage(int limit, int offset, string search = null, string sort = null, string order = null)
        {
            var actionName = string.Format("GetPage?limit={0}&offset={1}&search={2}&sort={3}&sortOrder={4}", limit, offset, search, sort, order);
            var results = (PaginationResult<T>)null;
            var response = this.httpClient.GetAsync(resourceUrl + actionName);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (PaginationResult<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(PaginationResult<T>));
            }
            return results;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="advancedSearch"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <param name="type">Used to help filter the type of advanced search if needed else it's just 0</param>
        /// <returns></returns>
        public virtual PaginationResult<T> GetAdvancedPage(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0)
        {
            var results = (PaginationResult<T>)null;
            var response = this.httpClient.PostAsJsonAsync(resourceUrl + string.Format("AdvancedPage/{0}/", type), new { SearchLimit = limit, SearchOffSet = offset, SearchString = search, SortString = sort, SortOrder = order, AdvancedSearch = advancedSearch });
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (PaginationResult<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(PaginationResult<T>));
            }
            return results;
        }

        /// <summary>
        /// Get the AutoComplete list for a specific entity based on the resource url
        /// </summary>
        /// <param name="searchTerm">The search term from the UI</param>
        /// <param name="length">Defaults to 20</param>
        /// <returns></returns>
        public virtual List<T> GetAutoComplete(string searchTerm, int length = 20, int type = 0)
        {
            var results = (List<T>)null;
            var response = this.httpClient.PostAsJsonAsync(resourceUrl + string.Format("GetAutoComplete/{0}/", type),
                new { Length = length, SearchTerm = searchTerm });
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(List<T>));
            }
            return results;
        }

        public virtual T Get(int id)
        {
            var actionName = string.Format("{0}", id);
            var response = this.httpClient.GetAsync(resourceUrl + actionName);
            var results = default(T);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(T));
            }
            return results;
        }

        public virtual List<T> GetExtra(int id, string extra)
        {
            try
            {
                var actionName = string.Format("{0}?extra={1}", id, extra);
                var response = this.httpClient.GetAsync(resourceUrl + actionName);
                var results = (List<T>)null;
                if (response.Result.IsSuccessStatusCode)
                {
                    var item = response.Result.Content.ReadAsStringAsync();
                    results = (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(List<T>));
                }
                return results;
            }
            catch (System.Exception)
            {
                return new List<T>();
            }
        }

        public virtual List<T> GetExtra(string id, string extra)
        {
            var actionName = string.Format("Get/?id={0}&extra={1}", id, extra);
            var response = this.httpClient.GetAsync(resourceUrl + actionName);
            var results = (List<T>)null;
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(List<T>));
            }
            return results;
        }

        #endregion Get Actions

        #region Post Actions

        public virtual void Post(T data)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, this.resourceUrl);
            requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = this.httpClient.SendAsync(requestMessage);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
            }
        }

        public virtual void Post(int id, string extra, object data)
        {
            var actionName = string.Format("?id={0}&extra={1}", id, extra);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, this.resourceUrl + actionName);
            requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = this.httpClient.SendAsync(requestMessage);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
            }
        }

        public virtual T PostReturnId(T data)
        {
            var results = default(T);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, this.resourceUrl);
            requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = this.httpClient.SendAsync(requestMessage);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (T)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(T));
            }
            return results;
        }

        public virtual void PostList(List<T> data)
        {
            var actionName = this.resourceUrl.EndsWith("/") ? this.resourceUrl : string.Format("{0}/", this.resourceUrl);
            actionName = string.Format("{0}{1}/", actionName, "PostList");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, actionName);
            requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = this.httpClient.SendAsync(requestMessage);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
            }
        }

        #endregion Post Actions

        #region Put Actions

        public virtual void Put(int id, T data)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, resourceUrl + id);
            requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = this.httpClient.SendAsync(requestMessage);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
            }
        }

        #endregion Put Actions

        #region Extension Helper Functions

        public string GetResource()
        {
            return this.resourceUrl;
        }

        public HttpClient GetClient()
        {
            return this.httpClient;
        }

        #endregion Extension Helper Functions

        #region Delete Actions

        public virtual void Delete(int id)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, resourceUrl + id);
            var response = this.httpClient.SendAsync(requestMessage);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
            }
        }

        #endregion Delete Actions
    }
}