using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Controllers;
using System.Collections.Generic;
using System.Web.Http;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom controller actions should be added to 
/// this file
/// 
/// This controller should contain NO business logic and should be very small
/// and mainly pass the information sent to the action down to the service layer
/// The BaseController contains Get/Put/Post/Delete/GetPage/GetAutocomplete.
/// The service for this entity can be found using the "this.CurrentService" variable
/// </summary>
namespace IBI.<%= projectname %>.Service.Controllers
{
    /// <summary>
    /// Controller for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
	[RoutePrefix("api/<%= entityinfo.PropertyName %>")]
    public partial class <%= entityinfo.PropertyName %>Controller : BaseController<I<%= entityinfo.PropertyName %>Service, I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        #region Constructors
		
        /// <summary>
        /// Normal constructor for the <see cref="<%= entityinfo.PropertyName %>Controller"/> that sets up the service
        /// </summary>
        /// <param name="service"><see cref="I<%= entityinfo.PropertyName %>Service"/></param>
		public <%= entityinfo.PropertyName %>Controller(I<%= entityinfo.PropertyName %>Service service)
        {
            this.CurrentService = service;
        }
		
		#endregion Constructors

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}