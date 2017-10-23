/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service.Core.Models
{
    public class AutoComplete
    {
        #region Properties
        /// <summary>
        /// The number of results to return
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// The term to search for
        /// </summary>
        public object SearchTerm { get; set; }

        #endregion Properties
    }
}