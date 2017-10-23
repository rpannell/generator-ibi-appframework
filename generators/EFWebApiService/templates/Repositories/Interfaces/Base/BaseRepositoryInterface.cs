using IBI.<%= projectname %>.Service.Core.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Entities;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	DO NOT UPDATE THIS FILE, ANY CHANGES WILL BE OVERWRITTEN 
/// BY THE ENTITY SCAFFOLDING ON THE NEXT RUN, ADD ANY NECESSARY
/// CODE CHANGES SHOULD BE ADDED TO THE EXTENDED INTERFACE
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories.Interfaces
{
    public partial interface I<%= entityinfo.PropertyName %>Repository : IBaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
		/* DO NOT UPDATE THIS FILE */
    }
}