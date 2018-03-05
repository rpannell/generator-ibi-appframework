using System.Collections.Generic;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
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
        public List<T> rows { get; set; }

        /// <summary>
        /// The total number of rows contain the filter
        /// </summary>
        public int total { get; set; }

        #endregion Properties
    }
}