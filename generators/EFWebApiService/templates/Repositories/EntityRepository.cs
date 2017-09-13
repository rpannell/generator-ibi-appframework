using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
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