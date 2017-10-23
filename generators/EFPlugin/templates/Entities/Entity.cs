using System;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom properties should be added
/// to this file and not the <%= entityinfo.PropertyName %> 
/// file in the base folder
/// 
/// Copy any custom properties created in the <%= entityinfo.PropertyName %>
/// entity in the web-api service to this file so the plugin has access to 
/// those properties (if necessary)
/// </summary>
<% if(isPlugin) { %>namespace IBI.<%= projectname %>.Plugin.Models.Entities<% } else { %>namespace IBI.<%= projectname %>.Application.Models.Entities<%}%>
{
    public partial class <%= entityinfo.PropertyName %> 
    {

    }
}