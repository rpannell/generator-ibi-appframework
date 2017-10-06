using IBI.<%= projectname %>.Service.Core.Controllers;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Entities;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	DO NOT UPDATE THIS FILE, ANY CHANGES WILL BE OVERWRITTEN 
/// BY THE ENTITY SCAFFOLDING ON THE NEXT RUN, ADD ANY NECESSARY
/// CODE CHANGES SHOULD BE ADDED TO THE EXTENDED CONTROLLER
/// </summary>
namespace IBI.<%= projectname %>.Service.Controllers
{
    public partial class <%= entityinfo.PropertyName %>Controller : BaseController<I<%= entityinfo.PropertyName %>Service, I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
		/* DO NOT UPDATE THIS FILE */
		
		#region Constructors
		
		public <%= entityinfo.PropertyName %>Controller(I<%= entityinfo.PropertyName %>Service service)
        {
            this.CurrentService = service;
        }
		
		#endregion Constructors
		
		/* DO NOT UPDATE THIS FILE */
    }
}