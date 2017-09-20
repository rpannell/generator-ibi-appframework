using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IBI.<%= projectname %>.Service.Core.Entities;
using IBI.<%= projectname %>.Service.Core.Attributes;
using static IBI.<%= projectname %>.Service.Core.Attributes.Searchable;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
/// </summary>
namespace IBI.<%= projectname %>.Service.Entities
{
	[Table("<%= entityinfo.TableName %>", Schema = "<%= entityinfo.Schema %>")<% if(entityinfo.IsReadOnly) { %>, ReadOnly()<% } %>]
    public partial class <%= entityinfo.PropertyName %> : Entity<<%= entityinfo.PrimaryKey %>>
    {
		<% for (property in entityinfo.Columns) { %><% if(!columns[property].Ignore) { %>
		[<% if(columns[property].IsPrimaryKey) { %>Key(), <% } %>Column("<%= columns[property].ColumnName %>"<% if(columns[property].DataType == "varchar") { %>, TypeName = "VARCHAR"<% } %>)<% if(columns[property].IsAutoComplete) { %>, AutoComplete()<% } %><% if(columns[property].IsInsertOnly) { %>, InsertOnlyField()<% } %><% if(columns[property].SearchTypeAttribute != "") { %>, SearchAble(<%= columns[property].SearchTypeAttribute %>)<% } %><% if(columns[property].MaxLength != null && columns[property].MaxLength != "MAX") { %>, StringLength(<%= columns[property].MaxLength %>), MaxLength(<%= columns[property].MaxLength %>)<% } %>]
		public <%= columns[property].PropertyType %> <%= columns[property].PropertyName %> { get; set; }
		<% } %><% } %>
    }
}