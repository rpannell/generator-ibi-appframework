using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Core.Services
{
    /// <summary>
    /// The base service that represents the base repository functions
    /// </summary>
    public partial class BaseService<TRepository, TEntity, TPrimaryKey> : IDisposable, IBaseService<TRepository, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TRepository : IBaseRepository<TEntity, TPrimaryKey>
    {
        #region Fields

        /// <summary>
        /// Represents the Repository of the Entity
        /// </summary>
        public TRepository Repository = default(TRepository);

        #endregion Fields

        #region Methods

        /// <summary>
        /// Delete an entity by the primary key
        /// </summary>
        /// <param name="id">The primary key</param>
        public virtual Task Delete(TPrimaryKey id)
        {
            return this.Repository.Delete(id);
        }

        /// <summary>
        /// If disposes with an open transaction roll it back
        /// </summary>
        public void Dispose()
        {
            this.Repository.RollBackTransaction();
        }

        /// <summary>
        /// Gets a list of all of the entities from a database view or table
        /// </summary>
        /// <returns>A List of Entity</returns>
        public virtual Task<List<TEntity>> Get()
        {
            return this.Repository.Get();
        }

        /// <summary>
        /// Gets a single row of the entity by the value of the primary key
        /// </summary>
        /// <param name="id">The value of the primary key</param>
        /// <returns>An Entity by the primary key</returns>
        public virtual Task<TEntity> Get(TPrimaryKey id)
        {
            return this.Repository.Get(id);
        }

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="model">The AdvancedPageModel</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        public virtual Task<PaginationResult<TEntity>> GetAdvancedPage(AdvancedPageModel model)
        {
            return this.Repository.GetAdvancedPage(model.SearchOffSet, model.SearchLimit, model.SearchString, model.SortString, model.SortOrder, model);
        }

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="offSet">The row offset on the database</param>
        /// <param name="limit">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search on the searchable attribute</param>
        /// <param name="sortName">The name of the property to sort on</param>
        /// <param name="sortOrder">The direction to sort (desc/asc)</param>
        /// <param name="model">The AdvancedPageModel</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        public Task<PaginationResult<TEntity>> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null)
        {
            return this.Repository.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, model);
        }

        /// <summary>
        /// Returns a Queryable of the Entity
        /// </summary>
        /// <returns>Queryable of Entity</returns>
        public IQueryable<TEntity> GetAll()
        {
            return Get().Result.AsQueryable();
        }

        /// <summary>
        /// Gets a set a records that are triggered by the AutoComplete search
        /// </summary>
        /// <param name="length">The number of records to return</param>
        /// <param name="term">The search term of the AutoComplete properties</param>
        /// <returns>List of the Entity</returns>
        public virtual Task<List<TEntity>> GetAutocomplete(int length, object term)
        {
            return this.Repository.GetAutocomplete(length, term);
        }

        /// <summary>
        /// Insert an entity in the database
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        /// <returns>The Entity after the database insert</returns>
        public virtual Task<TEntity> Insert(TEntity entity)
        {
            return this.Repository.Insert(entity);
        }

        /// <summary>
        /// Used to merge the values of one entity into another entity
        /// </summary>
        /// <param name="previousEntity">The current entity</param>
        /// <param name="newEntity">The updated entity containing the new values</param>
        public virtual void MergeValues(TEntity previousEntity, TEntity newEntity)
        {
            this.Repository.MergeValues(previousEntity, newEntity);
        }

        /// <summary>
        /// Updates the row in the database that represents the entity
        /// </summary>
        /// <param name="entity">The Entity to update</param>
        /// <returns>The Entity after the database update</returns>
        public virtual Task<TEntity> Update(TEntity entity)
        {
            return this.Repository.Update(entity);
        }

        #endregion Methods
    }
}