using Microsoft.AspNetCore.Mvc;

namespace IBI.<%= Name %>.Application.Utils.Core.Results
{
    public static class SuccessfulResult
    {
        #region Methods

        /// <summary>
        /// Helper to return a SuccessfulJSONResult that IsSuccessful is true and the error message is blank
        /// </summary>
        /// <returns></returns>
        public static ActionResult SuccessfulJSONResult()
        {
            return SuccessfulJSONResult(true, string.Empty);
        }

        /// <summary>
        /// Helper to ensure success json result looks the same across the board
        /// </summary>
        /// <param name="isSuccessful"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ActionResult SuccessfulJSONResult(bool isSuccessful, string errorMessage)
        {
            var result = new JsonResult(new { IsSuccessful = isSuccessful, ErrorMessage = errorMessage });
            return result;
        }

        /// <summary>
        /// Create a successful result with the output data
        /// </summary>
        /// <param name="isSuccessful"></param>
        /// <param name="errorMessage"></param>
        /// <param name="outputData"></param>
        /// <returns></returns>
        public static ActionResult SuccessfulJSONResult(bool isSuccessful, string errorMessage, object outputData)
        {
            var result = new JsonResult(new MVCResult(isSuccessful, errorMessage, outputData));
            return result;
        }

        /// <summary>
        /// Create a result with the IsSuccesful flag set to true with the output data
        /// </summary>
        /// <param name="outputData"></param>
        /// <returns></returns>
        public static ActionResult SuccessfulJSONResult(object outputData)
        {
            return SuccessfulJSONResult(true, string.Empty, outputData);
        }

        /// <summary>
        /// Create a result with the IsSuccessflag set to false with error message string
        /// and the output data set
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="outputData"></param>
        /// <returns></returns>
        public static ActionResult SuccessfulJSONResult(string errorMessage, object outputData)
        {
            return SuccessfulJSONResult(false, errorMessage, outputData);
        }

        /// <summary>
        /// Creates the successful json result based if there is an error message or not
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static ActionResult SuccessfulJSONResult(string errorMessage)
        {
            return SuccessfulJSONResult(errorMessage != string.Empty ? false : true, errorMessage);
        }

        #endregion Methods

        #region Classes

        private class MVCResult
        {
            #region Constructors

            public MVCResult(bool isSuccessFull, string errorMessage)
            {
                this.IsSuccessful = isSuccessFull;
                this.ErrorMessage = errorMessage;
            }

            public MVCResult(bool isSuccessFull, string errorMessage, object outputData)
                : this(isSuccessFull, errorMessage)
            {
                this.OutputData = outputData;
            }

            #endregion Constructors

            #region Properties

            public string ErrorMessage { get; set; }
            public bool IsSuccessful { get; set; }
            public object OutputData { get; set; }

            #endregion Properties
        }

        #endregion Classes
    }
}