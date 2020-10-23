using System.Collections.Generic;

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