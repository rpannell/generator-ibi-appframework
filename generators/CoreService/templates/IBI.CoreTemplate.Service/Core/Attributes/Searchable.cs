using System;

namespace IBI.<%= Name %>.Service.Core.Attributes
{
    /// <summary>
    /// Handles the Searchable attributes on entities
    /// </summary>
    public class Searchable
    {
        #region Enums

        /// <summary>
        /// Selector on how to search the database
        /// </summary>
        public enum SearchAbleType
        {
            /// <summary>
            /// If equal
            /// </summary>
            Equal,

            /// <summary>
            /// Not Equal
            /// </summary>
            NotEqual,

            /// <summary>
            /// A like query
            /// </summary>
            Contains,

            /// <summary>
            /// starts with
            /// </summary>
            StartsWith,

            /// <summary>
            /// Ends with
            /// </summary>
            EndsWith,

            /// <summary>
            /// Greater than
            /// </summary>
            GreaterThan,

            /// <summary>
            /// Greater than or equal to
            /// </summary>
            GreaterThanEqual,

            /// <summary>
            /// less than
            /// </summary>
            LessThan,

            /// <summary>
            /// less than or equal to
            /// </summary>
            LessThanEqual,

            /// <summary>
            /// Full Text search
            /// </summary>
            FullText
        }

        #endregion Enums

        #region Classes

        /// <summary>
        /// Attribute used on a property with in an entity
        /// to advise the repository that the property
        /// can be used with doing a generic or advanced search
        /// </summary>
        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public class SearchAbleAttribute : Attribute
        {
            #region Constructors

            /// <summary>
            /// Sets the entity property as searchable with the search terms
            /// is contained in the property value
            /// </summary>
            public SearchAbleAttribute()
            {
                this.SearchType = SearchAbleType.Contains;
            }

            /// <summary>
            /// Sets the entity property as searchable with the search terms
            /// is contained in the property value.  The alias name can a string
            /// that represents the alias name in the database.
            /// </summary>
            /// <param name="aliasName">The name of the property at the database level, can be a dot notation to a sub-entity property</param>
            public SearchAbleAttribute(string aliasName)
                : this()
            {
                this.AliasName = aliasName;
            }

            /// <summary>
            /// Sets an entity property as searchable with a specific database name and searchable type
            /// </summary>
            /// <param name="aliasName">The name of the property at the database level, can be a dot notation to a sub-entity property</param>
            /// <param name="searchType">How to search the property at the database level</param>
            public SearchAbleAttribute(string aliasName, SearchAbleType searchType)
                : this(aliasName)
            {
                this.SearchType = searchType;
            }

            /// <summary>
            /// Sets an entity property as searchable by a specific search type
            /// </summary>
            /// <param name="searchType">How to search the property at the database level</param>
            public SearchAbleAttribute(SearchAbleType searchType)
            {
                this.SearchType = searchType;
            }

            #endregion Constructors

            #region Properties

            /// <summary>
            /// The database alias name to the property
            /// </summary>
            public string AliasName { get; set; }

            /// <summary>
            /// How to search the property
            /// </summary>
            public SearchAbleType SearchType { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}