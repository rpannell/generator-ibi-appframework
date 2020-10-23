using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Core.Services.Interfaces
{
    /// <summary>
    /// Generic Base Service
    /// </summary>
    public interface IBaseService { }

    /// <summary>
    /// An interface that defines the BaseService
    /// </summary>
    public partial interface IBaseService<TRepository, TEntity, TPrimaryKey> : IBaseService
        where TRepository : IBaseRepository<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
    {
        #region Methods

        /// <summary>
        /// Delete an entity by the primary key
        /// </summary>
        /// <param name="Id">The primary key</param>
        Task Delete(TPrimaryKey Id);

        /// <summary>
        /// Gets a single row of the entity by the value of the primary key
        /// </summary>
        /// <param name="Id">The value of the primary key</param>
        /// <returns>An Entity by the primary key</returns>
        Task<TEntity> Get(TPrimaryKey Id);

        /// <summary>
        /// Gets a list of all of the entities from a database view or table
        /// </summary>
        /// <returns>A List of Entity</returns>
        Task<List<TEntity>> Get();

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
        Task<PaginationResult<TEntity>> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null);

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="model">The AdvancedPageModel</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        Task<PaginationResult<TEntity>> GetAdvancedPage(AdvancedPageModel model);

        /// <summary>
        /// Returns a Queryable of the Entity
        /// </summary>
        /// <returns>Queryable of Entity</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Gets a set a records that are triggered by the AutoComplete search
        /// </summary>
        /// <param name="length">The number of records to return</param>
        /// <param name="term">The search term of the AutoComplete properties</param>
        /// <returns>List of the Entity</returns>
        Task<List<TEntity>> GetAutocomplete(int length, object term);

        /// <summary>
        /// Insert an entity in the database
        /// </summary>
        /// <param name="Entity">The entity to insert</param>
        /// <returns>The Entity after the database insert</returns>
        Task<TEntity> Insert(TEntity Entity);

        /// <summary>
        /// Updates the row in the database that represents the entity
        /// </summary>
        /// <param name="Entity">The Entity to update</param>
        /// <returns>The Entity after the database update</returns>
        Task<TEntity> Update(TEntity Entity);

        #endregion Methods
    }
}