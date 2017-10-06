using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Controllers;
using System.Collections.Generic;
using System.Web.Http;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom entity controller actions 
/// should be added to this file and not the <%= entityinfo.PropertyName %>Controller
/// in the base folder
/// </summary>
namespace IBI.<%= projectname %>.Service.Controllers
{
	[RoutePrefix("api/<%= entityinfo.PropertyName %>")]
    public partial class <%= entityinfo.PropertyName %>Controller : BaseController<I<%= entityinfo.PropertyName %>Service, I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
    }
}