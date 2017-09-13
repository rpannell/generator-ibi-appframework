using System;

namespace IBI.<%= Name %>.Service.Core.Attributes
{
    ///
    ///Advises the attribute what type of property
    ///in the entity are we dealing with to join
    ///correctly
    ///

    public enum AutoCompletePropertyType
    {
        Int,
        String,
        Bit
    }

    /// <summary>
    /// Attribute used on a property within an entity
    /// that advises the repository to use the property
    /// when doing an auto-complete search
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AutoComplete : Attribute
    {
        #region Constructors

        public AutoComplete()
        {
            this.IsEqual = false;
            this.PropertyType = AutoCompletePropertyType.String;
        }

        public AutoComplete(bool isEqual)
        {
            this.IsEqual = isEqual;
            this.PropertyType = AutoCompletePropertyType.String;
        }

        public AutoComplete(string propertyName, bool isEqual)
        {
            this.PropertyName = propertyName;
            this.IsEqual = isEqual;
            this.PropertyType = AutoCompletePropertyType.String;
        }

        public AutoComplete(string propertyName, bool isEqual, AutoCompletePropertyType propertyType)
        {
            this.PropertyName = propertyName;
            this.IsEqual = isEqual;
            this.PropertyType = propertyType;
        }

        public AutoComplete(string propertyName)
            : this()
        {
            this.PropertyName = propertyName;
        }

        #endregion Constructors

        #region Properties

        public bool IsEqual { get; set; }
        public string PropertyName { get; set; }
        public AutoCompletePropertyType PropertyType { get; set; }

        #endregion Properties
    }
}