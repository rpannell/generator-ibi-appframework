using IBI.<%= projectname %>.Service.Core.Entities;
using IBI.<%= projectname %>.Service.Core.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IBI.<%= projectname %>.Service.Core.Attributes.Searchable;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	DO NOT UPDATE THIS FILE, ANY CHANGES WILL BE OVERWRITTEN 
/// BY THE ENTITY SCAFFOLDING ON THE NEXT RUN, ADD ANY NECESSARY
/// CODE CHANGES SHOULD BE ADDED TO THE EXTENDED ENTITY
/// </summary>
namespace IBI.<%= projectname %>.Service.Entities
{
	[Table("<%= entityinfo.TableName %>", Schema = "<%= entityinfo.Schema %>")<% if(entityinfo.IsReadOnly) { %>, ReadOnly()<% } %>]
    public partial class <%= entityinfo.PropertyName %> : Entity<<%= entityinfo.PrimaryKey %>>
    {
		/* DO NOT UPDATE THIS FILE */
		
		#region Properties
		
		<% for (var i = 0; i < entityinfo.Columns.length; i++) { var columnData = entityinfo.Columns[i];%><% if(!columnData.Ignore) { %>// property called <%= columnData.PropertyName %> for column <%= columnData.ColumnName %> (<%= columnData.DataType %>)
		[<% if(columnData.IsPrimaryKey) { %>Key(), <% } %>Column("<%= columnData.ColumnName %>"<% if(columnData.DataType == "varchar") { %>, TypeName = "VARCHAR"<% } %>)<% if(columnData.IsAutoComplete) { %>, AutoComplete()<% } %><% if(columnData.IsInsertOnly) { %>, InsertOnlyField()<% } %><% if(columnData.SearchTypeAttribute != "") { %>, SearchAble(<%= columnData.SearchTypeAttribute %>)<% } %><% if(columnData.MaxLength != null && columnData.MaxLength != "MAX") { %>, StringLength(<%= columnData.MaxLength %>), MaxLength(<%= columnData.MaxLength %>)<% } %>]
		public <%= columnData.PropertyType %> <%= columnData.PropertyName %> { get; set; }
		<% } %><% } %>
		
		#endregion Properties
		
		/* DO NOT UPDATE THIS FILE */
	}
}