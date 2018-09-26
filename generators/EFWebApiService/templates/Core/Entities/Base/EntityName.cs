using IBI.<%= projectname %>.Service.Core.Attributes;
using IBI.<%= projectname %>.Service.Core.Entities;
using IBI.<%= projectname %>.Service.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IBI.<%= projectname %>.Service.Core.Attributes.Searchable;

namespace IBI.<%= projectname %>.Service.Core.Context
{
    /// <summary>
    /// Adds <%= entityinfo.PropertyName %> to the set of tables the main context can use
    /// </summary>
    public partial class MainContext : DbContext
    {
        #region Properties

        internal DbSet<<%= entityinfo.PropertyName %>> <%= entityinfo.PropertyName %> { get; set; }

        #endregion Properties
    }
}

namespace IBI.<%= projectname %>.Service.Entities
{
    /// <summary>
	/// Entity for the <%= entityinfo.TableName %> table
	/// </summary>
	[Table("<%= entityinfo.TableName %>", Schema = "<%= entityinfo.Schema %>")<% if(entityinfo.IsReadOnly) { %>, ReadOnly()<% } %>]
    public partial class <%= entityinfo.PropertyName %> : Entity<<%= entityinfo.PrimaryKey %>>
    {
        /* DO NOT UPDATE THIS FILE */

        #region Properties
		
		<% for (var i = 0; i < entityinfo.Columns.length; i++) { var columnData = entityinfo.Columns[i];%><% if(!columnData.Ignore) { %>
		/// <summary>
		/// Property Name: <%= columnData.PropertyName %>
		/// ColumnName: <%= columnData.ColumnName %>
		/// ColumnType: <%= columnData.DataType %>
		/// </summary>
		[<% if(columnData.IsPrimaryKey) { %>Key(), <% } %>Column("<%= columnData.ColumnName %>"<% if(columnData.DataType == "varchar") { %>, TypeName = "VARCHAR(<%= columnData.MaxLength %>)" <% } %>)<% if(columnData.IsAutoComplete) { %>, AutoComplete()<% } %><% if(columnData.IsInsertOnly) { %>, InsertOnlyField()<% } %><% if(columnData.SearchTypeAttribute != "") { %>, SearchAble(<%= columnData.SearchTypeAttribute %>)<% } %><% if(columnData.MaxLength != null && columnData.MaxLength != "MAX") { %>, StringLength(<%= columnData.MaxLength %>), MaxLength(<%= columnData.MaxLength %>)<% } %>]
		public <%= columnData.PropertyType %> <%= columnData.PropertyName %> { get; set; }
		<% } %><% } %>
		
		#endregion Properties

        /* DO NOT UPDATE THIS FILE */
    }
}