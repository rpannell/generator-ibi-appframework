using System;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// 
/// Misc attributes for Entity Framework entities
/// </summary>
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