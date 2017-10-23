using IBI.<%= projectname %>.Service.Core.Entities;

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
///	Add any necessary custom properties for the <%= entityinfo.PropertyName %> 
/// entity should be added to this file 
/// 
/// Any necessary properties that aren't mapped to a database column
/// or any necessary extensions should be added as a virtual property
/// Any extensions that aren't marked as virtual will be ignored
/// Properties that don't map directly to a database column should 
/// be labeled with "[NotMapped()]" attribute.
/// </summary>
namespace IBI.<%= projectname %>.Service.Entities
{
	public partial class <%= entityinfo.PropertyName %> : Entity<<%= entityinfo.PrimaryKey %>>
	{
	}
}