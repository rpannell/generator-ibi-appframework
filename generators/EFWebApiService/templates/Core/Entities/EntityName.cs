using IBI.<%= projectname %>.Service.Core.Entities;

namespace IBI.<%= projectname %>.Service.Entities
{
    /// <summary>
	/// Entity for the <%= entityinfo.TableName %> table
	/// </summary>
	public partial class <%= entityinfo.PropertyName %> : Entity<<%= entityinfo.PrimaryKey %>>
    {
    }
}