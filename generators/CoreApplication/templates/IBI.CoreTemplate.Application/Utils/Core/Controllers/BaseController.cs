using IBI.<%= Name %>.Application.Models;
using IBI.<%= Name %>.Application.Utils.Core.Results;
using IBI.<%= Name %>.Application.Utils.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace IBI.<%= Name %>.Application.Utils.Core.Controllers
{
    [Authorize(Policy = "<%= Name %>")]
    public class BaseController : Controller
    {
        #region Constructors

        public BaseController(IHttpContextAccessor httpContext)
        {
            if (httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var imp = UserInformation.GetImpersonatingPayeeId(httpContext);
                var currentImp = httpContext.HttpContext.Session.GetString("CurrentImp");
                if (string.IsNullOrEmpty(currentImp) || imp != currentImp)
                {
                    httpContext.HttpContext.Session.SetString("CurrentImp", imp);
                    httpContext.HttpContext.Session.Remove("Menu");
                }

                if (imp != currentImp || !httpContext.HttpContext.Session.Keys.ToList().Exists(x => x == "Menu"))
                {
                    var ms = httpContext.HttpContext.RequestServices.GetService<IMenuService>();
                    var menu = ms.GetMenu();
                    var script = ms.GetMenuJavaScript();
                    if (!string.IsNullOrEmpty(menu))
                    {
                        httpContext.HttpContext.Session.SetString("Menu", menu);
                        httpContext.HttpContext.Session.SetString("MenuScript", script);
                    }
                }
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Returns auto complete result with a list of options
        /// </summary>
        /// <param name="options"></param>
        /// <returns>ActionResult</returns>
        protected internal ActionResult Autocomplete(List<AutocompleteResults> options)
        {
            return MiscResults.AutoCompleteResult(options);
        }

        protected internal ActionResult AutoCompleteResult(List<AutocompleteResults> options)
        {
            var result = new JsonResult(new { _options = options });
            return result;
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