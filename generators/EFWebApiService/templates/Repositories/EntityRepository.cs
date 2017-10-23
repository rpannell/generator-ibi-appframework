using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom functions should be added
/// to this file 
/// 
/// This file contains any necessary database operations for
/// the <%= entityinfo.PropertyName %> entity.  No business
/// logic should be performed in this class and should left
/// in the service layer.  This file is for performing database
/// operations only.  The base operations can be found in the
/// BaseRepository file
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories
{
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Repository
    {
		#region Constructors
		
		public <%= entityinfo.PropertyName %>Repository() : base()
		{
			System.Data.Entity.Database.SetInitializer<<%= entityinfo.PropertyName %>Repository>(null); //do not remove
		}
		
		#endregion Constructors

		#region Custom Model Building Use with Caution

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

		#endregion Custom Model Building Use with Caution

		/* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}