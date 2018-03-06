using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Controllers;
using IBI.<%= projectname %>.Service.Core.Models;
using System.Collections.Generic;
using System.Web.Http;

/*
 *  Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
 * 
 * 	Add any necessary custom controller actions should be added to 
 *  this file
 *  
 *  This controller should contain NO business logic and should be very small
 *  and mainly pass the information sent to the action down to the service layer
 *  The BaseController contains Get/Put/Post/Delete/GetPage/GetAutocomplete.
 *  The service for this entity can be found using the "this.CurrentService" variable
 */
namespace IBI.<%= projectname %>.Service.Controllers
{
    /// <summary>
    /// Controller for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
	[RoutePrefix("api/<%= entityinfo.PropertyName %>")]
    public partial class <%= entityinfo.PropertyName %>Controller : BaseController<I<%= entityinfo.PropertyName %>Service, I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        #region Constructors
		
        /// <summary>
        /// Normal constructor for the <see cref="<%= entityinfo.PropertyName %>Controller"/> that sets up the service
        /// </summary>
        /// <param name="service"><see cref="I<%= entityinfo.PropertyName %>Service"/></param>
		public <%= entityinfo.PropertyName %>Controller(I<%= entityinfo.PropertyName %>Service service)
        {
            this.CurrentService = service;
        }
		
		#endregion Constructors

        #region Overrides

        /// <summary>
        /// Returns page from the database of a specific size filtering on specific database that is passed
        /// into the request by the AdvancedPageModel in the request body
        /// </summary>
        /// <param name="id">Used to help create a different route and for the most part be ignored</param>
        /// <param name="model">The model containing the advanced paging options</param>
        /// <returns>A <see cref="PaginationResult{T}"/> that contains a list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpPost(), Route("AdvancedPage/{id}/")]
        public override IHttpActionResult AdvancedPage(int id, [FromBody] AdvancedPageModel model)
        {
            return base.AdvancedPage(id, model);
        }

        /// <summary>
        /// Will delete an entity from the database by the primary key value
        /// </summary>
        /// <param name="id">The value of the primary key of an Entity</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpDelete(), Route("{id}")]
        public override IHttpActionResult Delete(<%= entityinfo.PrimaryKey %> id)
        {
            return base.Delete(id);
        }

        /// <summary>
        /// Returns all of the entities from a given database view or table
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpGet(), Route("")]
        public override IHttpActionResult Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Returns a single entity by the primary key value
        /// </summary>
        /// <param name="id">The value of the primary key of <see cref="<%= entityinfo.PropertyName %>" /></param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpGet(), Route("{id}")]
        public override IHttpActionResult Get(<%= entityinfo.PrimaryKey %> id)
        {
            return base.Get(id);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">Used down stream if necessary, default to 0</param>
        /// <param name="model"><see cref="AutoComplete"/></param>
        /// <returns>A list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpPost(), Route("GetAutoComplete/{id}")]
        public override IHttpActionResult GetAutoComplete(int id, [FromBody] AutoComplete model)
        {
            return base.GetAutoComplete(id, model);
        }

        /// <summary>
        /// Returns page from the database of a specific size
        /// </summary>
        /// <param name="limit">The number of records to return</param>
        /// <param name="offset">The row number to offset the return limit</param>
        /// <param name="search">The search term to search the entity's searchable properties</param>
        /// <param name="sort">The property name to sort the return</param>
        /// <param name="sortOrder">The order to sort (descending/ascending) defaults to descending</param>
        /// <returns>A <see cref="PaginationResult{T}"/> that contains a list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpGet(), Route("GetPage")]
        public override IHttpActionResult GetPage(int limit, int offset, string search = null, string sort = null, string sortOrder = "desc")
        {
            return base.GetPage(limit, offset, search, sort, sortOrder);
        }

        /// <summary>
        /// Inserts an entity to the database that passed from the request body
        /// </summary>
        /// <param name="entity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The <see cref="<%= entityinfo.PropertyName %>"/> after it's inserted into the database</returns>
        [HttpPost(), Route("")]
        public override IHttpActionResult Post([FromBody] <%= entityinfo.PropertyName %> entity)
        {
            return base.Post(entity);
        }

        /// <summary>
        /// Updates an entity in the database by the primary key and the entity that
        /// passed into the request in the request body
        /// </summary>
        /// <param name="id">The primary key</param>
        /// <param name="entity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The <see cref="<%= entityinfo.PropertyName %>"/> after it's updated</returns>
        [HttpPut(), Route("{id}")]
        public override IHttpActionResult Put(int id, [FromBody] <%= entityinfo.PropertyName %> entity)
        {
            return base.Put(id, entity);
        }

        #endregion Overrides

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}