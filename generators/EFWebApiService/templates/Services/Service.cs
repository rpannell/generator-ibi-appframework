using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Services;
using System.Collections.Generic;

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

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}