using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using System;
using System.Linq.Expressions;

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
        public static IHtmlContent CleanHiddenFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            if (metadata.Model == null || (metadata.ModelType == typeof(string) && metadata.Model.ToString() == string.Empty))
            {
                return htmlHelper.HiddenFor(expression, new { @Value = "" });
            }

            return htmlHelper.HiddenFor(expression);
        }

        #endregion Methods
    }
}