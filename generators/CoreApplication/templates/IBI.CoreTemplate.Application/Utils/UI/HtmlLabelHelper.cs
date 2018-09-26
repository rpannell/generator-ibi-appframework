using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace IBI.<%= Name %>.Application.Utils.UI
{
    public static class HtmlLabelHelper
    {
        #region Methods

        /// <summary>
        /// Creates a HTML label with an id and value
        /// Note:   if the value is null or empty string then the function will
        ///         convert the value to a string with just a space
        /// </summary>
        /// <param name="helper">The MVC HtmlHelper</param>
        /// <param name="id">The value of the id attribute</param>
        /// <param name="labelValue">The value of the label</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent CleanLabel(this IHtmlHelper helper, string id, string labelValue, object htmlAttributes = null)
        {
            var rtunString = "<label";
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var idString = id != null && id != string.Empty ? string.Format("id=\"{0}\"", id) : string.Empty;
            var labelString = labelValue != null && labelValue != string.Empty ? labelValue : " ";
            foreach (var attr in attributes)
            {
                var key = attr.Key;
                var val = attr.Value;

                rtunString += string.Format(" {0}=\"{1}\"", key, val);
            }

            rtunString += string.Format(" {0}>{1}</label>", idString, labelValue);

            return new HtmlString(rtunString);
        }

        /// <summary>
        /// Creates a HTML label with just a value
        /// Note:   if the value is null or empty string then the function will
        ///         convert the value to a string with just a space
        /// </summary>
        /// <param name="helper">The MVC HtmlHelper</param>
        /// <param name="labelValue">The value of the label</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent CleanLabel(this IHtmlHelper helper, string labelValue, object htmlAttributes = null)
        {
            return CleanLabel(helper, string.Empty, labelValue, htmlAttributes);
        }

        #endregion Methods
    }
}