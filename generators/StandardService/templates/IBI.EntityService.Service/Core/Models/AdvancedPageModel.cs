using System.Collections.Generic;

namespace IBI.<%= Name %>.Service.Core.Models
{
    public class AdvancedPageModel
    {
        public List<AdvancedSearch> AdvancedSearch { get; set; }
        public int SearchLimit { get; set; }
        public int SearchOffSet { get; set; }
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
        public string SortString { get; set; }
    }
}