using IBI.<%= projectname %>.Service.Core.Services.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using System.Collections.Generic;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom function definitions should be added
/// to this file 
/// 
/// To expose a function to the Controller layer for the 
/// <%= entityinfo.PropertyName %> entity or another service 
/// please define the function or property here
/// </summary>
namespace IBI.<%= projectname %>.Service.Services.Interfaces
{
    public partial interface I<%= entityinfo.PropertyName %>Service : IBaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}