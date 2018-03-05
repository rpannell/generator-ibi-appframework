using IBI.<%= projectname %>.Service.Core.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using System.Collections.Generic;

/*
 *  Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
 * 
 * 	Add any necessary custom function definitions should be added
 *  to this file
 *  
 *  To expose a function to the service layer for the 
 *  <%= entityinfo.PropertyName %> entity or repository
 *  please define the function or property here
 */
namespace IBI.<%= projectname %>.Service.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
    public partial interface I<%= entityinfo.PropertyName %>Repository : IBaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}