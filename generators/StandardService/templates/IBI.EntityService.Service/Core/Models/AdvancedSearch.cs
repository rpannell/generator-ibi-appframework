using System.Collections.Generic;

namespace IBI.<%= Name %>.Service.Core.Models
{
    public class AdvancedSearch
    {
        #region Enums

        public enum AdvancedSearchType { Between, NotEqual, Equal, LessThan, LessThanEqual, GreaterThan, GreaterThanEqual, Like, In, IsNull, IsNotNull };

        #endregion Enums

        #region Properties

        /// <summary>
        /// Used for int values because for some reason
        /// an int value on one side would convert to a long
        /// here and thus causing issues
        /// </summary>
        public int? IntValue { get; set; }

        public List<object> ListValue { get; set; }
        public string PropertyName { get; set; }
        public AdvancedSearchType TypeOfSearch { get; set; }
        public object Value { get; set; }

        /// <summary>
        /// Only should be set for between functionality
        /// </summary>
        public object Value2 { get; set; }

        #endregion Properties

        #region Constructors

        public AdvancedSearch()
        {
        }

        public AdvancedSearch(string propertyName, AdvancedSearchType type, object valueA, object valueB)
        {
            this.PropertyName = propertyName;
            this.TypeOfSearch = type;
            if (valueA.GetType() == typeof(int) || valueA.GetType() == typeof(int?))
            {
                this.IntValue = (int?)valueA;
            }
            else
            {
                this.Value = valueA;
            }
            this.Value2 = valueB;
        }

        public AdvancedSearch(string propertyName, AdvancedSearchType type, List<object> values)
        {
            this.PropertyName = propertyName;
            this.TypeOfSearch = type;
            this.ListValue = values;
        }

        #endregion Constructors

        #region Methods

        public static AdvancedSearch Between(string propertyName, object a, object b)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.Between, a, b);
        }

        public static AdvancedSearch Equal(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.Equal, a, null);
        }

        public static AdvancedSearch GreaterThan(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.GreaterThan, a, null);
        }

        public static AdvancedSearch GreaterThanOrEqualTo(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.GreaterThanEqual, a, null);
        }

        public static AdvancedSearch In(string propertyName, List<object> a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.In, a);
        }

        public static AdvancedSearch IsNotNull(string propertyName)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.IsNotNull, null, null);
        }

        public static AdvancedSearch IsNull(string propertyName)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.IsNull, null, null);
        }

        public static AdvancedSearch LessThan(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.LessThan, a, null);
        }

        public static AdvancedSearch LessThanOrEqualTo(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.LessThanEqual, a, null);
        }

        public static AdvancedSearch Like(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.Like, a, null);
        }

        public static AdvancedSearch NotEqual(string propertyName, object a)
        {
            return new AdvancedSearch(propertyName, AdvancedSearchType.NotEqual, a, null);
        }

        #endregion Methods
    }
}