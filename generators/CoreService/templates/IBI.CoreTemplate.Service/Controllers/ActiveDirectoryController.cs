using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Entities;
using IBI.<%= Name %>.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Controllers
{
    /// <summary>
    /// Controller for <see cref="ActiveDirectory"/>
    /// </summary>
	[Route("api/ActiveDirectory"), Authorize(Policy = "Authenticated")]
    public partial class ActiveDirectoryController : Controller
    {
        #region Fields

        private readonly IActiveDirectoryService CurrentService;
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Normal constructor for the <see cref="ActiveDirectoryController"/> that sets up the service
        /// </summary>
        /// <param name="service"><see cref="IActiveDirectoryService"/></param>
        /// <param name="logger">Logger for <see cref="ActiveDirectoryController"/></param>
        public ActiveDirectoryController(IActiveDirectoryService service, ILogger<ActiveDirectoryController> logger)
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
        /// <returns>A <see cref="PaginationResult{T}"/> that contains a list of <see cref="ActiveDirectory"/></returns>
        [HttpPost(), Route("AdvancedPage/")]
        public async Task<PaginationResult<ActiveDirectory>> AdvancedPage([FromBody] AdvancedPageModel model)
        {
            logger.LogDebug("Entered ActiveDirectory Controller AdvancedPage Action");
            try
            {
                var pageResult = await this.CurrentService.GetAdvancedPage(model);
                return pageResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns all of the entities from a given database view or table
        /// </summary>
        /// <returns><see cref="List{T}"/> of <see cref="ActiveDirectory"/></returns>
        [HttpGet(), Route("")]
        public async Task<List<ActiveDirectory>> Get()
        {
            logger.LogDebug("Entered ActiveDirectory Controller Get Action");
            try
            {
                var allentities = await this.CurrentService.Get();
                return allentities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Returns a single entity by the primary key value
        /// </summary>
        /// <param name="id">The value of the primary key of <see cref="ActiveDirectory" /></param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        [HttpGet(), Route("{id}")]
        public async Task<ActiveDirectory> Get(int id)
        {
            logger.LogDebug("Entered ActiveDirectory Controller Get Action");
            try
            {
                var entity = await this.CurrentService.Get(id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Performs a search on <see cref="ActiveDirectory"/> searching the autocomplete attribute
        /// </summary>
        /// <param name="model"><see cref="AutoComplete"/></param>
        /// <returns>A list of <see cref="ActiveDirectory"/></returns>
        [HttpPost(), Route("GetAutoComplete/")]
        public async Task<List<ActiveDirectory>> GetAutoComplete([FromBody] AutoComplete model)
        {
            logger.LogDebug("Entered ActiveDirectory Controller GetAutoComplete Action");
            try
            {
                var results = await this.CurrentService.GetAutocomplete(model.Length, model.SearchTerm);
                return results;
            }
            catch (Exception ex)
            {
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
        /// <returns>A <see cref="PaginationResult{T}"/> that contains a list of <see cref="ActiveDirectory"/></returns>
        [HttpGet(), Route("GetPage")]
        public async Task<PaginationResult<ActiveDirectory>> GetPage(int limit, int offset, string search = null, string sort = null, string sortOrder = "desc")
        {
            logger.LogDebug("Entered ActiveDirectory Controller GetPage Action");
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
                throw ex;
            }
        }

        #endregion GetMethods

        #region Edit Methods

        /// <summary>
        /// Will delete an entity from the database by the primary key value
        /// </summary>
        /// <param name="id">The value of the primary key of an Entity</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        [HttpDelete(), Route("{id}")]
        public async Task<ActiveDirectory> Delete(int id)
        {
            logger.LogDebug("Entered ActiveDirectory Controller Delete Action");
            try
            {
                var entity = await this.CurrentService.Get(id);
                await this.CurrentService.Delete(id);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Inserts an entity to the database that passed from the request body
        /// </summary>
        /// <param name="entity"><see cref="ActiveDirectory"/></param>
        /// <returns>The <see cref="ActiveDirectory"/> after it's inserted into the database</returns>
        [HttpPost(), Route("")]
        public async Task<ActiveDirectory> Post([FromBody] ActiveDirectory entity)
        {
            logger.LogDebug("Entered ActiveDirectory Controller Post Action");
            try
            {
                await this.CurrentService.Insert(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Updates an entity in the database by the primary key and the entity that
        /// passed into the request in the request body
        /// </summary>
        /// <param name="id">The primary key</param>
        /// <param name="entity"><see cref="ActiveDirectory"/></param>
        /// <returns>The <see cref="ActiveDirectory"/> after it's updated</returns>
        [HttpPut(), Route("{id}")]
        public async Task<ActiveDirectory> Put(int id, [FromBody] ActiveDirectory entity)
        {
            logger.LogDebug("Entered ActiveDirectory Controller Put Action");
            try
            {
                await this.CurrentService.Update(entity);
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Edit Methods

        #endregion Default Methods

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}