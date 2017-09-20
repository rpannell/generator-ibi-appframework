using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace IBI.<%= Name %>.Application.Utils.Core.Results
{
    public class LargeJsonResult : JsonResult
    {
        #region Fields

        private const string JsonRequest_GetNotAllowed = "This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.";

        #endregion Fields

        #region Constructors

        public LargeJsonResult()
        {
            this.MaxJsonLength = 102400000;
            this.RecursionLimit = 1000;
        }

        public LargeJsonResult(object data)
            : this()
        {
            this.Data = data;
        }

        #endregion Constructors

        #region Properties

        public int MaxJsonLength { get; set; }
        public int RecursionLimit { get; set; }

        #endregion Properties

        #region Methods

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(JsonRequest_GetNotAllowed);
            }

            var response = context.HttpContext.Response;
            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                var serializer = new JavaScriptSerializer() { MaxJsonLength = MaxJsonLength, RecursionLimit = RecursionLimit };
                response.Write(serializer.Serialize(Data));
            }
        }

        #endregion Methods
    }
}