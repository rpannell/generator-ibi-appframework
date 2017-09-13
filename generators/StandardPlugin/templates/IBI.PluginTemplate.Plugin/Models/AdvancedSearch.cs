using System.Collections.Generic;

namespace IBI.<%= Name %>.Plugin.Models
{
    public class AdvancedSearch
    {
        #region Enums

        public enum AdvancedSearchType { Between, Equal, LessThan, GreaterThan, Like, In, EqOrNull, IsNull, IsNotNull, FullTextSearch };

        #endregion Enums

        #region Properties

        /// <summary>
        /// Used for int values because for some reason
        /// an int value on one side would convert to a long
        /// here and thus causing issues
        /// </summary>
        public int? IntValue { get; set; }

        /// <summary>
        /// Used for the In property
        /// </summary>
        public List<int> ListIntValue { get; set; }

        public List<object> ListValue { get; set; }
        public string PropertyName { get; set; }
        public AdvancedSearchType TypeOfSearch { get; set; }
        public object Value { get; set; }

        /// <summary>
        /// Only should be set for between functionality
        /// </summary>
        public object Value2 { get; set; }

        #endregion Properties
    }
}