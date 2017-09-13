using System;

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

        /*
         * Attribute used on a property with in an entity
         * to advise the repository that the property
         * can be used with doing a generic or advanced search
         */

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public class SearchAbleAttribute : Attribute
        {
            #region Constructors

            public SearchAbleAttribute()
            {
                this.SearchType = SearchAbleType.Contains;
            }

            public SearchAbleAttribute(string aliasName)
                : this()
            {
                this.AliasName = aliasName;
            }

            public SearchAbleAttribute(string aliasName, SearchAbleType searchType)
                : this(aliasName)
            {
                this.SearchType = searchType;
            }

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