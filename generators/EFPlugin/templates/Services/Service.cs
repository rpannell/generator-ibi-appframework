<% if(isPlugin) { %>using IBI.<%= projectname %>.Plugin.Models.Entities;<% } else { %>using IBI.<%= projectname %>.Application.Models.Entities;<%}%>
<% if(isPlugin) { %>using IBI.<%= projectname %>.Plugin.Models;<% } else { %>using IBI.<%= projectname %>.Application.Models;<%}%>
<% if(isPlugin) { %>using InterlineBrands.Platform.Core.Settings;<% } else { }%>
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
/// Add any necessarsy custom functions to needed to communicate
/// to the <%= entityinfo.PropertyName %> web-api service
/// </summary>
<% if(isPlugin) { %>namespace IBI.<%= projectname %>.Plugin.Services<% } else { %>namespace IBI.<%= projectname %>.Application.Services<%}%>
{
	/// <summary>
    /// The plugin service that manages the <%= entityinfo.PropertyName %> entity via the web-api service connected to this plugin
    /// </summary>
    public class <%= entityinfo.PropertyName %>Service : BaseService, Interfaces.I<%= entityinfo.PropertyName %>Service
    {
		#region Fields
		/// <summary>
        /// The RestClient for <%= entityinfo.PropertyName %> entity
        /// </summary>
        private RestClient.<%= entityinfo.PropertyName %>RestClient serviceClient = null;

		#endregion Fields
		
		#region Constructors
		
		<% if(isPlugin) { %>
		/// <summary>
        /// Creates a(n) <%= entityinfo.PropertyName %>Service with the help of the IPluginSettings
        /// </summary>
        public <%= entityinfo.PropertyName %>Service(IPluginSettings pluginSettings)
            : base(pluginSettings)
        {
            this.serviceClient = new RestClient.<%= entityinfo.PropertyName %>RestClient(this.URL, "api/<%= entityinfo.PropertyName %>/", this.UserName, this.Role);
        }
		<% } else { %>
		/// <summary>
        /// Creates a(n) <%= entityinfo.PropertyName %>Service with the help of the IPluginSettings
        /// </summary>
        public <%= entityinfo.PropertyName %>Service()
            : base()
        {
            this.serviceClient = new RestClient.<%= entityinfo.PropertyName %>RestClient(this.URL, "api/<%= entityinfo.PropertyName %>/", this.UserName, this.Role);
        }
		<%}%>
		
		#endregion Constructors
		
		#region CRUD Operations
		/// <summary>
        /// Get all of the <%= entityinfo.PropertyName %> entity from the database
        /// </summary>
        /// <returns>List of <%= entityinfo.PropertyName %></returns>
		public List<<%= entityinfo.PropertyName %>> Get()
        {
			return ServiceCache.Get<List<<%= entityinfo.PropertyName %>>>("All<%= entityinfo.PropertyName %>", () => this.serviceClient.Get());
        }
		
		/// <summary>
        /// Returns a single <%= entityinfo.PropertyName %> entity by it's primary key
        /// </summary>
        /// <returns><%= entityinfo.PropertyName %></returns>
		public <%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> Id)
        {
            return this.serviceClient.Get(Id);
        }

		/// <summary>
        /// Deletes a(n) <%= entityinfo.PropertyName %> entity from the databaes
        /// </summary>
        /// <param name="Id">The primary key of the <%= entityinfo.PropertyName %> to delete</param>
		public void Delete(<%= entityinfo.PrimaryKey %> Id)
        {
            this.serviceClient.Delete(Id);
        }

		/// <summary>
        /// Inserts a(n) <%= entityinfo.PropertyName %> record to the database and 
        /// returns the same entity back with it's new primary 
        /// key filled in
        /// </summary>
        /// <param name="entity">The new <%= entityinfo.PropertyName %> record to create</param>
        /// <returns><%= entityinfo.PropertyName %></returns>
		public <%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> entity)
        {
            return this.serviceClient.PostReturnId(entity);
        }

		/// <summary>
        /// Updates a(n) <%= entityinfo.PropertyName %> record on the database
        /// </summary>
        /// <param name="entity">The <%= entityinfo.PropertyName %> entity to update</param>
		public void Update(<%= entityinfo.PropertyName %> entity)
        {
            this.serviceClient.Put(entity.<%= entityinfo.PrimaryName %>, entity);
        }

		#endregion CRUD Operations
		
		#region Paging Operations
		
		/// <summary>
        /// Get a page of the <%= entityinfo.PropertyName %> entity from the database 
        /// with the standard paging
        /// </summary>
        /// <param name="limit">The size of the page</param>
        /// <param name="offset">The row offset</param>
        /// <param name="search">The search string if any</param>
        /// <param name="sort">The name of the property to sort by</param>
        /// <param name="order">The order direction to sort the property (asc/desc)</param>
        /// <returns>PaginationResult of <%= entityinfo.PropertyName %></returns>
		public PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int limit, int offset, string search, string sort, string order)
        {
            return this.serviceClient.GetPage(limit, offset, search, sort, order);
        }

		/// <summary>
        /// Gets a page of the <%= entityinfo.PropertyName %> entity with
        /// an advanced query to help filter the data
        /// </summary>
        /// <param name="advancedSearch">List of AdvancedSearch properties</param>
        /// <param name="limit">The size of the page</param>
        /// <param name="offset">The row offset</param>
        /// <param name="search">The search string if any</param>
        /// <param name="sort">The name of the property to sort by</param>
        /// <param name="order">The order direction to sort the property (asc/desc)</param>
        /// <param name="type">Helps with filtering the data further</param>
        /// <returns>PaginationResult of <%= entityinfo.PropertyName %></returns>
		public PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0)
        {
            return this.serviceClient.GetAdvancedPage(advancedSearch, limit, offset, search, sort, order, type);
        }
		
		#endregion Paging Operations
		
		#region Auto Complete
		
		/// <summary>
        /// Returns a list of the <%= entityinfo.PropertyName %> entity by the autocomplete
        /// properties by the term
        /// </summary>
        /// <param name="term">The search term to filter the records of the <%= entityinfo.PropertyName %> entity</param>
        /// <param name="length">The number of records to return</param>
        /// <param name="type">The type to return</param>
        /// <returns>List of <%= entityinfo.PropertyName %></returns>
		public List<<%= entityinfo.PropertyName %>> GetAutocomplete(string term, int length = 20, int type = 0)
        {
            return this.serviceClient.GetAutoComplete(term, length, type);
        }
		
		#endregion

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}