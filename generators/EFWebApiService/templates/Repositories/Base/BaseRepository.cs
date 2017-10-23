using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	DO NOT UPDATE THIS FILE, ANY CHANGES WILL BE OVERWRITTEN 
/// BY THE ENTITY SCAFFOLDING ON THE NEXT RUN, ADD ANY NECESSARY
/// CODE CHANGES SHOULD BE ADDED TO THE EXTENDED REPOSITORY
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories
{
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Repository
    {
		/* DO NOT UPDATE THIS FILE */
		
		#region Constructors
		
		public <%= entityinfo.PropertyName %>Repository() : base()
		{
			System.Data.Entity.Database.SetInitializer<<%= entityinfo.PropertyName %>Repository>(null); //do not remove
		}
		
		#endregion Constructors
		
		/* DO NOT UPDATE THIS FILE */
    }
}