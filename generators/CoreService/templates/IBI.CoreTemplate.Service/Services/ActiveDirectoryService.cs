using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Services;
using IBI.<%= Name %>.Service.Entities;
using IBI.<%= Name %>.Service.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Services
{
    /// <summary>
    /// The service that handles <see cref="ActiveDirectory"/>
    /// </summary>
	public partial class ActiveDirectoryService : BaseService<IActiveDirectoryRepository, ActiveDirectory, int>, IActiveDirectoryService
    {
        #region Fields

        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor that injects <see cref="IActiveDirectoryRepository"/> for
        /// base entity of the service
        /// </summary>
        /// <param name="repository">The repository of <see cref="ActiveDirectory"/></param>
        /// <param name="logger">Logger for <see cref="ActiveDirectoryService"/></param>
        public ActiveDirectoryService(IActiveDirectoryRepository repository, ILogger<ActiveDirectoryService> logger)
        {
            this.Repository = repository;
            this.logger = logger;
        }

        #endregion Constructors

        #region Overrides

        #region Read-Only Methods

        /// <summary>
        /// Get a <see cref="ActiveDirectory"/> by the primary key
        /// </summary>
        /// <param name="id">The value of the primary key</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        public override Task<ActiveDirectory> Get(int id)
        {
            logger.LogDebug(string.Format("Entered ActiveDirectoryService Get({0})", id));
            var entity = base.Get(id);
            /*
             *  an override that is common is to check the user's
             *  permission if they have the right to get the entity
             */
            return entity;
        }

        /// <summary>
        /// </summary>
        /// <returns>List of <see cref="ActiveDirectory"/></returns>
        public override Task<List<ActiveDirectory>> Get()
        {
            logger.LogDebug("Entered ActiveDirectoryService Get");
            return base.Get();
        }

        /// <summary>
        /// Gets a page of <see cref="ActiveDirectory"/> from the database by
        /// the <see cref="AdvancedPageModel"/> settings
        /// </summary>
        /// <param name="model"><see cref="AdvancedPageModel"/> passed in from the controller</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="ActiveDirectory"/></returns>
        public override Task<PaginationResult<ActiveDirectory>> GetAdvancedPage(AdvancedPageModel model)
        {
            /*
             * An override that is common is to change the query based on a users role
             *
             * var query = this.Repository.GetFullEntity();
             * if (!Utils.UserInfo.IsUserInRole("Admin"))
             * {
             *     var user = this.ActiveDirectoryService.GetCurrentlyLoggedInUser();
             *     query = query.Where(x => x.CreatedUserId == user.ActiveDirectoryID);
             * }
             * return this.Repository.GetAdvancedPage(model.SearchOffSet, model.SearchLimit, model.SearchString, model.SortString, model.SortOrder, model, query);
             */
            logger.LogDebug("Entered ActiveDirectoryService GetAdvancedPage");
            return base.GetAdvancedPage(model);
        }

        /// <summary>
        /// Does a search on <see cref="ActiveDirectory"/> by any property that
        /// has the <see cref="AutoComplete"/> attribute
        /// </summary>
        /// <param name="length">The number of results</param>
        /// <param name="term">search term</param>
        /// <returns>List of <see cref="ActiveDirectory"/></returns>
        public override Task<List<ActiveDirectory>> GetAutocomplete(int length, object term)
        {
            logger.LogDebug(string.Format("Entered ActiveDirectoryService GetAutocomplete({0}, {1}", length, term));
            return base.GetAutocomplete(length, term);
        }

        #endregion Read-Only Methods

        #region Edit Methods

        /// <summary>
        /// Delete <see cref="ActiveDirectory"/> by the primary key
        /// </summary>
        /// <param name="id">The primary key of <see cref="ActiveDirectory"/></param>
        public async override Task Delete(int id)
        {
            /*
             * An example of an override is to set the entity as a soft delete
             * meaning the developer sets a flag instead of actually deleting the
             * entity from the database
             *
             * var entity = base.Get(id);
             * var user = this.ActiveDirectoryService.GetCurrentlyLoggedInUser();
             * entity.IsActive = false;
             * entity.DeletedUserId = user.ActiveDirectoryID;
             * entity.DeletedUTCDateTime = System.DateTime.UtcNow;
             * base.Update(entity);
             */

            try
            {
                logger.LogDebug("Entered ActiveDirectoryService Delete");
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
        /// Inserts a new <see cref="ActiveDirectory"/> into the database
        /// </summary>
        /// <param name="newEntity"><see cref="ActiveDirectory"/></param>
        /// <returns><see cref="ActiveDirectory"/> with primary key value set</returns>
        public async override Task<ActiveDirectory> Insert(ActiveDirectory newEntity)
        {
            /*
             * at this point update the entity with the properies that should be
             * set only during the insert (think CreatedDateTime and CreatedUserId)
             * var user = this.ActiveDirectoryService.GetCurrentlyLoggedInUser();
             * newEntity.CreatedUserId = user.ActiveDirectoryID;
             * newEntity.UpdatedUserId = user.ActiveDirectoryID;
             * newEntity.CreatedUTCDateTime = System.DateTime.UtcNow;
             * newEntity.UpdatedUTCDateTime = System.DateTime.UtcNow;
             */

            try
            {
                logger.LogDebug("Entered ActiveDirectoryService Insert");
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
        /// Merges the properties in the tracked <see cref="ActiveDirectory"/> from a non-tracked <see cref="ActiveDirectory"/>
        /// </summary>
        /// <param name="previousEntity">The tracked <see cref="ActiveDirectory"/></param>
        /// <param name="newEntity">The non-tracked <see cref="ActiveDirectory"/></param>
        public override void MergeValues(ActiveDirectory previousEntity, ActiveDirectory newEntity)
        {
            base.MergeValues(previousEntity, newEntity);
        }

        /// <summary>
        /// Update <see cref="ActiveDirectory"/>
        /// </summary>
        /// <param name="updatedEntity"><see cref="ActiveDirectory"/></param>
        /// <returns>The updated <see cref="ActiveDirectory"/></returns>
        public async override Task<ActiveDirectory> Update(ActiveDirectory updatedEntity)
        {
            /*
             * usually the entire entity is not passed over to do the update (think CreatedDateTime)
             * so may need to merge the data from the original entity to the new entity
             *
             * var user = this.ActiveDirectoryService.GetCurrentlyLoggedInUser();
             * var original = this.Repository.GetForUpdate(updatedEntity.ContractId);
             * updatedEntity.CreatedUserId = original.CreatedUserId;
             * updatedEntity.CreatedUTCDateTime = original.CreatedUTCDateTime;
             * updatedEntity.UpdatedUserId = user.ActiveDirectoryID;
             * updatedEntity.UpdatedUTCDateTime = System.DateTime.UtcNow;
             * base.MergeValues(original, updatedEntity);
             * return base.Update(original);
             */
            try
            {
                logger.LogDebug("Entered ActiveDirectoryService Update");
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

        #region Methods

        /// <summary>
        /// Get the active directory of the currently logged in user
        /// </summary>
        /// <returns><see cref="ActiveDirectory"/></returns>
        public ActiveDirectory GetCurrentUser()
        {
            return this.Repository.GetCurrentUser();
        }

        /// <summary>
        /// Get ActiveDirectory by the username
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        public ActiveDirectory GetUserByUserName(string userName)
        {
            return this.Repository.GetUserByUserName(userName);
        }

        #endregion Methods

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}