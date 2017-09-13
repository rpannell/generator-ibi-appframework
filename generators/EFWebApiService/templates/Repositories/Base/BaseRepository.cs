using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Core.Repositories;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories
{
    public partial class <%= entityinfo.PropertyName %>Repository : BaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Repository
    {
		public <%= entityinfo.PropertyName %>Repository() : base()
		{
			System.Data.Entity.Database.SetInitializer<<%= entityinfo.PropertyName %>Repository>(null); //do not remove
		}
    }
}