using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Services;
using IBI.<%= projectname %>.Service.Core.Models;
using System.Collections.Generic;
using System.Linq.Expressions;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom functions should be added
/// to this file and not the <%= entityinfo.PropertyName %>Service 
/// file in the base folder
/// 
/// This service performs the business logic for the entity <%= entityinfo.PropertyName %>
/// and should not perform any database operations.  Other services can be
/// injected into the constructor using the interface of 
/// the other service.  See documentation for more information.
/// The Repository of the entity can be found using the
/// "this.Repository" variable
/// </summary>
namespace IBI.<%= projectname %>.Service.Services
{
	public partial class <%= entityinfo.PropertyName %>Service : BaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Service
    {
        #region Constructors
		
		public <%= entityinfo.PropertyName %>Service(I<%= entityinfo.PropertyName %>Repository repository)
        {
            this.Repository = repository;
        }
		
		#endregion Constructors

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

        public override PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(AdvancedPageModel model)
        {
            return base.GetAdvancedPage(model);
        }

        public override List<<%= entityinfo.PropertyName %>> GetAutocomplete(int length, object term)
        {
            return base.GetAutocomplete(length, term);
        }

        public override PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int offSet, int limit, string searchCriteria = null, string sortName = null, string sortOrder = "desc", ParameterExpression genericType = null, Expression extraExpr = null)
        {
            return base.GetPage(offSet, limit, searchCriteria, sortName, sortOrder, genericType, extraExpr);
        }

        public override <%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> newEntity)
        {
            /*
                at this point update the entity with the properies that should be
                set only during the insert (think CreatedDateTime and CreatedUserId)
            */
            return base.Insert(newEntity);
        }

        public override void MergeValues(<%= entityinfo.PropertyName %> previousEntity, <%= entityinfo.PropertyName %> newEntity)
        {
            base.MergeValues(previousEntity, newEntity);
        }

        public override <%= entityinfo.PropertyName %> Update(<%= entityinfo.PropertyName %> updatedEntity)
        {
            /*
                usually the entire entity is not passed over to do the update (think CreatedDateTime)
                so may need to merge the data from the original entity to the new entity
            */
            //var original = this.Repository.GetForUpdate(updatedEntity.Id);
            // set the properties from the original that aren't populated on the entity
            //base.MergeValues(original, updatedEntity);
            //return base.Update(original);

            return base.Update(updatedEntity);
        }

        #endregion Overrides

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}