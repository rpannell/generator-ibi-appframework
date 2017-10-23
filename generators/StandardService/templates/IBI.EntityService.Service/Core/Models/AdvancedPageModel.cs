using System.Collections.Generic;
/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Models
{
    /// <summary>
    /// A model that represents the advanced paging information to filter
    /// the database information
    /// </summary>
    public class AdvancedPageModel
    {
        /// <summary>
        /// A list of the AdvancedSearch models to 
        /// filter the database 
        /// </summary>
        public List<AdvancedSearch> AdvancedSearch { get; set; }

        /// <summary>
        /// The number of results to return 
        /// </summary>
        public int SearchLimit { get; set; }

        /// <summary>
        /// The row offset
        /// </summary>
        public int SearchOffSet { get; set; }

        /// <summary>
        /// The string to search for any property that is marked "SearchAble"
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// The direction to sort the entity by the
        /// SortString (desc/asc)
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// The property name to sort the entity
        /// </summary>
        public string SortString { get; set; }
    }
}