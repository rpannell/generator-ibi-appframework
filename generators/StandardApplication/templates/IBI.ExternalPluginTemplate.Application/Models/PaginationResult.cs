using System.Collections.Generic;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Models
{
    public class PaginationResult<T>
    {
        #region Properties

        public List<T> rows { get; set; }
        public int total { get; set; }

        #endregion Properties
    }
}