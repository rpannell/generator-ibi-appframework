using IBI.Plugin.Utilities.Logging.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Services;
using IBI.<%= projectname %>.Service.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

/*
 *  Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
 * 
 * 	Add any necessary custom functions should be added
 *  to this file and not the <%= entityinfo.PropertyName %>Service 
 *  file in the base folder
 *  
 *  This service performs the business logic for the entity <%= entityinfo.PropertyName %>
 *  and should not perform any database operations.  Other services can be
 *  injected into the constructor using the interface of 
 *  the other service.  See documentation for more information.
 *  The Repository of the entity can be found using the
 *  "this.Repository" variable
 */
namespace IBI.<%= projectname %>.Service.Services
{
    /// <summary>
    /// The service that handles <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
	public partial class <%= entityinfo.PropertyName %>Service : BaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Service
    {
        #region Fields

        /*
         * Add list of inject services as private readonly here
         */
        //private readonly IErpActiveDirectoryService erpActiveDirectoryService;
        private ILogAdapter Log = <%= projectname %>Logger<<%= entityinfo.PropertyName %>Service>.GetLogger();

        #endregion Fields

        #region Constructors
		
        /// <summary>
        /// Default constructor that injects <see cref="I<%= entityinfo.PropertyName %>Repository"/> for 
        /// base entity of the service
        /// </summary>
        /// <param name="repository">The repository of <see cref="<%= entityinfo.PropertyName %>"/></param>
		public <%= entityinfo.PropertyName %>Service(I<%= entityinfo.PropertyName %>Repository repository)
        {
            /*
             * Update the constructor to pass in services needed by injecting the interface of the service needed and
             * set the private variable to the service passed in
             *
             * public <%= entityinfo.PropertyName %>Service(I<%= entityinfo.PropertyName %>Repository repository, IErpActiveDirectoryService erpActiveDirectoryService)
             * {
             *     this.Repository = repository;
             *     this.erpActiveDirectoryService = erpActiveDirectoryService;
             * }
             */

            this.Repository = repository;
        }
		
        

		#endregion Constructors

        #region Overrides

        /// <summary>
        /// Delete <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// </summary>
        /// <param name="id">The primary key of <see cref="<%= entityinfo.PropertyName %>"/></param>
        public override void Delete(<%= entityinfo.PrimaryKey %> id)
        {
            /*
             * An example of an override is to set the entity as a soft delete
             * meaning the developer sets a flag instead of actually deleting the
             * entity from the database
             * 
             * var entity = base.Get(id);
             * var user = this.erpActiveDirectoryService.GetCurrentlyLoggedInUser();
             * entity.IsActive = false;
             * entity.DeletedUserId = user.ActiveDirectoryID;
             * entity.DeletedUTCDateTime = System.DateTime.UtcNow;
             * base.Update(entity);
             */
            base.Delete(id);
        }

        /// <summary>
        /// Get a <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// </summary>
        /// <param name="id">The value of the primary key</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override <%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> id)
        {
            var entity = base.Get(id);
            /*
             *  an override that is common is to check the user's
             *  permission if they have the right to get the entity
             */
            return entity;
        }


        /// <summary>
        /// Return every <see cref="<%= entityinfo.PropertyName %>"/> from the database table
        /// </summary>
        /// <returns>List of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override List<<%= entityinfo.PropertyName %>> Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Gets a page of <see cref="<%= entityinfo.PropertyName %>"/> from the database by
        /// the <see cref="AdvancedPageModel"/> settings
        /// </summary>
        /// <param name="model"><see cref="AdvancedPageModel"/> passed in from the controller</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(AdvancedPageModel model)
        {
            /*
             * An override that is common is to change the query based on a users role
             *
             * var query = this.Repository.GetFullEntity();
             * if (!Utils.UserInfo.IsUserInRole("Admin"))
             * {
             *     var user = this.erpActiveDirectoryService.GetCurrentlyLoggedInUser();
             *     query = query.Where(x => x.CreatedUserId == user.ActiveDirectoryID);
             * }
             * return this.Repository.GetAdvancedPage(model.SearchOffSet, model.SearchLimit, model.SearchString, model.SortString, model.SortOrder, model, query);
             */

            return base.GetAdvancedPage(model);
        }

