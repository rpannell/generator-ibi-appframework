using System;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	DO NOT UPDATE THIS FILE, ANY CHANGES WILL BE OVERWRITTEN 
/// BY THE ENTITY SCAFFOLDING ON THE NEXT RUN, ADD ANY NECESSARY
/// CODE CHANGES SHOULD BE ADDED TO THE EXTENDED ENTITY
/// </summary>
<% if(isPlugin) { %>namespace IBI.<%= projectname %>.Plugin.Models.Entities<% } else { %>namespace IBI.<%= projectname %>.Application.Models.Entities<%}%>
{
    public partial class <%= entityinfo.PropertyName %>
    {
		/* DO NOT UPDATE THIS FILE */
		
		#region Properties
		<% for (property in columns) { if(!columns[property].Ignore) { %>
		public <%= columns[property].PropertyType %> <%= columns[property].PropertyName %> { get; set; }
		<% } } %>
		#endregion Properties
		
		/* DO NOT UPDATE THIS FILE */
    }
}