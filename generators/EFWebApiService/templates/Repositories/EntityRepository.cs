using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom functions should be added
/// to this file and not the <%= entityinfo.PropertyName %>Repository 
/// file in the base folder
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories
{
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Repository
    {
		#region Custom Model Building Use with Caution

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		#endregion Custom Model Building Use with Caution
    }
}