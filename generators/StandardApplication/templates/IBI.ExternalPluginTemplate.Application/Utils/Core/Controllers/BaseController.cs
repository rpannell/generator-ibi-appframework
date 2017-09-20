using IBI.<%= Name %>.Application.Utils.Core.Results;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IBI.<%= Name %>.Application.Utils.Core.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        #region Methods

        /// <summary>
        /// Returns auto complete result with a list of options
        /// </summary>
        /// <param name="options"></param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Autocomplete(List<Models.AutocompleteResults> options)
        {
            return MiscResults.AutoCompleteResult(options);
        }

        /// <summary>
        /// Creates a short cut to create a LargeJson
        /// </summary>
        /// <param name="data">The data</param>
        /// <returns>JsonResult</returns>
        protected internal JsonResult LargeJson(object data)
        {
            return new LargeJsonResult(data) { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Returns successful json result
        /// </summary>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Successful()
        {
            return SuccessfulResult.SuccessfulJSONResult();
        }

        /// <summary>
        /// Returns successful json result based if it's successful and the error message
        /// </summary>
        /// <param name="isSuccessful">boolean if the result is successful</param>
        /// <param name="errorMessage">The error message if any</param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Successful(bool isSuccessful, string errorMessage)
        {
            return SuccessfulResult.SuccessfulJSONResult(isSuccessful, errorMessage);
        }

        /// <summary>
        /// Returns successful json based on the success flag, error message and the data to return
        /// </summary>
        /// <param name="isSuccessful">The flag to set if the result is true or false</param>
        /// <param name="errorMessage">The error message</param>
        /// <param name="outputData">Data to return</param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Successful(bool isSuccessful, string errorMessage, object outputData)
        {
            return SuccessfulResult.SuccessfulJSONResult(isSuccessful, errorMessage, outputData);
        }

        /// <summary>
        /// Returns successful json result that is successful and return data
        /// </summary>
        /// <param name="outputData">Data to return</param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Successful(object outputData)
        {
            return SuccessfulResult.SuccessfulJSONResult(outputData);
        }

        /// <summary>
        /// Returns successful json result that is not successful with an error message and data that returns with it
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <param name="outputData">Data to return</param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Successful(string errorMessage, object outputData)
        {
            return SuccessfulResult.SuccessfulJSONResult(errorMessage, outputData);
        }

        /// <summary>
        /// Returns a successful json result that is not successful and gives the error message
        /// </summary>
        /// <param name="errorMessage">The error message</param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Successful(string errorMessage)
        {
            return SuccessfulResult.SuccessfulJSONResult(errorMessage);
        }

        #endregion Methods
    }
}