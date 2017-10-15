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
		<% for (var i = 0; i < entityinfo.Columns.length; i++) {  var columnData = entityinfo.Columns[i]; if(!columnData.Ignore) { %>
		public <%= columnData.PropertyType %> <%= columnData.PropertyName %> { get; set; }
		<% } } %>
		#endregion Properties
		
		/* DO NOT UPDATE THIS FILE */
    }
}