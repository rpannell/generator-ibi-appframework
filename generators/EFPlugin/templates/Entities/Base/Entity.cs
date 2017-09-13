using System;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
/// </summary>
namespace IBI.<%= projectname %>.Plugin.Models.Entities
{
    public partial class <%= entityinfo.PropertyName %>
    {
		<% for (property in columns) { if(!columns[property].Ignore) { %>
		public <%= columns[property].PropertyType %> <%= columns[property].PropertyName %> { get; set; }
		<% } } %>
    }
}