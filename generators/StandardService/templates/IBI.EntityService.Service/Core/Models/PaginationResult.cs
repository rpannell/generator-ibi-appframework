using System.Collections.Generic;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Models
{
    public class PaginationResult<T>
    {
        #region Properties
        /// <summary>
        /// The entities in a page
        /// </summary>
        public List<T> rows { get; set; }

        /// <summary>
        /// The total number of rows contain the filter
        /// </summary>
        public int total { get; set; }

        #endregion Properties
    }
}