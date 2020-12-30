using IBI.<%= Name %>.Application.Models;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.RestClient
{
    /// <summary>
    /// Generic Rest Client
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestClient<T> : IDisposable
    {
        #region Private Variables

        private readonly HttpClient httpClient;
        private readonly string resourceUrl = string.Empty;
        private readonly string url = string.Empty;

        #endregion Private Variables

        #region Constructor

        /// <summary>
        /// Create a RestClient with no authentication
        /// </summary>
        /// <param name="url">The root URL of the service</param>
        /// <param name="resource">The resource to call</param>
        public RestClient(string url, string resource)
        {
            //ensure the resource is setup correctly
            //resource = !resource.StartsWith("api/") ? string.Format("api/{0}", resource) : resource;
            //resource = !resource.EndsWith("/") ? string.Format("{0}/", resource) : resource;
            this.resourceUrl = resource;

            this.url = url;
            this.httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(url))
            {
                this.httpClient.BaseAddress = new Uri(this.url);
            }
            this.httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Creates a RestClient that uses a user name and password for
        /// authentication
        /// </summary>
        /// <param name="url">The root URL of the service</param>
        /// <param name="resource">The resource to call</param>
        /// <param name="username">The username</param>
        /// <param name="role">The comma delimited list of roles</param>
        public RestClient(string url, string resource, string username, string role) : this(url, resource)
        {
            //ensure the resource is setup correctly
            this.httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, role))));
        }

        /// <summary>
        /// Creates RestClient that uses a token for authentication
        /// </summary>
        /// <param name="url">The root URL of the service</param>
        /// <param name="resource">The resource to call</param>
        /// <param name="token">The user token</param>
        public RestClient(string url, string resource, string token) : this(url, resource)
        {
            this.httpClient.SetBearerToken(token);
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Used to create a Rest call to a service that doesn't correspond to our standard
        ///
        /// The action name parameter may include the query parameters if the url parameters
        /// is null
        /// </summary>
        /// <typeparam name="TObject">The return object from the service</typeparam>
        /// <param name="method">The verb used to call a serice (Get, Post, Put, Delete)</param>
        /// <param name="action">The name of the custom action on the entity's controller</param>
        /// <param name="data">The data to serialize and add to the body</param>
        /// <param name="args">The URL parameters in order to add to the url</param>
        /// <returns>TObject</returns>
        public TObject Custom<TObject>(HttpMethod method, string action = "", object data = null, params object[] args)
        {
            var response = this.CreateResponse(method, action, data, args);
            var results = this.HandleReturn<TObject>(response);
            return results;
        }

        /// <summary>
        /// Used to create a Rest call to a service that doesn't correspond to our standard
        ///
        /// The action name parameter may include the query parameters if the url parameters
        /// is null
        /// </summary>
        /// <param name="method">The verb used to call a serice (Get, Post, Put, Delete)</param>
        /// <param name="action">The name of the custom action on the entity's controller</param>
        /// <param name="data">The data to serialize and add to the body</param>
        /// <param name="args">The URL parameters in order to add to the url</param>
        public void Custom(HttpMethod method, string action = "", object data = null, params object[] args)
        {
            var response = this.CreateResponse(method, action, data, args);
            this.HandleReturn(response);
        }

        /// <summary>
        /// Get an entity from an action name and list of url parameters
        /// </summary>
        /// <param name="action">The action name</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>T</returns>
        public T GetFromAction(string action, params object[] args)
        {
            var response = this.CreateResponse(HttpMethod.Get, action, null, args);
            var results = this.HandleReturn<T>(response);
            return results;
        }

        /// <summary>
        /// Get an entity from an action name and list of url parameters
        /// </summary>
        /// <param name="action">The action name</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>T</returns>
        public async Task<T> GetFromActionAsync(string action, params object[] args)
        {
            var response = await this.CreateResponse(HttpMethod.Get, action, null, args);
            var results = this.HandleReturnAsync<T>(response);
            return results;
        }

        /// <summary>
        /// Get a list of entities from an action name and list of url parameters
        /// </summary>
        /// <param name="action">The action name</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>List of entities</returns>
        public List<T> GetListFromAction(string action, params object[] args)
        {
            var response = this.CreateResponse(HttpMethod.Get, action, null, args);
            var results = this.HandleReturn<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Get a list of entities from an action name and list of url parameters
        /// </summary>
        /// <param name="action">The action name</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>List of entities</returns>
        public async Task<List<T>> GetListFromActionAsync(string action, params object[] args)
        {
            var response = await this.CreateResponse(HttpMethod.Get, action, null, args);
            var results = this.HandleReturnAsync<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Post to an action name with some url parameters and return a list of entities
        /// </summary>
        /// <param name="action">The action name</param>
        /// <param name="obj">The object to send to the controller</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>T</returns>
        public T PostGetFromAction(string action, object obj, params object[] args)
        {
            var response = this.CreateResponse(HttpMethod.Post, action, obj, args);
            var results = this.HandleReturn<T>(response);
            return results;
        }

        /// <summary>
        /// Post to an action name with some url parameters and return a list of entities
        /// </summary>
        /// <param name="action">The action name</param>
        /// <param name="obj">The object to send to the controller</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>T</returns>
        public async System.Threading.Tasks.Task<T> PostGetFromActionAsync(string action, object obj, params object[] args)
        {
            var response = await this.CreateResponse(HttpMethod.Post, action, obj, args);
            return this.HandleReturnAsync<T>(response);
        }

        /// <summary>
        /// Post to an action name with some url parameters and return a list of entities
        /// </summary>
        /// <param name="action">The action name</param>
        /// /// <param name="obj">The object to send to the controller</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>List of entities</returns>
        public List<T> PostGetListFromAction(string action, object obj, params object[] args)
        {
            var response = this.CreateResponse(HttpMethod.Post, action, obj, args);
            var results = this.HandleReturn<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Post to an action name with some url parameters and return a list of entities
        /// </summary>
        /// <param name="action">The action name</param>
        /// /// <param name="obj">The object to send to the controller</param>
        /// <param name="args">List of url parameters</param>
        /// <returns>List of entities</returns>
        public async System.Threading.Tasks.Task<List<T>> PostGetListFromActionAsync(string action, object obj, params object[] args)
        {
            var response = await this.CreateResponse(HttpMethod.Post, action, obj, args);
            var results = this.HandleReturnAsync<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Post to an action name with some url parameters and doesn't return anything
        /// </summary>
        /// <param name="action">The action name</param>
        /// /// <param name="obj">The object to send to the controller</param>
        /// <param name="args">List of url parameters</param>
        public void PostToAction(string action, object obj, params object[] args)
        {
            var response = this.CreateResponse(HttpMethod.Post, action, obj, args);
            this.HandleReturn(response);
        }

        /// <summary>
        /// Post to an action name with some url parameters and doesn't return anything
        /// </summary>
        /// <param name="action">The action name</param>
        /// /// <param name="obj">The object to send to the controller</param>
        /// <param name="args">List of url parameters</param>
        public async Task PutToActionAsync(string action, object obj, params object[] args)
        {
            var response = await this.CreateResponse(HttpMethod.Put, action, obj, args);
            this.HandleReturnAsync(response);
        }

        #endregion Methods

        #region Get Actions

        /// <summary>
        /// Get the entity by the primary key when the key is an int
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity</returns>
        public virtual T Get(int id)
        {
            var response = this.CreateResponse(HttpMethod.Get, string.Empty, null, id);
            var results = this.HandleReturn<T>(response);
            return results;
        }

        /// <summary>
        /// Get the entity by the primary key when the key is an int
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity</returns>
        public virtual async Task<T> GetAsync(int id)
        {
            var response = await this.CreateResponse(HttpMethod.Get, string.Empty, null, id);
            var results = this.HandleReturnAsync<T>(response);
            return results;
        }

        /// <summary>
        /// Get the entity by the primary key when the key is a bigint
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity</returns>
        public virtual T Get(long id)
        {
            var response = this.CreateResponse(HttpMethod.Get, string.Empty, null, id);
            var results = this.HandleReturn<T>(response);
            return results;
        }

        /// <summary>
        /// Get all of the records for that entity
        /// </summary>
        /// <returns>List of the entity</returns>
        public virtual List<T> Get()
        {
            var response = this.CreateResponse(HttpMethod.Get);
            var results = this.HandleReturn<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Get all of the records for that entity
        /// </summary>
        /// <returns>List of the entity</returns>
        public virtual async Task<List<T>> GetAsync()
        {
            var response = await this.CreateResponse(HttpMethod.Get);
            var results = this.HandleReturnAsync<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Get a page from an List of AdvancedSearch model
        /// </summary>
        /// <param name="advancedSearch"></param>
        /// <param name="limit">The size of the page</param>
        /// <param name="offset">The row number of the offset</param>
        /// <param name="search">The search string</param>
        /// <param name="sort">The sort string</param>
        /// <param name="order">The order direction</param>
        /// <param name="type">Used to help filter the type of advanced search if needed else it's just 0</param>
        /// <returns>PaginationResult T></returns>
        public virtual PaginationResult<T> GetAdvancedPage(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0)
        {
            var response = this.CreateResponse(HttpMethod.Post,
                                        "AdvancedPage",
                                        new { SearchLimit = limit, SearchOffSet = offset, SearchString = search, SortString = sort, SortOrder = order, AdvancedSearch = advancedSearch });
            var results = this.HandleReturn<PaginationResult<T>>(response);
            return results;
        }

        /// <summary>
        /// Get a page from an List of AdvancedSearch model
        /// </summary>
        /// <param name="advancedSearch"></param>
        /// <param name="limit">The size of the page</param>
        /// <param name="offset">The row number of the offset</param>
        /// <param name="search">The search string</param>
        /// <param name="sort">The sort string</param>
        /// <param name="order">The order direction</param>
        /// <param name="type">Used to help filter the type of advanced search if needed else it's just 0</param>
        /// <returns>PaginationResult T></returns>
        public virtual async Task<PaginationResult<T>> GetAdvancedPageAsync(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0)
        {
            var response = await this.CreateResponse(HttpMethod.Post,
                                        "AdvancedPage",
                                        new { SearchLimit = limit, SearchOffSet = offset, SearchString = search, SortString = sort, SortOrder = order, AdvancedSearch = advancedSearch });
            var results = this.HandleReturnAsync<PaginationResult<T>>(response);
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
            var response = this.CreateResponse(HttpMethod.Post, "GetAutoComplete", new { Length = length, SearchTerm = searchTerm }, type);
            var results = this.HandleReturn<List<T>>(response);
            return results;
        }

        /// <summary>
        /// Get the AutoComplete list for a specific entity based on the resource url
        /// </summary>
        /// <param name="searchTerm">The search term from the UI</param>
        /// <param name="length">Defaults to 20</param>
        /// <returns></returns>
        public virtual async Task<List<T>> GetAutoCompleteAsync(string searchTerm, int length = 20, int type = 0)
        {
            var response = await this.CreateResponse(HttpMethod.Post, "GetAutoComplete", new { Length = length, SearchTerm = searchTerm }, type);
            var results = this.HandleReturnAsync<List<T>>(response);
            return results;
        }

        [Obsolete("Get extra is no longer supported, please use a custom get route")]
        public virtual List<T> GetExtra(int id, string extra)
        {
            try
            {
                var response = this.CreateResponse(HttpMethod.Get, string.Format("{0}?extra={1}", id, extra));
                var results = this.HandleReturn<List<T>>(response);
                return results;
            }
            catch (System.Exception)
            {
                return new List<T>();
            }
        }

        [Obsolete("Get extra is no longer supported, please use a custom get route")]
        public virtual List<T> GetExtra(string id, string extra)
        {
            var response = this.CreateResponse(HttpMethod.Get, string.Format("Get/?id={0}&extra={1}", id, extra));
            var results = this.HandleReturn<List<T>>(response);
            return results;
        }

        public T GetFromUrlAction(string action, params UrlParameters[] args)
        {
            var response = this.CreateUrlResponse(HttpMethod.Get, action, null, args);
            var results = this.HandleReturn<T>(response);
            return results;
        }

        /// <summary>
        /// Get a page of data from an entity
        /// </summary>
        /// <param name="limit">The size of a page</param>
        /// <param name="offset">The row number to offset a page</param>
        /// <param name="search">The search string</param>
        /// <param name="sort">The sort Property</param>
        /// <param name="order">The sort direction</param>
        /// <returns>PaginationResult T</returns>
        public virtual PaginationResult<T> GetPage(int limit, int offset, string search = null, string sort = null, string order = null)
        {
            var response = this.CreateResponse(HttpMethod.Get, string.Format("GetPage?limit={0}&offset={1}&search={2}&sort={3}&sortOrder={4}", limit, offset, search, sort, order));
            var results = this.HandleReturn<PaginationResult<T>>(response);
            return results;
        }

        /// <summary>
        /// Get a page of data from an entity
        /// </summary>
        /// <param name="limit">The size of a page</param>
        /// <param name="offset">The row number to offset a page</param>
        /// <param name="search">The search string</param>
        /// <param name="sort">The sort Property</param>
        /// <param name="order">The sort direction</param>
        /// <returns>PaginationResult T</returns>
        public virtual async Task<PaginationResult<T>> GetPageAsync(int limit, int offset, string search = null, string sort = null, string order = null)
        {
            var response = await this.CreateResponse(HttpMethod.Get, string.Format("GetPage?limit={0}&offset={1}&search={2}&sort={3}&sortOrder={4}", limit, offset, search, sort, order));
            var results = this.HandleReturnAsync<PaginationResult<T>>(response);
            return results;
        }
        
        #endregion Get Actions

        #region Post Actions

        /// <summary>
        /// Post an entity to the web service
        /// </summary>
        /// <param name="data">The entity</param>
        public virtual void Post(T data)
        {
            var response = this.CreateResponse(HttpMethod.Post, string.Empty, data);
            this.HandleReturn(response);
        }

        /// <summary>
        /// Post an entity to the web service
        /// </summary>
        /// <param name="data">The entity</param>
        public virtual async Task PostAsync(T data)
        {
            var response = await this.CreateResponse(HttpMethod.Post, string.Empty, data);
            this.HandleReturnAsync(response);
        }

        [Obsolete("Post extra is no longer supported and should use a custom post route instead")]
        public virtual void Post(int id, string extra, object data)
        {
            var actionName = string.Format("?id={0}&extra={1}", id, extra);
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, this.resourceUrl + actionName);
            requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = this.httpClient.SendAsync(requestMessage);
            this.HandleReturn(response);
        }

        /// <summary>
        /// Post a list of entities to the PostList action
        /// </summary>
        /// <param name="data">List of entities</param>
        public virtual void PostList(List<T> data)
        {
            this.PostToAction("PostList", data);
        }

        /// <summary>
        /// Post an entity to the service and return the entity back
        /// </summary>
        /// <param name="data"></param>
        /// <returns>T</returns>
        public virtual T PostReturnId(T data)
        {
            var response = this.CreateResponse(HttpMethod.Post, string.Empty, data);
            var results = this.HandleReturn<T>(response);
            return results;
        }

        /// <summary>
        /// Post an entity to the service and return the entity back
        /// </summary>
        /// <param name="data"></param>
        /// <returns>T</returns>
        public virtual async Task<T> PostReturnIdAsync(T data)
        {
            var response = await this.CreateResponse(HttpMethod.Post, string.Empty, data);
            var results = this.HandleReturnAsync<T>(response);
            return results;
        }

        #endregion Post Actions

        #region Put Actions

        /// <summary>
        /// Send an entity to be updated to the service
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <param name="data">The entity to update</param>
        public virtual void Put(int id, T data)
        {
            var response = this.CreateResponse(HttpMethod.Put, string.Empty, data, id);
            this.HandleReturn(response);
        }

        /// <summary>
        /// Send an entity to be updated to the service
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <param name="data">The entity to update</param>
        public virtual async Task PutAsync(int id, T data)
        {
            var response = await this.CreateResponse(HttpMethod.Put, string.Empty, data, id);
            this.HandleReturnAsync(response);
        }

        /// <summary>
        /// Send an entity to be updated to the service
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        /// <param name="data">The entity to update</param>
        public virtual void Put(long id, T data)
        {
            var response = this.CreateResponse(HttpMethod.Put, string.Empty, data, id);
            this.HandleReturn(response);
        }

        #endregion Put Actions

        #region Extension Helper Functions

        /// <summary>
        /// Gets the client
        /// </summary>
        /// <returns>HttpClient</returns>
        public HttpClient GetClient()
        {
            return this.httpClient;
        }

        /// <summary>
        /// Gets the resource url
        /// </summary>
        /// <returns>string</returns>
        public string GetResource()
        {
            return this.resourceUrl;
        }

        #endregion Extension Helper Functions

        #region Delete Actions

        /// <summary>
        /// Call the delete action on an entity by the primary key
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        public virtual void Delete(int id)
        {
            var response = this.CreateResponse(HttpMethod.Delete, string.Empty, null, id);
            this.HandleReturn(response);
        }

        /// <summary>
        /// Call the delete action on an entity by the primary key
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        public virtual async Task DeleteAsync(int id)
        {
            var response = await this.CreateResponse(HttpMethod.Delete, string.Empty, null, id);
            this.HandleReturnAsync(response);
        }

        /// <summary>
        /// Call the delete action on an entity by the primary key
        /// </summary>
        /// <param name="id">The primary key of the entity</param>
        public virtual void Delete(long id)
        {
            var response = this.CreateResponse(HttpMethod.Delete, string.Empty, null, id);
            this.HandleReturn(response);
        }

        #endregion Delete Actions

        #region Private functions

        /// <summary>
        /// Used to create a HttpResponseMessage for a specific Verb (Get, Post, Delete, Put)
        /// A action name (default to empty string)
        /// Data to add to the body
        /// and list of urm parameters
        /// </summary>
        /// <param name="method">The type of verb</param>
        /// <param name="action">The name of the action (defaults to empty string)</param>
        /// <param name="data">The data to serialize as a json and add to the body</param>
        /// <param name="args">The list of url parameters</param>
        /// <returns>Task of HttpResponseMessage</returns>
        public Task<HttpResponseMessage> CreateUrlResponse(HttpMethod method, string action = "", object data = null, params UrlParameters[] args)
        {
            var actionName = this.GetUrlActionName(action, args);
            var requestMessage = new HttpRequestMessage(method, actionName);
            if (data != null)
            {
                requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }
            return this.httpClient.SendAsync(requestMessage);
        }

        /// <summary>
        /// Gets the url based an action name and parameter arguments
        /// The name of the action can include the query string parameters as long
        /// if there aren't any url parameters passed in
        /// </summary>
        /// <param name="actionName">The name of the action</param>
        /// <param name="args">The list of arguments</param>
        /// <returns>string</returns>
        public string GetActionName(string actionName, params object[] args)
        {
            var actionurl = this.resourceUrl.EndsWith("/") ? this.resourceUrl : string.Format("{0}/", this.resourceUrl);
            if (actionName != string.Empty)
            {
                actionurl = string.Format("{0}{1}/", actionurl, actionName);
            }

            if (args != null && args.Length > 0)
            {
                foreach (var arg in args)
                {
                    actionurl = string.Format("{0}{1}/", actionurl, arg);
                }
            }
            //else
            //{
            actionurl = actionurl.Substring(0, actionurl.Length - 1);
            //}

            return actionurl;
        }

        /// <summary>
        /// Used to create a HttpResponseMessage for a specific Verb (Get, Post, Delete, Put)
        /// A action name (default to empty string)
        /// Data to add to the body
        /// and list of urm parameters
        /// </summary>
        /// <param name="method">The type of verb</param>
        /// <param name="action">The name of the action (defaults to empty string)</param>
        /// <param name="data">The data to serialize as a json and add to the body</param>
        /// <param name="args">The list of url parameters</param>
        /// <returns>Task of HttpResponseMessage</returns>
        private async Task<HttpResponseMessage> CreateResponse(HttpMethod method, string action = "", object data = null, params object[] args)
        {
            var actionName = this.GetActionName(action, args);
            var requestMessage = new HttpRequestMessage(method, actionName);
            if (data != null)
            {
                requestMessage.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            }
            return await this.httpClient.SendAsync(requestMessage);
        }

        /// <summary>
        /// Handles the return of a HttpClient response
        /// </summary>
        /// <typeparam name="TObject">The object to return</typeparam>
        /// <param name="response">The response from a HttpClient</param>
        /// <returns>TObject</returns>
        private TObject HandleReturn<TObject>(Task<HttpResponseMessage> response)
        {
            var results = default(TObject);
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
                results = (TObject)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(TObject));
            }
            else if (response.Result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //Log.Error(response.Result.Content.ReadAsStringAsync().Result);
            }
            return results;
        }

        private TObject HandleReturnAsync<TObject>(HttpResponseMessage response)
        {
            var results = default(TObject);
            if (response.IsSuccessStatusCode)
            {
                var item = response.Content.ReadAsStringAsync();
                results = (TObject)Newtonsoft.Json.JsonConvert.DeserializeObject(item.Result, typeof(TObject));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //Log.Error(response.Result.Content.ReadAsStringAsync().Result);
            }
            return results;
        }

        /// <summary>
        /// Handle a response that doesn't return anything
        /// </summary>
        /// <param name="response"></param>
        private void HandleReturnAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var item = response.Content.ReadAsStringAsync();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //Log.Error(response.Result.Content.ReadAsStringAsync().Result);
            }
        }

        /// <summary>
        /// Handle a response that doesn't return anything
        /// </summary>
        /// <param name="response"></param>
        private void HandleReturn(Task<HttpResponseMessage> response)
        {
            if (response.Result.IsSuccessStatusCode)
            {
                var item = response.Result.Content.ReadAsStringAsync();
            }
            else if (response.Result.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                //Log.Error(response.Result.Content.ReadAsStringAsync().Result);
            }
        }

        #endregion Private functions

        /// <summary>
        /// GEt the URL Action by name and url args
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetUrlActionName(string actionName, params UrlParameters[] args)
        {
            var actionUrl = Path.Combine(this.resourceUrl, actionName);
            var urlParams = string.Empty;
            if (args != null)
            {
                foreach (var up in args)
                {
                    urlParams += string.Format("{0}{1}={2}",
                                                urlParams == string.Empty ? "?" : "&",
                                                up.Name,
                                                up.Value);
                }
            }

            return string.Format("{0}{1}", actionUrl, urlParams);
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #region Classes

        public class UrlParameters
        {
            #region Constructors

            public UrlParameters(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }

            #endregion Constructors

            #region Properties

            public string Name { get; set; }
            public string Value { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}