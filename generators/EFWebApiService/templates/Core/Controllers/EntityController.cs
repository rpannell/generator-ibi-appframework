using IBI.<%= projectname %>.Service.Core.Models;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBI.<%= projectname %>.Service.Controllers
{
    /// <summary>
    /// Controller for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
	[Route("api/<%= entityinfo.PropertyName %>"), Authorize()]
    public partial class <%= entityinfo.PropertyName %>Controller : Controller
    {
        #region Fields

        private readonly I<%= entityinfo.PropertyName %>Service CurrentService;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Normal constructor for the <see cref="<%= entityinfo.PropertyName %>Controller"/> that sets up the service
        /// </summary>
        /// <param name="service"><see cref="I<%= entityinfo.PropertyName %>Service"/></param>
        /// <param name="logger">Logger for <see cref="<%= entityinfo.PropertyName %>Controller"/></param>
        public <%= entityinfo.PropertyName %>Controller(I<%= entityinfo.PropertyName %>Service service, ILogger<<%= entityinfo.PropertyName %>Controller> logger)
        {
            this.CurrentService = service;
            this.logger = logger;
        }

        #endregion Constructors

        #region Default Methods

        #region GetMethods

        /// <summary>
        /// Returns page from the database of a specific size filtering on specific database that is passed
        /// into the request by the AdvancedPageModel in the request body
        /// </summary>
        /// <param name="model">The model containing the advanced paging options</param>
        /// <returns>A <see cref="PaginationResult{T}"/> that contains a list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpPost(), Route("AdvancedPage/")]
        public async Task<PaginationResult<<%= entityinfo.PropertyName %>>> AdvancedPage([FromBody] AdvancedPageModel model)
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller AdvancedPage Action");
            try
            {
                var pageResult = await this.CurrentService.GetAdvancedPage(model);
                return pageResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        /// <summary>
        /// Returns all of the entities from a given database view or table
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpGet(), Route("")]
        public async Task<List<<%= entityinfo.PropertyName %>>> Get()
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller Get Action");
            try
            {
                var allentities = await this.CurrentService.Get();
                return allentities;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        /// <summary>
        /// Returns a single entity by the primary key value
        /// </summary>
        /// <param name="id">The value of the primary key of <see cref="<%= entityinfo.PropertyName %>" /></param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpGet(), Route("{id}")]
        public async Task<<%= entityinfo.PropertyName %>> Get(<%= entityinfo.PrimaryKey %> id)
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller Get Action");
            try
            {
                var entity = await this.CurrentService.Get(id);
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        /// <summary>
        /// Performs a search on <see cref="<%= entityinfo.PropertyName %>"/> searching the autocomplete attribute
        /// </summary>
        /// <param name="model"><see cref="AutoComplete"/></param>
        /// <returns>A list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpPost(), Route("GetAutoComplete/")]
        public async Task<List<<%= entityinfo.PropertyName %>>> GetAutoComplete([FromBody] AutoComplete model)
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller GetAutoComplete Action");
            try
            {
                var results = await this.CurrentService.GetAutocomplete(model.Length, model.SearchTerm);
                return results;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
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
        public async Task<PaginationResult<<%= entityinfo.PropertyName %>>> GetPage(int limit, int offset, string search = null, string sort = null, string sortOrder = "desc")
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller GetPage Action");
            try
            {
                var pageResult = await this.CurrentService.GetAdvancedPage(new AdvancedPageModel()
                {
                    SearchLimit = limit,
                    SearchOffSet = offset,
                    SearchString = search,
                    SortOrder = sortOrder,
                    SortString = sort
                });
                return pageResult;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        #endregion GetMethods
<% if(!entityinfo.IsReadOnly) { %>
        #region Edit Methods

        /// <summary>
        /// Will delete an entity from the database by the primary key value
        /// </summary>
        /// <param name="id">The value of the primary key of an Entity</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        [HttpDelete(), Route("{id}")]
        public async Task<<%= entityinfo.PropertyName %>> Delete(<%= entityinfo.PrimaryKey %> id)
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller Delete Action");
            try
            {
                var entity = await this.CurrentService.Get(id);
                await this.CurrentService.Delete(id);
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        /// <summary>
        /// Inserts an entity to the database that passed from the request body
        /// </summary>
        /// <param name="entity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The <see cref="<%= entityinfo.PropertyName %>"/> after it's inserted into the database</returns>
        [HttpPost(), Route("")]
        public async Task<<%= entityinfo.PropertyName %>> Post([FromBody] <%= entityinfo.PropertyName %> entity)
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller Post Action");
            try
            {
                await this.CurrentService.Insert(entity);
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        /// <summary>
        /// Updates an entity in the database by the primary key and the entity that
        /// passed into the request in the request body
        /// </summary>
        /// <param name="id">The primary key</param>
        /// <param name="entity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The <see cref="<%= entityinfo.PropertyName %>"/> after it's updated</returns>
        [HttpPut(), Route("{id}")]
        public async Task<<%= entityinfo.PropertyName %>> Put(<%= entityinfo.PrimaryKey %> id, [FromBody] <%= entityinfo.PropertyName %> entity)
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %> Controller Put Action");
            try
            {
                await this.CurrentService.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, null, null);
                throw ex;
            }
        }

        #endregion Edit Methods
<% } %>
        #endregion Default Methods

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}