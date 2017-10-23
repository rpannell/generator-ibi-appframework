using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils.UI
{
    /// <summary>
    /// MVC Helper extensions for a bootstrap auto-complete text-box and corresponding
    /// javascript function for the text box
    /// </summary>
    public static class BootstrapAutocompleteHelper
    {
        #region Methods

        /// <summary>
        /// Create an auto-complete text-box and corresponding javascript text-box
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="textBoxId">The Id of the text-box</param>
        /// <param name="textBoxValue">The initial value of the text-box</param>
        /// <param name="hiddenFieldId">The Id of the hidden value</param>
        /// <param name="hiddenFieldValue">The initial value of the hidden field</param>
        /// <param name="dataUrl">The url used to get the auto-complete data</param>
        /// <param name="custom">Is this using a custom model</param>
        /// <param name="parentId">The Id of the parent element the auto-complete text-box is a child of</param>
        /// <param name="typeName">The radio-button selection for the type of auto-complete</param>
        /// <param name="selectJavascriptFunction">The name of the javascript function to run after a user has selects an search result</param>
        /// <returns></returns>
        public static MvcHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression,
                                                                        string textBoxId, string textBoxValue, string hiddenFieldId, string hiddenFieldValue,
                                                                        string dataUrl, bool custom = false, string parentId = "", string typeName = "", string selectJavascriptFunction = "")
        {
            //create the auto-complete text-box
            var autoCompleteTextBox = AutoCompleteFor(htmlHelper, expression, textBoxId, textBoxValue, hiddenFieldId, hiddenFieldValue);
            //create the auto-complete script
            var autoCompleteScript = AutoCompleteScript(htmlHelper, textBoxId, hiddenFieldId, dataUrl, custom, parentId, typeName, selectJavascriptFunction);
            //combine both the text-box and script
            return new MvcHtmlString(string.Format("{0} {1}", autoCompleteTextBox, autoCompleteScript));
        }

        /// <summary>
        /// Create an Auto-complete text-box for a specific property for a model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="textBoxId">The Id of the text-box</param>
        /// <param name="textBoxValue">The initial value of the text-box</param>
        /// <param name="hiddenFieldId">The Id of the hidden value</param>
        /// <param name="hiddenFieldValue">The initial value of the hidden field</param>
        /// <returns></returns>
        public static MvcHtmlString AutoCompleteFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string textBoxId, string textBoxValue, string hiddenFieldId, string hiddenFieldValue)
        {
            var values = new Dictionary<string, object>();
            values.Add("id", textBoxId);
            values.Add("value", textBoxValue);
            var textBox = BootstrapTextBoxHelper.BootstrapTextBoxFor(htmlHelper, expression, values).ToHtmlString();
            var hidden = string.Format("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" />", hiddenFieldId, hiddenFieldValue);
            return new MvcHtmlString(string.Format("{0}{1}", textBox, hidden));
        }

        /// <summary>
        /// Create the text-box and hidden field that is also has validation hooked up
        /// </summary>
        /// <typeparam name="TModel">The model</typeparam>
        /// <typeparam name="TProperty">The property to display in the text-box</typeparam>
        /// <typeparam name="TKey">The id in the hidden field</typeparam>
        /// <param name="htmlHelper">The helper</param>
        /// <param name="nameExpression">The expression for the string in the text-box</param>
        /// <param name="hiddenFor">The expression for the hidden field id</param>
        /// <returns></returns>
        public static MvcHtmlString AutoCompleteFor<TModel, TProperty, TKey>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> nameExpression, Expression<Func<TModel, TKey>> hiddenFor)
        {
            var textbox = BootstrapTextBoxHelper.BootstrapTextBoxFor(htmlHelper, nameExpression).ToHtmlString();
            var hiddenField = HtmlHiddenHelperFor.CleanHiddenFor(htmlHelper, hiddenFor).ToHtmlString();

            return new MvcHtmlString(string.Format("{0} {1}", textbox, hiddenField));
        }

        /// <summary>
        /// Create the javascript for an auto-complete text-box
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="textBoxId">The id of the text-box that this auto-complete is for</param>
        /// <param name="hiddenFieldId">The id of the hidden field that this auto-complete is for</param>
        /// <param name="dataUrl">The url used to get the auto-complete data</param>
        /// <param name="custom">Is this using a custom model</param>
        /// <param name="parentId">The Id of the parent element the auto-complete text-box is a child of</param>
        /// <param name="typeName">The radio-button selection for the type of auto-complete</param>
        /// <param name="selectJavascriptFunction">The name of the javascript function to run after a user has selects an search result</param>
        /// <returns></returns>
        public static MvcHtmlString AutoCompleteScript(this HtmlHelper helper, string textBoxId, string hiddenFieldId, string dataUrl, bool custom = false, string parentId = "", string typeName = "", string selectJavascriptFunction = "", int miscId = 0)
        {
            var ctrl = new StringBuilder();
            ctrl.Append("<script type=\"text/javascript\">");
            ctrl.Append("$(document).ready(function () {");

            ctrl.Append(" $(function () {");
            ctrl.Append("   var cache = {};");
            ctrl.Append(string.Format(" $(\"#{0}\").autocomplete({{", textBoxId));
            ctrl.Append("minLength: 2,");
            ctrl.Append("select: function (event, ui) {");

            ctrl.Append(string.Format("$(\"#{0}\").val(ui.item.id);", hiddenFieldId));

            if (!string.IsNullOrWhiteSpace(selectJavascriptFunction))
            {
                ctrl.Append(string.Format("{0}(event, ui);", selectJavascriptFunction));
            }

            ctrl.Append(" return true;},");

            ctrl.Append(" source: function (request, response) {");
            ctrl.Append("      var term = encodeURIComponent(request.term);");
            ctrl.Append("      if (term in cache) {");
            ctrl.Append("          response(cache[term]);");
            ctrl.Append("          return;");
            ctrl.Append("      }");
            ctrl.Append(string.Format("$(\"#{0}\").val('');", hiddenFieldId));

            if (!string.IsNullOrWhiteSpace(typeName))
            {
                ctrl.Append(string.Format("var searchType = IBI.UI.GetRadioSelectedValue(\"{0}\");", typeName));
                ctrl.Append(string.Format("      $.getJSON(\"{0}?term=\" + term + \"&searchType=\" + searchType, request, function (data, status, xhr) {{", dataUrl));
            }
            else if (miscId != 0)
            {
                ctrl.Append(string.Format("      $.getJSON(\"{0}?term=\" + term + \"&id={1}\", request, function (data, status, xhr) {{", dataUrl, miscId));
            }
            else
            {
                ctrl.Append(string.Format("      $.getJSON(\"{0}?term=\" + term, request, function (data, status, xhr) {{", dataUrl));
            }

            //ctrl.Append("          console.log(data);");
            ctrl.Append("          cache[term] = data._options;");
            ctrl.Append("          response(data._options);");
            ctrl.Append("      });");
            ctrl.Append("  }");
            ctrl.Append(" })");

            if (custom == true)
            {
                ctrl.Append(".data( 'ui-autocomplete' )._renderItem = ");
                ctrl.Append(" function( ul, item ) {");
                ctrl.Append("  return $( '<li>' )");
                ctrl.Append(".data( 'ui-autocomplete-item', item )");
                ctrl.Append(".append( '<hr/><a>' + item.label + ");
                ctrl.Append(" (item.description != null && item.description != '' ? '<br/>' + item.description : '') + ");
                ctrl.Append(" (item.line1 != null && item.line1 != '' ? '<br/>' + item.line1 : '') + ");
                ctrl.Append(" (item.line2 != null && item.line2 != '' ? '<br/>' + item.line2 : '') + ");
                ctrl.Append(" (item.line3 != null && item.line3 != '' ? '<br/>' + item.line3 : '') + ");
                ctrl.Append(" (item.line4 != null && item.line4 != '' ? '<br/>' + item.line4 : '') + '</a>')");
                ctrl.Append(".appendTo( ul );");
                ctrl.Append("  };");
            }

            //if you use this on a modal you must set the parent element id to tell where to append this data
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                ctrl.Append(string.Format(" $(\"#{0}\").autocomplete(\"option\", \"appendTo\", \"#{1}\");", textBoxId, parentId));
            }

            ctrl.Append(" });");
            ctrl.Append("  });");
            ctrl.Append(" </script>");

            return new MvcHtmlString(ctrl.ToString());
        }

        #endregion Methods
    }
}