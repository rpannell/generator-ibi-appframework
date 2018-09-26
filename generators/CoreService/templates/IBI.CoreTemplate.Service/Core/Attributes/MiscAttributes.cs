using System;

namespace IBI.<%= Name %>.Service.Core.Attributes
{
    /// <summary>
    /// Sets a property as insert only
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InsertOnlyFieldAttribute : Attribute
    {
    }

    /// <summary>
    /// Sets an entity as read-only
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ReadOnlyAttribute : Attribute
    {
    }
}