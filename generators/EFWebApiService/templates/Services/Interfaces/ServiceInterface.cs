using IBI.<%= projectname %>.Service.Core.Services.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom function definitions should be added
/// to this file and not the I<%= entityinfo.PropertyName %>Service 
/// file in the base folder
/// </summary>
namespace IBI.<%= projectname %>.Service.Services.Interfaces
{
    public partial interface I<%= entityinfo.PropertyName %>Service : IBaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
    }
}