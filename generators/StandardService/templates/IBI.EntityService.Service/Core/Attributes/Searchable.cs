using System;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Attributes
{
    public class Searchable
    {
        public enum SearchAbleType
        {
            Equal,
            NotEqual,
            Contains,
            StartsWith,
            EndsWith,
            GreaterThan,
            GreaterThanEqual,
            LessThan,
            LessThanEqual,
            FullText
        }

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

            public string AliasName { get; set; }
            public SearchAbleType SearchType { get; set; }

            #endregion Properties
        }
    }
}