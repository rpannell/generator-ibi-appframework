using IBI.<%= projectname %>.Service.Core.Models;
using IBI.<%= projectname %>.Service.Core.Services;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBI.<%= projectname %>.Service.Services
{
    /// <summary>
    /// The service that handles <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
	public partial class <%= entityinfo.PropertyName %>Service : BaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Service
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor that injects <see cref="I<%= entityinfo.PropertyName %>Repository"/> for
        /// base entity of the service
        /// </summary>
        /// <param name="repository">The repository of <see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <param name="logger">Logger for <see cref="<%= entityinfo.PropertyName %>Service"/></param>
        public <%= entityinfo.PropertyName %>Service(I<%= entityinfo.PropertyName %>Repository repository, ILogger<<%= entityinfo.PropertyName %>Service> logger)
        {
            this.Repository = repository;
            this.logger = logger;
        }

        #endregion Constructors

        #region Overrides

        #region Read-Only Methods

        /// <summary>
        /// Get a <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// </summary>
        /// <param name="id">The value of the primary key</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<<%= entityinfo.PropertyName %>> Get(<%= entityinfo.PrimaryKey %> id)
        {
            logger.LogDebug(string.Format("Entered <%= entityinfo.PropertyName %>Service Get({0})", id));
            var entity = base.Get(id);
            /*
             *  an override that is common is to check the user's
             *  permission if they have the right to get the entity
             */
            return entity;
        }

        /// <summary>
        /// </summary>
        /// <returns>List of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<List<<%= entityinfo.PropertyName %>>> Get()
        {
            logger.LogDebug("Entered <%= entityinfo.PropertyName %>Service Get");
            return base.Get();
        }

        /// <summary>
        /// Gets a page of <see cref="<%= entityinfo.PropertyName %>"/> from the database by
        /// the <see cref="AdvancedPageModel"/> settings
        /// </summary>
        /// <param name="model"><see cref="AdvancedPageModel"/> passed in from the controller</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<PaginationResult<<%= entityinfo.PropertyName %>>> GetAdvancedPage(AdvancedPageModel model)
        {
            /*
             * An override that is common is to change the query based on a users role
             *
             * var query = this.Repository.GetFullEntity();
             * if (!Utils.UserInfo.IsUserInRole("Admin"))
             * {
             *     var user = this.<%= entityinfo.PropertyName %>Service.GetCurrentlyLoggedInUser();
             *     query = query.Where(x => x.CreatedUserId == user.<%= entityinfo.PropertyName %>ID);
             * }
             * return this.Repository.GetAdvancedPage(model.SearchOffSet, model.SearchLimit, model.SearchString, model.SortString, model.SortOrder, model, query);
             */
            logger.LogDebug("Entered <%= entityinfo.PropertyName %>Service GetAdvancedPage");
            return base.GetAdvancedPage(model);
        }

        /// <summary>
        /// Does a search on <see cref="<%= entityinfo.PropertyName %>"/> by any property that
        /// has the <see cref="AutoComplete"/> attribute
        /// </summary>
        /// <param name="length">The number of results</param>
        /// <param name="term">search term</param>
        /// <returns>List of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<List<<%= entityinfo.PropertyName %>>> GetAutocomplete(int length, object term)
        {
            logger.LogDebug(string.Format("Entered <%= entityinfo.PropertyName %>Service GetAutocomplete({0}, {1}", length, term));
            return base.GetAutocomplete(length, term);
        }

        #endregion Read-Only Methods

        #region Edit Methods

        /// <summary>
        /// Delete <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// </summary>
        /// <param name="id">The primary key of <see cref="<%= entityinfo.PropertyName %>"/></param>
        public async override Task Delete(<%= entityinfo.PrimaryKey %> id)
        {
            /*
             * An example of an override is to set the entity as a soft delete
             * meaning the developer sets a flag instead of actually deleting the
             * entity from the database
             *
             * var entity = base.Get(id);
             * var user = this.<%= entityinfo.PropertyName %>Service.GetCurrentlyLoggedInUser();
             * entity.IsActive = false;
             * entity.DeletedUserId = user.<%= entityinfo.PropertyName %>ID;
             * entity.DeletedUTCDateTime = System.DateTime.UtcNow;
             * base.Update(entity);
             */

            try
            {
                logger.LogDebug("Entered <%= entityinfo.PropertyName %>Service Delete");
                this.Repository.StartTransaction();
                await base.Delete(id);
                this.Repository.CommitTransaction();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception in Delete");
                this.Repository.RollBackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// Inserts a new <see cref="<%= entityinfo.PropertyName %>"/> into the database
        /// </summary>
        /// <param name="newEntity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/> with primary key value set</returns>
        public async override Task<<%= entityinfo.PropertyName %>> Insert(<%= entityinfo.PropertyName %> newEntity)
        {
            /*
             * at this point update the entity with the properies that should be
             * set only during the insert (think CreatedDateTime and CreatedUserId)
             * var user = this.<%= entityinfo.PropertyName %>Service.GetCurrentlyLoggedInUser();
             * newEntity.CreatedUserId = user.<%= entityinfo.PropertyName %>ID;
             * newEntity.UpdatedUserId = user.<%= entityinfo.PropertyName %>ID;
             * newEntity.CreatedUTCDateTime = System.DateTime.UtcNow;
             * newEntity.UpdatedUTCDateTime = System.DateTime.UtcNow;
             */

            try
            {
                logger.LogDebug("Entered <%= entityinfo.PropertyName %>Service Insert");
                this.Repository.StartTransaction();
                var rtn = await base.Insert(newEntity);
                this.Repository.CommitTransaction();
                return rtn;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Insert");
                this.Repository.RollBackTransaction();
                throw ex;
            }
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
        public async override Task<<%= entityinfo.PropertyName %>> Update(<%= entityinfo.PropertyName %> updatedEntity)
        {
            /*
             * usually the entire entity is not passed over to do the update (think CreatedDateTime)
             * so may need to merge the data from the original entity to the new entity
             *
             * var user = this.<%= entityinfo.PropertyName %>Service.GetCurrentlyLoggedInUser();
             * var original = this.Repository.GetForUpdate(updatedEntity.ContractId);
             * updatedEntity.CreatedUserId = original.CreatedUserId;
             * updatedEntity.CreatedUTCDateTime = original.CreatedUTCDateTime;
             * updatedEntity.UpdatedUserId = user.<%= entityinfo.PropertyName %>ID;
             * updatedEntity.UpdatedUTCDateTime = System.DateTime.UtcNow;
             * base.MergeValues(original, updatedEntity);
             * return base.Update(original);
             */
            try
            {
                logger.LogDebug("Entered <%= entityinfo.PropertyName %>Service Update");
                this.Repository.StartTransaction();
                var rtn = await base.Update(updatedEntity);
                this.Repository.CommitTransaction();
                return rtn;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in Insert");
                this.Repository.RollBackTransaction();
                throw ex;
            }
        }

        #endregion Edit Methods

        #endregion Overrides

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}