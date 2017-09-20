using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace IBI.<%= Name %>.Application.Utils.UI
{
    /// <summary>
    /// HTML Helpers for Bootstrap Drop Down List
    /// </summary>
    public static class BoostrapDropDownHelper
    {
        #region Private Functions

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static string GetEnumValue<TEnum>(TEnum value)
        {
            return value.ToString();
        }

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        #endregion Private Functions

        #region Bootstrap Dropdown List

        /// <summary>
        /// Creates a bootstrap dropdown list
        ///
        /// Note:   if the dropdown is readonly then it will also create a hidden field
        ///         with the value of the expression so that you can post this data using
        ///         a form with no issues
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">The Linq expression from the model</param>
        /// <param name="selectList">The Select List</param>
        /// <param name="emptyText">The string to display if the model doesn't use one of the items in the select list</param>
        /// <param name="htmlAttributes">The HTML Attributes</param>
        /// <param name="isReadonly">Marks the dropdown as disable if true</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapDropDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, object htmlAttributes = null, string emptyText = "", bool isReadonly = false)
        {
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            if (isReadonly) attributes.Add("disabled", "disabled");
            if (!attributes.ContainsKey("class")) attributes.Add("class", string.Empty);
            attributes["class"] += " form-control ";
            var dropDownString = htmlHelper.DropDownListFor(expression, selectList, emptyText, attributes);
            if (isReadonly)
            {
                var hiddenFor = htmlHelper.HiddenFor(expression);
                return new MvcHtmlString(hiddenFor.ToHtmlString() + dropDownString.ToHtmlString());
            }
            else
            {
                return dropDownString;
            }
        }

        public static MvcHtmlString BootstrapDropDownFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string emptyText = "", bool isReadonly = false)
        {
            return BootstrapDropDownFor(htmlHelper, expression, selectList, null, emptyText, isReadonly);
        }

        /// <summary>
        /// Creates a bootstrap dropdown based on enum
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">The Linq expression from the model</param>
        /// <param name="emptyText">The string to display if the model doesn't use one of the items in the select list</param>
        /// <param name="htmlAttributes">The HTML Attributes</param>
        /// <param name="isReadonly">Marks the dropdown as disable if true</param>
        /// <returns></returns>
        public static MvcHtmlString BootstrapEnumDropDownFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes, string emptyText = "", bool isReadonly = false)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();

            var items =
                values.Select(
                   value =>
                   new SelectListItem
                   {
                       Text = GetEnumDescription(value),
                       Value = value.ToString(),
                       Selected = value.Equals(metadata.Model)
                   });

            return BootstrapDropDownFor(htmlHelper, expression, items, htmlAttributes, emptyText, isReadonly);
        }

        /// <summary>
        /// Creates an Bootstrap dropdown box where the value is an integer and not the string value of the enum
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="emptyText"></param>
        /// <param name="isReadonly"></param>
        /// <returns></returns>
        public static MvcHtmlString BootstrapEnumIntDropDownFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes, string emptyText = "", bool isReadonly = false)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();
            var items =
                values.Select(
                   value =>
                   new SelectListItem
                   {
                       Text = GetEnumDescription(value),
                       Value = GetEnumValue(value),
                       Selected = value.Equals(metadata.Model)
                   });

            return BoostrapDropDownHelper.BootstrapDropDownFor(htmlHelper, expression, items, htmlAttributes, emptyText, isReadonly);
        }

        /// <summary>
        /// Creates a bootstrap dropdown with Yes/No as the options based on a boolean model value
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">The Linq expression from the model</param>
        /// <param name="emptyText">The string to display if the model doesn't use one of the items in the select list</param>
        /// <param name="htmlAttributes">The HTML Attributes</param>
        /// <param name="isReadonly">Marks the dropdown as disable if true</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapYesNoDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string emptyStringText = "", bool isReadonly = false)
        {
            var items = new List<SelectListItem>();
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            items.Add(new SelectListItem()
            {
                Text = "Yes",
                Value = "true",
                Selected = true.Equals(metadata.Model)
            });
            items.Add(new SelectListItem()
            {
                Text = "No",
                Value = "false",
                Selected = false.Equals(metadata.Model)
            });
            return BootstrapDropDownFor(htmlHelper, expression, items, htmlAttributes, emptyStringText, isReadonly);
        }

        /// <summary>
        /// Creates a bootstrap dropdown with Yes/No as the options based on a boolean model value
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">The Linq expression from the model</param>
        /// <param name="emptyText">The string to display if the model doesn't use one of the items in the select list</param>
        /// <param name="isReadonly">Marks the dropdown as disable if true</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapYesNoDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string emptyStringText = "", bool isReadonly = false)
        {
            return BootstrapYesNoDropDownListFor(htmlHelper, expression, null, emptyStringText, isReadonly);
        }

        #endregion Bootstrap Dropdown List
    }
}