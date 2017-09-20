using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace IBI.<%= Name %>.Application.Utils.UI
{
    /// <summary>
    /// Used to create Bootstrap Text-boxes
    /// </summary>
    public static class BootstrapTextBoxHelper
    {
        #region TextBox

        /// <summary>
        /// Create a bootstrap text-box that doesn't link to a model property
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="name">The value of the name attribute of the text-box</param>
        /// <param name="value">The initial value of the text-box</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns></returns>
        public static MvcHtmlString BootstrapTextBox(this HtmlHelper htmlHelper, string name, object value, object htmlAttributes, bool isReadOnly = false)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            return BootstrapTextBox(htmlHelper, name, value, values, isReadOnly);
        }

        /// <summary>
        /// Create a bootstrap text-box that doesn't link to a model property
        /// </summary>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="name">The value of the name attribute of the text-box</param>
        /// <param name="value">The initial value of the text-box</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        private static MvcHtmlString BootstrapTextBox(this HtmlHelper htmlHelper, string name, object value, IDictionary<string, object> htmlAttributes, bool isReadOnly = false)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("class")) htmlAttributes.Add("class", string.Empty);
            htmlAttributes["class"] += " form-control";
            if (isReadOnly) htmlAttributes.Add("readonly", "readonly");
            return htmlHelper.TextBox(name, value, htmlAttributes);
        }

        #endregion TextBox

        #region Text-box For

        /// <summary>
        /// Create a bootstrap text-box based on a model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var values = new Dictionary<string, object>();
            return BootstrapTextBoxFor(htmlHelper, expression, values, false);
        }

        /// <summary>
        /// Create a bootstrap text-box based on a model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            return BootstrapTextBoxFor(htmlHelper, expression, values, false);
        }

        /// <summary>
        /// Create a bootstrap text-box based on a model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool isReadOnly)
        {
            var values = new Dictionary<string, object>();
            return BootstrapTextBoxFor(htmlHelper, expression, values, isReadOnly);
        }

        /// <summary>
        /// Create a bootstrap text-box based on a model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, bool isReadOnly = false)
        {
            return BootstrapTextBoxFor(htmlHelper, expression, htmlAttributes, isReadOnly);
        }

        /// <summary>
        /// Create a bootstrap text-box based on a model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, bool isReadOnly = false)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("class")) htmlAttributes.Add("class", string.Empty);
            htmlAttributes["class"] += " form-control";
            if (isReadOnly) htmlAttributes.Add("readonly", "readonly");
            return htmlHelper.TextBoxFor<TModel, TProperty>(expression, htmlAttributes);
        }

        #endregion Text-box For

        #region TextArea

        /// <summary>
        /// Create a bootstrap text-area based on model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var values = new Dictionary<string, object>();
            return BootstrapTextAreaFor(htmlHelper, expression, values, false);
        }

        /// <summary>
        /// Create a bootstrap text-area based on model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool isReadOnly)
        {
            var values = new Dictionary<string, object>();
            return BootstrapTextAreaFor(htmlHelper, expression, values, isReadOnly);
        }

        /// <summary>
        /// Create a bootstrap text-area based on model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            return BootstrapTextAreaFor(htmlHelper, expression, values, false);
        }

        /// <summary>
        /// Create a bootstrap text-area based on model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool isReadOnly = false)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            return BootstrapTextAreaFor(htmlHelper, expression, values, isReadOnly);
        }

        /// <summary>
        /// Create a bootstrap text-area based on model property
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        private static MvcHtmlString BootstrapTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, bool isReadOnly = false)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("class")) htmlAttributes.Add("class", string.Empty);
            if (!htmlAttributes.ContainsKey("rows"))
            {
                htmlAttributes.Add("rows", string.Empty);
                htmlAttributes["rows"] = 4;
            }
            htmlAttributes["class"] += " form-control";
            if (isReadOnly) htmlAttributes.Add("readonly", "readonly");
            return htmlHelper.TextAreaFor<TModel, TProperty>(expression, htmlAttributes);
        }

        #endregion TextArea

        #region File Upload

        /// <summary>
        /// Create an HTML5 text-box of the file type
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var values = new Dictionary<string, object>();
            return BootstrapTextFileFor(htmlHelper, expression, values);
        }

        /// <summary>
        /// Create an HTML5 text-box of the file type
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapTextFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            return BootstrapTextFileFor(htmlHelper, expression, values);
        }

        /// <summary>
        /// Create an HTML5 text-box of the file type
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="htmlAttributes">The extra attributes a programmer can manage</param>
        /// <returns>MvcHtmlString</returns>
        private static MvcHtmlString BootstrapTextFileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("type"))
            {
                htmlAttributes.Add("type", "file");
            }
            return htmlHelper.BootstrapTextBoxFor(expression, htmlAttributes);
        }

        #endregion File Upload

        #region Number Text-box

        /// <summary>
        /// Create a bootstrap HTML5 number text-box with step value of 1 and a required min/max
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="min">The min value that should be represented</param>
        /// <param name="max">The max value that should be represented</param>
        /// <param name="htmlAttributes">Extra HTML attributes to pass into the HTML code</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapStepperFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int min, int max, bool isReadOnly = false)
        {
            return BootstrapStepperFor(htmlHelper, expression, min, max, null, isReadOnly);
        }

        /// <summary>
        /// Create a bootstrap HTML5 number text-box with step value of 1 and a required min/max
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The type of the model property</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the text box</param>
        /// <param name="expression">The Linq expression to get to the model property</param>
        /// <param name="min">The min value that should be represented</param>
        /// <param name="max">The max value that should be represented</param>
        /// <param name="htmlAttributes">Extra HTML attributes to pass into the HTML code</param>
        /// <param name="isReadOnly">Mark the text box as read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapStepperFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, int min, int max, IDictionary<string, object> htmlAttributes, bool isReadOnly = false)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("type")) htmlAttributes.Add("type", string.Empty);
            if (!htmlAttributes.ContainsKey("min")) htmlAttributes.Add("min", string.Empty);
            if (!htmlAttributes.ContainsKey("max")) htmlAttributes.Add("max", string.Empty);

            htmlAttributes["type"] = "number";
            htmlAttributes["min"] = min;
            htmlAttributes["max"] = max;

            return BootstrapTextBoxFor(htmlHelper, expression, htmlAttributes, isReadOnly);
        }

        #endregion Number Text-box
    }
}