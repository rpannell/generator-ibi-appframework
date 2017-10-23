using System.Web.Mvc;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils.UI
{
    public static class BootstrapButtonsHelper
    {
        #region Default Button

        /// <summary>
        /// Used to create a simple bootstrap default button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonDefault(this HtmlHelper htmlHelper, string buttonText, bool isSubmit, object htmlAttributes, bool isVisible)
        {
            return BootstrapButtonDefault(htmlHelper, buttonText, string.Empty, isSubmit, htmlAttributes, isVisible);
        }

        /// <summary>
        /// Used to create a simple bootstrap default button with a specific id
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="id">The id attribute value</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonDefault(this HtmlHelper htmlHelper, string buttonText, string id, bool isSubmit, object htmlAttributes, bool isVisible)
        {
            return BootstrapButtonDefault(htmlHelper, buttonText, string.Empty, id, isSubmit, htmlAttributes, isVisible);
        }

        /// <summary>
        /// Used to create a simple bootstrap default button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="value">The value of the button</param>
        /// <param name="id">The id attribute value</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonDefault(this HtmlHelper htmlHelper, string buttonText, string value, string id, bool isSubmit, object htmlAttributes, bool isVisible)
        {
            return BootstrapButton(htmlHelper, buttonText, value, id, isSubmit, isVisible, "btn btn-default", htmlAttributes);
        }

        #endregion Default Button

        #region Primary Button

        /// <summary>
        /// Used to create a simple bootstrap primary button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonPrimary(this HtmlHelper htmlHelper, string buttonText, bool isSubmit, object htmlAttributes = null, bool isVisible = true)
        {
            return BootstrapButtonPrimary(htmlHelper, buttonText, string.Empty, string.Empty, isSubmit, htmlAttributes, isVisible);
        }

        /// <summary>
        /// Used to create a simple bootstrap primary button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="value">The value of the button</param>
        /// <param name="id">The id attribute value</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonPrimary(this HtmlHelper htmlHelper, string buttonText, string value, string id, bool isSubmit, object htmlAttributes = null, bool isVisible = true)
        {
            return BootstrapButton(htmlHelper, buttonText, value, id, isSubmit, isVisible, "btn btn-primary", htmlAttributes);
        }

        #endregion Primary Button

        #region Warning Button

        /// <summary>
        /// Used to create a warning bootstrap button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="value">The value of the button</param>
        /// <param name="id">The id attribute value</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonWarning(this HtmlHelper htmlHelper, string buttonText, string value, string id, bool isSubmit, object htmlAttributes, bool isVisible)
        {
            return BootstrapButton(htmlHelper, buttonText, value, id, isSubmit, isVisible, "btn btn-warning", htmlAttributes);
        }

        #endregion Warning Button

        #region Success Button

        /// <summary>
        /// Used to create a success bootstrap button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="value">The value of the button</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapButtonSuccess(this HtmlHelper htmlHelper, string buttonText, string value, bool isSubmit, object htmlAttributes = null, bool isVisible = true)
        {
            return BootstrapButton(htmlHelper, buttonText, value, string.Empty, isSubmit, isVisible, "btn btn-success", htmlAttributes);
        }

        #endregion Success Button

        #region Button Builder

        /// <summary>
        /// Used to create the bootstrap button
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to help create the button</param>
        /// <param name="buttonText">The text to display on the button</param>
        /// <param name="value">The value of the button</param>
        /// <param name="id">The id attribute value</param>
        /// <param name="isSubmit">Marks the button as a submit button</param>
        /// <param name="isVisible">Marks the button as hidden</param>
        /// <param name="btnCssClass"></param>
        /// <param name="htmlAttributes">The extra attributes a programmer can update or override</param>
        /// <returns>MvcHtmlString</returns>
        private static MvcHtmlString BootstrapButton(this HtmlHelper htmlHelper, string buttonText, string value, string id, bool isSubmit, bool isVisible, string btnCssClass, object htmlAttributes = null)
        {
            var cssClass = string.Format("{0} {1}", isVisible ? string.Empty : "hidden", btnCssClass);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var builder = new TagBuilder("button");

            if (htmlAttributes != null) builder.MergeAttributes(attributes);
            if (!string.IsNullOrWhiteSpace(value)) builder.Attributes.Add("value", value);
            if (!string.IsNullOrWhiteSpace(id)) builder.Attributes.Add("id", id);

            builder.Attributes.Add("type", isSubmit ? "submit" : "button");
            builder.AddCssClass(cssClass);
            builder.InnerHtml = buttonText;
            return new MvcHtmlString(builder.ToString());
        }

        #endregion Button Builder
    }
}