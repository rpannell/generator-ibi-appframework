using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils.UI
{
    public static class HtmlHiddenHelperFor
    {
        #region Methods

        /// <summary>
        /// A HiddenFor helper that will check if the value of the property is null
        /// and if it's null then set the value of the hidden to empty string else set
        /// it as a normal HiddenFor
        ///
        /// will also check for a string property that is set to string.empty and will
        /// set the value as "" (this is different in MVC 5.2.3)
        /// </summary>
        /// <typeparam name="TModel">The view model</typeparam>
        /// <typeparam name="TProperty">The property of the view model for the hidden value</typeparam>
        /// <param name="htmlHelper">The MVC helper</param>
        /// <param name="expression">The lambda expression to get to the property</param>
        /// <returns>MvcHtmlString for the view</returns>
        public static MvcHtmlString CleanHiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            if (metadata.Model == null || (metadata.ModelType == typeof(string) && metadata.Model.ToString() == string.Empty))
            {
                return htmlHelper.HiddenFor(expression, new { @Value = "" });
            }

            return htmlHelper.HiddenFor(expression);
        }

        #endregion Methods
    }
}