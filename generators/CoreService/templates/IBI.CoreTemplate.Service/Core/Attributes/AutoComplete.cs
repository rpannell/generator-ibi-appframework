using System;

namespace IBI.<%= Name %>.Service.Core.Attributes
{
    /// <summary>
    /// Advises the attribute what type of property
    /// in the entity are we dealing with to join
    /// correctly
    /// </summary>
    public enum AutoCompletePropertyType
    {
        /// <summary>
        /// Is the property an Int
        /// </summary>
        Int,

        /// <summary>
        /// Is the property a string
        /// </summary>
        String,

        /// <summary>
        /// Is the property a true false field
        /// </summary>
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

        /// <summary>
        /// Create an AutoComplete that is not equal
        /// and the property is a string value
        /// </summary>
        public AutoComplete() : this(false)
        {
            this.IsEqual = false;
            this.PropertyType = AutoCompletePropertyType.String;
        }

        /// <summary>
        /// Create an AutoComplete where the property is a string
        /// </summary>
        /// <param name="isEqual">Should the property equal the search term</param>
        public AutoComplete(bool isEqual)
        {
            this.IsEqual = isEqual;
            this.PropertyType = AutoCompletePropertyType.String;
        }

        /// <summary>
        /// Create an AutoComplete with a specific property name that
        /// can be different from the actual name of the entity property
        /// it's attached to
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="isEqual">Should the property equal the search term</param>
        public AutoComplete(string propertyName, bool isEqual)
        {
            this.PropertyName = propertyName;
            this.IsEqual = isEqual;
            this.PropertyType = AutoCompletePropertyType.String;
        }

        /// <summary>
        /// Create an AutoComplete with a specific property name,
        /// specific property type and if the search term should equal
        /// the value of the property from the database
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <param name="isEqual">Should the property equal the search term</param>
        /// <param name="propertyType">The type of the property to AutoComplete</param>
        public AutoComplete(string propertyName, bool isEqual, AutoCompletePropertyType propertyType)
        {
            this.PropertyName = propertyName;
            this.IsEqual = isEqual;
            this.PropertyType = propertyType;
        }

        /// <summary>
        /// Creates an AutoComplete where the property name may be different
        /// than the actual property
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        public AutoComplete(string propertyName)
            : this()
        {
            this.PropertyName = propertyName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Should the search team equal the value of the property
        /// </summary>
        public bool IsEqual { get; set; }

        /// <summary>
        /// Name of the property to search
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// The type of the property to auto complete on
        /// </summary>
        public AutoCompletePropertyType PropertyType { get; set; }

        #endregion Properties
    }
}