using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;
using IBI.<%= projectname %>.Service.Core.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

/*
 * Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
 * 
 * 	Add any necessary custom functions should be added
 *  to this file 
 *  
 *  This file contains any necessary database operations for
 *  the <%= entityinfo.PropertyName %> entity.  No business
 *  logic should be performed in this class and should left
 *  in the service layer.  This file is for performing database
 *  operations only.  The base operations can be found in the
 *  BaseRepository file
 */
namespace IBI.<%= projectname %>.Service.Repositories
{
    /// <summary>
    /// Repository for the <see cref="<%= entityinfo.PropertyName %>"/> entity
    /// </summary>
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Repository
    {
		#region Constructors
		
        /// <summary>
        /// Default constructor for the <see cref="<%= entityinfo.PropertyName %>"/> repository (or <see cref="DbContext"/>)
        /// </summary>
		public <%= entityinfo.PropertyName %>Repository() : base()
		{
			System.Data.Entity.Database.SetInitializer<<%= entityinfo.PropertyName %>Repository>(null); //do not remove
		}
		
		#endregion Constructors

		#region Custom Model Building Use with Caution

        /// <summary>
        /// Handle advanced modeling
        /// </summary>
        /// <param name="modelBuilder"></param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		#endregion Custom Model Building Use with Caution

		#region Overrides

        /// <summary>
        /// Delete <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// unless <see cref="<%= entityinfo.PropertyName %>"/> is marked as read-only then
        /// nothing will be done
        /// </summary>
        /// <param name="id">The value of <see cref="<%= entityinfo.PropertyName %>"/> primary key</param>
        public override void Delete(<%= entityinfo.PrimaryKey %> id)
        {
            base.Delete(id);
        }

        /// <summary>
        /// retrieves <see cref="<%= entityinfo.PropertyName %>"/> by the primary key
        /// </summary>
        /// <param name="id">The value of <see cref="<%= entityinfo.PropertyName %>"/> primary key</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override <%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> id)
        {
            return base.Get(id);
        }

        /// <summary>
        /// retrieves every <see cref="<%= entityinfo.PropertyName %>"/> from the database
        /// </summary>
        /// <returns>A list of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override List<<%= entityinfo.PropertyName %>> Get()
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
        public override PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<<%= entityinfo.PropertyName %>> query = null)
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
        public override List<<%= entityinfo.PropertyName %>> GetAutocomplete(int length, object term)
        {
            return base.GetAutocomplete(length, term);
        }

        /// <summary>
        /// Returns <see cref="<%= entityinfo.PropertyName %>"/> that is tracked by
        /// entity framework
        /// </summary>
        /// <param name="id">The value of <see cref="<%= entityinfo.PropertyName %>"/> primary key</param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override <%= entityinfo.PropertyName %> GetForUpdate(<%= entityinfo.PrimaryKey %> id)
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
        /// <param name="query">Extra query to further filter the page</param>
        /// <returns><see cref="PaginationResult{T}"/> of <see cref="<%= entityinfo.PropertyName %>"/></returns>
        [System.Obsolete("Use GetAdvancedPage")]
        public override PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null, IQueryable<<%= entityinfo.PropertyName %>> query = null)
        {
            return base.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, null, query);
        }

        /// <summary>
        /// Inserts a new <see cref="<%= entityinfo.PropertyName %>"/> into the database
        /// </summary>
        /// <param name="newItem"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns><see cref="<%= entityinfo.PropertyName %>"/> with primary key value set</returns>
        public override <%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> newItem)
        {
            return base.Insert(newItem);
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
        /// <param name="entity"><see cref="<%= entityinfo.PropertyName %>"/></param>
        /// <returns>The updated <see cref="<%= entityinfo.PropertyName %>"/></returns>
        public override <%= entityinfo.PropertyName %> Update(<%= entityinfo.PropertyName %> entity)
        {
            return base.Update(entity);
        }

        #endregion Overrides

		/* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}