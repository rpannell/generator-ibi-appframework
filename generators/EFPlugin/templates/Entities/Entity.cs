using System;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom properties should be added
/// to this file and not the <%= entityinfo.PropertyName %> 
/// file in the base folder
/// </summary>
<% if(isPlugin) { %>namespace IBI.<%= projectname %>.Plugin.Models.Entities<% } else { %>namespace IBI.<%= projectname %>.Application.Models.Entities<%}%>
{
    public partial class <%= entityinfo.PropertyName %> 
    {

    }
}