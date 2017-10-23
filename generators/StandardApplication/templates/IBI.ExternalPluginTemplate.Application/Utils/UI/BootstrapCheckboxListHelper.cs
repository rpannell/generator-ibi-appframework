using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils.UI
{
    public static class BootstrapCheckboxListHelper
    {
        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="listNameExpr"></param>
        /// <param name="sourceDataExpr"></param>
        /// <param name="valueExpr"></param>
        /// <param name="textToDisplayExpr"></param>
        /// <param name="selectedValuesExpr"></param>
        /// <param name="cssClassName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString BootstrapCheckboxListFor<TModel, TProperty, TItem, TValue, TKey>(this HtmlHelper<TModel> htmlHelper,
                                                                                                    Expression<Func<TModel, TProperty>> listNameExpr,
                                                                                                    Expression<Func<TModel, IEnumerable<TItem>>> sourceDataExpr,
                                                                                                    Expression<Func<TItem, TValue>> valueExpr,
                                                                                                    Expression<Func<TItem, TKey>> textToDisplayExpr,
                                                                                                    Expression<Func<TModel, IEnumerable<TItem>>> selectedValuesExpr,
                                                                                                    string cssClassName,
                                                                                                    object htmlAttributes = null)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            var htmlString = new StringBuilder();
            var modelMetadata = ModelMetadata.FromLambdaExpression(listNameExpr, htmlHelper.ViewData);
            var model = htmlHelper.ViewData.Model;
            var valueFunction = valueExpr.Compile();
            var textFunc = textToDisplayExpr.Compile();
            var nameOfCheckboxes = ExpressionHelper.GetExpressionText(listNameExpr);

            //get the list of selected values
            var selectedItems = new List<TItem>();

            if (selectedValuesExpr != null)
            {
                var selectedItems_temp = selectedValuesExpr.Compile()(model);

                if (selectedItems_temp != null) selectedItems = selectedItems_temp.ToList();
            }

            var selectedValues = selectedItems.Select(s => valueFunction(s).ToString()).ToList();

            //walk each possibly selected value
            foreach (var item in sourceDataExpr.Compile()(model).ToList())
            {
                var thisValue = valueFunction(item).ToString();
                var thisLable = textFunc(item).ToString();
                var isSelected = selectedValues.Any(value => value == thisValue);

                htmlString.Append(CreateCheckbox<TModel>(htmlHelper, isSelected, nameOfCheckboxes, thisValue, thisLable, cssClassName, values));
            }

            return MvcHtmlString.Create(htmlString.ToString());
        }

        /// <summary>
        /// Create the checkbox HTML for a specific checkbox in the list
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="isChecked"></param>
        /// <param name="checkboxName"></param>
        /// <param name="checkboxValue"></param>
        /// <param name="checkboxLabel"></param>
        /// <param name="cssClass"></param>
        /// <returns></returns>
        private static string CreateCheckbox<TModel>(this HtmlHelper<TModel> htmlHelper, bool isChecked, string checkboxName, string checkboxValue, string checkboxLabel, string cssClass, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("class")) htmlAttributes.Add("class", string.Empty);
            if (!htmlAttributes.ContainsKey("value")) htmlAttributes.Add("value", checkboxValue);
            //var checkBoxString = htmlHelper.CheckBox(checkboxName, isChecked, htmlAttributes);
            var checkBoxString = string.Format("<input type=\"checkbox\" name=\"{0}\" value=\"{1}\" class=\"{2}\" />", checkboxName, checkboxValue, htmlAttributes["class"].ToString());
            return string.Format("<div class=\"checkbox {2}\"><label>{0} {1}</label></div>", checkBoxString, checkboxLabel, cssClass);
        }

        #endregion Methods
    }
}