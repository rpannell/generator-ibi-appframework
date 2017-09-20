using IBI.<%= Name %>.Application.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IBI.<%= Name %>.Application.Utils.Core.Results
{
    public class MiscResults
    {
        #region Methods

        public static ActionResult AutoCompleteResult(List<AutocompleteResults> options)
        {
            var result = new JsonResult();
            result.Data = new { _options = options };
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;
        }

        #endregion Methods
    }
}