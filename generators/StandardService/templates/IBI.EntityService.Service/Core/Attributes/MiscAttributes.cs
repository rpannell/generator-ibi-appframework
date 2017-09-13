using System;

namespace IBI.<%= Name %>.Service.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InsertOnlyFieldAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ReadOnlyAttribute : Attribute
    {
    }
}