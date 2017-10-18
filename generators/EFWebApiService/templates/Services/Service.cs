using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Services;
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom functions should be added
/// to this file and not the <%= entityinfo.PropertyName %>Service 
/// file in the base folder
/// </summary>
namespace IBI.<%= projectname %>.Service.Services
{
	public partial class <%= entityinfo.PropertyName %>Service : BaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}