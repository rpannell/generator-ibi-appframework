using IBI.<%= projectname %>.Service.Core.Services.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using System.Collections.Generic;

/*
 *  Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
 * 
 * 	Add any necessary custom function definitions should be added
 *  to this file 
 *  
 *  To expose a function to the Controller layer for the 
 *  <%= entityinfo.PropertyName %> entity or another service 
 *  please define the function or property here
 */
namespace IBI.<%= projectname %>.Service.Services.Interfaces
{
    /// <summary>
    /// Service interface for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
    public partial interface I<%= entityinfo.PropertyName %>Service : IBaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}