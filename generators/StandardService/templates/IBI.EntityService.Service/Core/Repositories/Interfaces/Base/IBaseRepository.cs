using IBI.<%= Name %>.Service.Core.Entities;
using IBI.<%= Name %>.Service.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service.Core.Repositories.Interfaces
{
    /// <summary>
    /// Interface that represents the base repository functions
    /// </summary>
    public partial interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : Entity<TPrimaryKey>
    {
        /// <summary>
        /// Gets the full entity also joining any sub-entities in 
        /// one call to the database
        /// </summary>
        /// <returns>IQueryable</returns>
        IQueryable<TEntity> GetFullEntity();

        /// <summary>
        /// Delete an entity by the primary key
        /// If it's ReadOnly, nothing is done
        /// </summary>
        /// <param name="Id">The primary key</param>
        void Delete(TPrimaryKey Id);

        /// <summary>
        /// Insert an entity in the database
        /// If it's ReadOnly nothing is done
        /// </summary>
        /// <param name="Entity">The entity to insert</param>
        /// <returns>The Entity after the database insert</returns>
        TEntity Insert(TEntity Entity);

        /// <summary>
        /// Updates the row in the database that represents the entity
        /// </summary>
        /// <param name="Entity">The Entity to update</param>
        /// <returns>The Entity after the database update</returns>
        TEntity Update(TEntity Entity);

        /// <summary>
        /// Used to merge the values of one entity into another entity
        /// If it's ReadOnly nothing is done
        /// And InsertOnly properties are ignored
        /// </summary>
        /// <param name="previousEntity">The current entity</param>
        /// <param name="newEntity">The updated entity containing the new values</param>
        void MergeValues(TEntity previousEntity, TEntity newEntity);

        /// <summary>
        /// Gets a single row of the entity by the value of the primary key
        /// </summary>
        /// <param name="Id">The value of the primary key</param>
        /// <returns>An Entity by the primary key</returns>
        TEntity Get(TPrimaryKey Id);

        /// <summary>
        /// Gets a list of all of the entities from a database view or table
        /// </summary>
        /// <returns>A List of Entity</returns>
        List<TEntity> Get();

        /// <summary>
        /// Will get the entity by the primary key and keep 
        /// it attached to the session to allow for update
        /// </summary>
        /// <param name="id">The primary key value of the entity</param>
        /// <returns>TEntity</returns>
        TEntity GetForUpdate(TPrimaryKey id);

        /// <summary>
        /// Gets a set a records that are triggered by the AutoComplete search
        /// </summary>
        /// <param name="length">The number of records to return</param>
        /// <param name="term">The search term of the AutoComplete properties</param>
        /// <returns>List of the Entity</returns>
        List<TEntity> GetAutocomplete(int length, object term);

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="offSet">The row offset on the database</param>
        /// <param name="limit">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search on the searchable attribute</param>
        /// <param name="sortName">The name of the property to sort on</param>
        /// <param name="sortOrder">The direction to sort (desc/asc)</param>
        /// <param name="genericType">The parameter expression that represents the entity table</param>
        /// <param name="extraExpr">Any extra Linq.Expression to filter down in the database</param>
        /// <param name="query">Extra query to filter down the entity</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        PaginationResult<TEntity> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null, IQueryable<TEntity> query = null);

        /// <summary>
        /// Returns a page of data of the entity
        /// </summary>
        /// <param name="offSet">The row offset on the database</param>
        /// <param name="limit">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search on the searchable attribute</param>
        /// <param name="sortName">The name of the property to sort on</param>
        /// <param name="sortOrder">The direction to sort (desc/asc)</param>
        /// <param name="model">The AdvancedPageModel</param>
        /// <param name="query">Extra query to filter down the entity</param>
        /// <returns>A PaginationResult that contains the entities</returns>
        PaginationResult<TEntity> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<TEntity> query = null);
    }
}