namespace IBI.<%= Name %>.Service.Core.Entities
{
    /// <summary>
    /// The base class for all sql server entities
    /// </summary>
    public class BaseEntity { }

    /// <summary>
    /// The entity that contains the primary key type
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public class Entity<TPrimaryKey> : BaseEntity
    {
    }
}