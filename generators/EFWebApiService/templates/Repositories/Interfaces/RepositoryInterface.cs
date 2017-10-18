using IBI.<%= projectname %>.Service.Core.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom function definitions should be added
/// to this file and not the I<%= entityinfo.PropertyName %>Repository 
/// file in the base folder
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories.Interfaces
{
    public partial interface I<%= entityinfo.PropertyName %>Repository : IBaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}