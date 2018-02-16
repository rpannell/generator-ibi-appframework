using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;
using IBI.<%= projectname %>.Service.Core.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom functions should be added
/// to this file 
/// 
/// This file contains any necessary database operations for
/// the <%= entityinfo.PropertyName %> entity.  No business
/// logic should be performed in this class and should left
/// in the service layer.  This file is for performing database
/// operations only.  The base operations can be found in the
/// BaseRepository file
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories
{
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Repository
    {
		#region Constructors
		
		public <%= entityinfo.PropertyName %>Repository() : base()
		{
			System.Data.Entity.Database.SetInitializer<<%= entityinfo.PropertyName %>Repository>(null); //do not remove
		}
		
		#endregion Constructors

		#region Custom Model Building Use with Caution

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		#endregion Custom Model Building Use with Caution

		#region Overrides

        public override void Delete(<%= entityinfo.PrimaryKey %> id)
        {
            base.Delete(id);
        }

        public override <%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> id)
        {
            return base.Get(id);
        }

        public override List<<%= entityinfo.PropertyName %>> Get()
        {
            return base.Get();
        }

        public override PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", AdvancedPageModel model = null, IQueryable<<%= entityinfo.PropertyName %>> query = null)
        {
            return base.GetAdvancedPage(offSet, limit, searchCriteria, sortName, sortOrder, model, query);
        }

        public override List<<%= entityinfo.PropertyName %>> GetAutocomplete(int length, object term)
        {
            return base.GetAutocomplete(length, term);
        }

        public override <%= entityinfo.PropertyName %> GetForUpdate(<%= entityinfo.PrimaryKey %> id)
        {
            return base.GetForUpdate(id);
        }

        public override IQueryable<<%= entityinfo.PropertyName %>> GetFullEntity()
        {
            return base.GetFullEntity();
        }

        public override PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null, IQueryable<<%= entityinfo.PropertyName %>> query = null)
        {
            return base.GetPage(offSet, limit, searchCriteria, sortName, sortOrder, genericType, extraExpr, query);
        }

        public override <%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> newItem)
        {
            return base.Insert(newItem);
        }

        public override void MergeValues(<%= entityinfo.PropertyName %> previousEntity, <%= entityinfo.PropertyName %> newEntity)
        {
            base.MergeValues(previousEntity, newEntity);
        }

        public override <%= entityinfo.PropertyName %> Update(<%= entityinfo.PropertyName %> entity)
        {
            return base.Update(entity);
        }

        #endregion Overrides

		/* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}