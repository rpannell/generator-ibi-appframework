using IBI.<%= projectname %>.Service.Core.Entities;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	Add any necessary custom properties should be added
/// to this file and not the <%= entityinfo.PropertyName %> 
/// file in the base folder
/// </summary>
namespace IBI.<%= projectname %>.Service.Entities
{
	public partial class <%= entityinfo.PropertyName %> : Entity<<%= entityinfo.PrimaryKey %>>
	{
	}
}