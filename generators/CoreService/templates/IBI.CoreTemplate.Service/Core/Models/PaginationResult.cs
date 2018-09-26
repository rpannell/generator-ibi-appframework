using System.Collections.Generic;

namespace IBI.<%= Name %>.Service.Core.Models
{
    /// <summary>
    /// The return result of the paging functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginationResult<T>
    {
        #region Properties

        /// <summary>
        /// The entities in a page
        /// </summary>
        public List<T> Rows { get; set; }

        /// <summary>
        /// The total number of rows contain the filter
        /// </summary>
        public int Total { get; set; }

        #endregion Properties
    }
}