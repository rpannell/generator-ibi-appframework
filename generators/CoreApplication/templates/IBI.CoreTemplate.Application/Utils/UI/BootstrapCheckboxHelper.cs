using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace IBI.<%= Name %>.Application.Utils.UI
{
    /// <summary>
    /// Used to create a Bootstrap Check-box for a model
    /// </summary>
    public static class BootstrapCheckboxHelper
    {
        #region Methods

        /// <summary>
        /// Used to create a bootstrap check-box
        /// </summary>
        /// <typeparam name="TModel">The Model of the view</typeparam>
        /// <param name="htmlHelper">The HtmlHelper used to create the bootstrap check-box</param>
        /// <param name="expression">The linq expression for the model property to use</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent BootstrapCheckBoxFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            return BootstrapCheckBoxFor(htmlHelper, expression, string.Empty, null, false);
        }

        /// <summary>
        /// Used to create a bootstrap check-box with a label
        /// </summary>
        /// <typeparam name="TModel">The Model of the view</typeparam>
        /// <param name="htmlHelper">The HtmlHelper used to create the bootstrap check-box</param>
        /// <param name="expression">The linq expression for the model property to use</param>
        /// <param name="labelText">The label text next to the check-box</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent BootstrapCheckBoxFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, string labelText = "")
        {
            return BootstrapCheckBoxFor(htmlHelper, expression, labelText, null, false);
        }

        /// <summary>
        /// Used to create a bootstrap check-box with a label
        /// </summary>
        /// <typeparam name="TModel">The Model of the view</typeparam>
        /// <param name="htmlHelper">The HtmlHelper used to create the bootstrap check-box</param>
        /// <param name="expression">The linq expression for the model property to use</param>
        /// <param name="labelText">The label text next to the check-box</param>
        /// <param name="isReadOnly">Makes the check-box read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent BootstrapCheckBoxFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, string labelText = "", bool isReadOnly = false)
        {
            return BootstrapCheckBoxFor(htmlHelper, expression, labelText, null, isReadOnly);
        }

        /// <summary>
        /// Used to create a check-box with a label and specific HTML attributes
        /// </summary>
        /// <typeparam name="TModel">The Model of the view</typeparam>
        /// <param name="htmlHelper">The HtmlHelper used to create the bootstrap check-box</param>
        /// <param name="expression">The linq expression for the model property to use</param>
        /// <param name="labelText">The label text next to the check-box</param>
        /// <param name="htmlAttributes">Extra HTML attributes programmer can pass in</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent BootstrapCheckBoxFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, string labelText = "", object htmlAttributes = null)
        {
            return BootstrapCheckBoxFor(htmlHelper, expression, labelText, htmlAttributes, false);
        }

        /// <summary>
        /// Used to create a check-box with a label, specific HTML attributes,
        /// and can be marked as read-only
        /// </summary>
        /// <typeparam name="TModel">The Model of the view</typeparam>
        /// <param name="htmlHelper">The HtmlHelper used to create the bootstrap check-box</param>
        /// <param name="expression">The linq expression for the model property to use</param>
        /// <param name="labelText">The label text next to the check-box</param>
        /// <param name="htmlAttributes">Extra HTML attributes programmer can pass in</param>
        /// <param name="isReadOnly">Makes the check-box read-only</param>
        /// <returns>MvcHtmlString</returns>
        public static IHtmlContent BootstrapCheckBoxFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, string labelText = "", object htmlAttributes = null, bool isReadOnly = false)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            return BootstrapCheckBoxFor(htmlHelper, expression, labelText, values, isReadOnly);
        }

        /// <summary>
        /// Private function used to create the actual bootstrap check-box
        /// </summary>
        /// <typeparam name="TModel">The Model of the view</typeparam>
        /// <param name="htmlHelper">The HtmlHelper used to create the bootstrap check-box</param>
        /// <param name="expression">The linq expression for the model property to use</param>
        /// <param name="labelText">The label text next to the check-box</param>
        /// <param name="htmlAttributes">Extra HTML attributes programmer can pass in</param>
        /// <param name="isReadOnly">Makes the check-box read-only</param>
        /// <returns>MvcHtmlString</returns>
        private static IHtmlContent BootstrapCheckBoxFor<TModel>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, string labelText, IDictionary<string, object> htmlAttributes, bool isReadOnly = false)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("class")) htmlAttributes.Add("class", string.Empty);
            if (isReadOnly) htmlAttributes.Add("disabled", "disabled");
            var checkBoxString = Utils.GetString(htmlHelper.CheckBoxFor<TModel>(expression));
            return new HtmlString(string.Format("<div class=\"checkbox {2}\"><label>{0} {1}</label></div>", checkBoxString, labelText, isReadOnly ? "disabled" : string.Empty));
        }

        #endregion Methods
    }
}