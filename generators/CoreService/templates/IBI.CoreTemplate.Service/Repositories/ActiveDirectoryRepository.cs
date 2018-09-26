using IBI.<%= Name %>.Service.Core.Attributes;
using IBI.<%= Name %>.Service.Core.Context;
using IBI.<%= Name %>.Service.Core.Models;
using IBI.<%= Name %>.Service.Core.Repositories;
using IBI.<%= Name %>.Service.Entities;
using IBI.<%= Name %>.Service.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Repositories
{
    /// <summary>
    /// Repository for the <see cref="ActiveDirectory"/> entity
    /// </summary>
    public partial class ActiveDirectoryRepository : BaseRepository<ActiveDirectory, int>, IActiveDirectoryRepository
    {
        #region Constructors

        /// <summary>
        /// Default constructor for the <see cref="ActiveDirectory"/> repository
        /// </summary>
        public ActiveDirectoryRepository(MainContext mainContext) : base(mainContext)
        {
            base.Entity = base.mainContext.Set<ActiveDirectory>();
        }

        #endregion Constructors

        #region Overrides

        #region Edit Methods

        /// <summary>
        /// Delete <see cref="ActiveDirectory"/> by the primary key
        /// unless <see cref="ActiveDirectory"/> is marked as read-only then
        /// nothing will be done
        /// </summary>
        /// <param name="id">The value of <see cref="ActiveDirectory"/> primary key</param>
        public override Task Delete(int id)
        {
            return base.Delete(id);
        }

        /// <summary>
        /// Inserts a new <see cref="ActiveDirectory"/> into the database
        /// </summary>
        /// <param name="newItem"><see cref="ActiveDirectory"/></param>
        /// <returns><see cref="ActiveDirectory"/> with primary key value set</returns>
        public override Task<ActiveDirectory> Insert(ActiveDirectory newItem)
        {
            return base.Insert(newItem);
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
        /// <param name="entity"><see cref="ActiveDirectory"/></param>
        /// <returns>The updated <see cref="ActiveDirectory"/></returns>
        public override Task<ActiveDirectory> Update(ActiveDirectory entity)
        {
            return base.Update(entity);
        }

        #endregion Edit Methods

        #region Read-Only Methods

        /// <summary>
        /// retrieves <see cref="ActiveDirectory"/> by the primary key
        /// </summary>
        /// <param name="id">The value of <see cref="ActiveDirectory"/> primary key</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        public override Task<ActiveDirectory> Get(int id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// retrieves every <see cref="ActiveDirectory"/> from the database
        /// </summary>
        /// <returns>A list of <see cref="ActiveDirectory"/></returns>
        public override Task<List<ActiveDirectory>> Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Gets a page of <see cref="ActiveDirectory"/> from the database
        /// </summary>
        /// <param name="offSet">Row offset for the page</param>
        /// <param name="limit">Number of rows to return</param>
        /// <param name="searchCriteria">The search string for the simple search</param>
        /// <param name="sortName">The property name of <see cref="ActiveDirectory"/> to sort</param>
        /// <param name="sortOrder">The sort direction (asc or desc)</param>
        /// <param name="model"><see cref="AdvancedPageModel"/></param>
        /// <param name="query">Extra query to further filter the page</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="ActiveDirectory"/></returns>
        public override Task<PaginationResult<ActiveDirectory>> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<ActiveDirectory> query = null)
        {
            return base.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, model, query);
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
            return base.GetAutocomplete(length, term);
        }

        /// <summary>
        /// Returns <see cref="ActiveDirectory"/> that is tracked by
        /// entity framework
        /// </summary>
        /// <param name="id">The value of <see cref="ActiveDirectory"/> primary key</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        public override Task<ActiveDirectory> GetForUpdate(int id)
        {
            return base.GetForUpdate(id);
        }

        /// <summary>
        /// Create a query that joins the sub-properties in one database call
        /// </summary>
        /// <returns><see cref="IQueryable"/> of <see cref="ActiveDirectory"/></returns>
        public override IQueryable<ActiveDirectory> GetFullEntity()
        {
            return base.GetFullEntity();
        }

        #endregion Read-Only Methods

        #endregion Overrides

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}