        /// <summary>
        /// Does a search on <see cref="<%= entityinfo.PropertyName %>"/> by any property that
        /// has the <see cref="AutoComplete"/> attribute
        /// </summary>
        /// <param name="length">The number of results</param>
        /// <param name="term">search term</param>
        /// <returns>List of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override List<<%= entityinfo.PropertyName %>> GetAutocomplete(int length, object term)
        {
            return base.GetAutocomplete(length, term);
        }


        /// <summary>
        /// Simple GetPage of <see cref="<%= entityinfo.PropertyName %>"/>
        /// </summary>
        /// <param name="offSet">Row offset for the page</param>
        /// <param name="limit">Number of rows to return</param>
        /// <param name="searchCriteria">The search string for the simple search</param>
        /// <param name="sortName">The property name of <see cref="<%= entityinfo.PropertyName %>"/> to sort</param>
        /// <param name="sortOrder">The sort direction (asc or desc)</param>
        /// <param name="genericType">The type of <see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <param name="extraExpr">Extra linq expression</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [System.Obsolete("Use GetAdvancedPage")]
        public override PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null)
        {
            return base.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, null);
        }

        /// <summary>
        /// Inserts a new <see cref="<%= entityinfo.PropertyName %>"/> into the database
        /// </summary>
        /// <param name="newEntity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/> with primary key value set</returns>
        public override <%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> newEntity)
        {
            /*
             * at this point update the entity with the properies that should be
             * set only during the insert (think CreatedDateTime and CreatedUserId)
             * var user = this.erpActiveDirectoryService.GetCurrentlyLoggedInUser();
             * newEntity.CreatedUserId = user.ActiveDirectoryID;
             * newEntity.UpdatedUserId = user.ActiveDirectoryID;
             * newEntity.CreatedUTCDateTime = System.DateTime.UtcNow;
             * newEntity.UpdatedUTCDateTime = System.DateTime.UtcNow;
             */
            return base.Insert(newEntity);
        }

        /// <summary>
        /// Merges the properties in the tracked <see cref="<%= entityinfo.PropertyName %>"/> from a non-tracked <see cref="<%= entityinfo.PropertyName %>"/>
        /// </summary>
        /// <param name="previousEntity">The tracked <see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <param name="newEntity">The non-tracked <see cref="<%= entityinfo.PropertyName %>"/></param>
        public override void MergeValues(<%= entityinfo.PropertyName %> previousEntity, <%= entityinfo.PropertyName %> newEntity)
        {
            base.MergeValues(previousEntity, newEntity);
        }

        /// <summary>
        /// Update <see cref="<%= entityinfo.PropertyName %>"/> 
        /// </summary>
        /// <param name="updatedEntity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The updated <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override <%= entityinfo.PropertyName %> Update(<%= entityinfo.PropertyName %> updatedEntity)
        {
            /*
             * usually the entire entity is not passed over to do the update (think CreatedDateTime)
             * so may need to merge the data from the original entity to the new entity
             * 
             * var user = this.erpActiveDirectoryService.GetCurrentlyLoggedInUser();
             * var original = this.Repository.GetForUpdate(updatedEntity.ContractId);
             * updatedEntity.CreatedUserId = original.CreatedUserId;
             * updatedEntity.CreatedUTCDateTime = original.CreatedUTCDateTime;
             * updatedEntity.UpdatedUserId = user.ActiveDirectoryID;
             * updatedEntity.UpdatedUTCDateTime = System.DateTime.UtcNow;
             * base.MergeValues(original, updatedEntity);
             * return base.Update(original);
             */

            return base.Update(updatedEntity);
        }

        #endregion Overrides

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}