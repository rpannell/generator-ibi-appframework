using IBI.<%= projectname %>.Service.Core.Attributes;
using IBI.<%= projectname %>.Service.Core.Context;
using IBI.<%= projectname %>.Service.Core.Models;
using IBI.<%= projectname %>.Service.Core.Repositories;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IBI.<%= projectname %>.Service.Repositories
{
    /// <summary>
    /// Repository for the <see cref="<%= entityinfo.PropertyName %>"/> entity
    /// </summary>
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, int>, I<%= entityinfo.PropertyName %>Repository
    {
        #region Constructors

        /// <summary>
        /// Default constructor for the <see cref="<%= entityinfo.PropertyName %>"/> repository
        /// </summary>
        public <%= entityinfo.PropertyName %>Repository(MainContext mainContext) : base(mainContext)
        {
            base.Entity = base.mainContext.Set<<%= entityinfo.PropertyName %>>();
        }

        #endregion Constructors

        #region Overrides

        #region Edit Methods

        /// <summary>
        /// Delete <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// unless <see cref="<%= entityinfo.PropertyName %>"/> is marked as read-only then
        /// nothing will be done
        /// </summary>
        /// <param name="id">The value of <see cref="<%= entityinfo.PropertyName %>"/> primary key</param>
        public override Task Delete(<%= entityinfo.PrimaryKey %> id)
        {
            <% if(!entityinfo.IsReadOnly) { %> return base.Delete(id); <% } else { %> throw new System.NotImplementedException(); <% } %>
        }

        /// <summary>
        /// Inserts a new <see cref="<%= entityinfo.PropertyName %>"/> into the database
        /// </summary>
        /// <param name="newItem"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/> with primary key value set</returns>
        public override Task<<%= entityinfo.PropertyName %>> Insert(<%= entityinfo.PropertyName %> newItem)
        {
            <% if(!entityinfo.IsReadOnly) { %> return base.Insert(newItem); <% } else { %> throw new System.NotImplementedException(); <% } %>
        }

        /// <summary>
        /// Merges the properties in the tracked <see cref="<%= entityinfo.PropertyName %>"/> from a non-tracked <see cref="<%= entityinfo.PropertyName %>"/>
        /// </summary>
        /// <param name="previousEntity">The tracked <see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <param name="newEntity">The non-tracked <see cref="<%= entityinfo.PropertyName %>"/></param>
        public override void MergeValues(<%= entityinfo.PropertyName %> previousEntity, <%= entityinfo.PropertyName %> newEntity)
        {
            <% if(!entityinfo.IsReadOnly) { %> base.MergeValues(previousEntity, newEntity); <% } else { %> throw new System.NotImplementedException(); <% } %>
        }

        /// <summary>
        /// Update <see cref="<%= entityinfo.PropertyName %>"/>
        /// </summary>
        /// <param name="entity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The updated <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<<%= entityinfo.PropertyName %>> Update(<%= entityinfo.PropertyName %> entity)
        {
            <% if(!entityinfo.IsReadOnly) { %> return base.Update(entity); <% } else { %> throw new System.NotImplementedException(); <% } %>
        }

        #endregion Edit Methods

        #region Read-Only Methods

        /// <summary>
        /// retrieves <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// </summary>
        /// <param name="id">The value of <see cref="<%= entityinfo.PropertyName %>"/> primary key</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<<%= entityinfo.PropertyName %>> Get(<%= entityinfo.PrimaryKey %> id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// retrieves every <see cref="<%= entityinfo.PropertyName %>"/> from the database
        /// </summary>
        /// <returns>A list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<List<<%= entityinfo.PropertyName %>>> Get()
        {
            return base.Get();
        }

        /// <summary>
        /// Gets a page of <see cref="<%= entityinfo.PropertyName %>"/> from the database
        /// </summary>
        /// <param name="offSet">Row offset for the page</param>
        /// <param name="limit">Number of rows to return</param>
        /// <param name="searchCriteria">The search string for the simple search</param>
        /// <param name="sortName">The property name of <see cref="<%= entityinfo.PropertyName %>"/> to sort</param>
        /// <param name="sortOrder">The sort direction (asc or desc)</param>
        /// <param name="model"><see cref="AdvancedPageModel"/></param>
        /// <param name="query">Extra query to further filter the page</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<PaginationResult<<%= entityinfo.PropertyName %>>> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<<%= entityinfo.PropertyName %>> query = null)
        {
            return base.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, model, query);
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
            return base.GetAutocomplete(length, term);
        }

        /// <summary>
        /// Returns <see cref="<%= entityinfo.PropertyName %>"/> that is tracked by
        /// entity framework
        /// </summary>
        /// <param name="id">The value of <see cref="<%= entityinfo.PropertyName %>"/> primary key</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override Task<<%= entityinfo.PropertyName %>> GetForUpdate(int id)
        {
            return base.GetForUpdate(id);
        }

        /// <summary>
        /// Create a query that joins the sub-properties in one database call
        /// </summary>
        /// <returns><see cref="IQueryable"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override IQueryable<<%= entityinfo.PropertyName %>> GetFullEntity()
        {
            return base.GetFullEntity();
        }

        #endregion Read-Only Methods

        #endregion Overrides

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}