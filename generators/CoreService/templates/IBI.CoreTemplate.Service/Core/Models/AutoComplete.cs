namespace IBI.<%= Name %>.Service.Core.Models
{
    /// <summary>
    /// Created by Genie 2018-04-04, 02:18 PM by verion 1.1.25
    /// </summary>
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