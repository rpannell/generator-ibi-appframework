using IBI.<%= Name %>.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IBI.<%= Name %>.Application.Utils.Core.Results
{
    public class MiscResults
    {
        #region Methods

        public static ActionResult AutoCompleteResult(List<AutocompleteResults> options)
        {
            var result = new JsonResult(new { _options = options });
            return result;
        }

        #endregion Methods
    }
}