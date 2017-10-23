using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils.UI
{
    public static class BootstrapRadioButtonListHelper
    {
        #region Methods

        /// <summary>
        /// Create a bootstrap radio button list
        /// </summary>
        /// <typeparam name="TModel">The Model</typeparam>
        /// <typeparam name="TProperty">The type of the Model property used for this radio list</typeparam>
        /// <param name="htmlHelper">The HTML helper used to create the radio button list</param>
        /// <param name="expression">The linq expression to the Model's property</param>
        /// <param name="selectList">The list of items</param>
        /// <param name="htmlAttributes">Extra attributes the programmer can add or override</param>
        /// <param name="isReadOnly">Marks the list as readonly</param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString BootstrapRadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IDictionary<string, object> htmlAttributes = null, bool isReadOnly = false)
        {
            if (htmlAttributes == null) htmlAttributes = new Dictionary<string, object>();
            if (!htmlAttributes.ContainsKey("class")) htmlAttributes.Add("class", string.Empty);
            if (isReadOnly) htmlAttributes.Add("disabled", "disabled");
            var radioListString = string.Empty;
            foreach (var item in selectList)
            {
                var radioButton = htmlHelper.RadioButtonFor<TModel, TProperty>(expression, item.Value, htmlAttributes).ToHtmlString();
                var label = string.Format("<div class=\"radio\"><label>{0} {1}</label></div>", radioButton, item.Text);
                radioListString += label;
            }
            return new MvcHtmlString(radioListString);
        }

        #endregion Methods
    }
}