using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Utils.UI
{
    /// <summary>
    /// MVC Helper extentions for a bootstrap typeahead inputtags textbox and corresponding
    /// javascript function for the text box
    /// </summary>
    ///
    public static class BootstrapTypeaheadHelper
    {
        #region Methods

        /// <summary>
        /// Create a typeahead inputtags textbox and corresponding javascript textbox
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="textBoxId">The Id of the textbox</param>
        /// <param name="htmlAttributes">The textbox attributes</param>
        /// <returns></returns>
        ///
        public static MvcHtmlString TypeAhead(this HtmlHelper htmlHelper, string textBoxId, object htmlAttributes = null)
        {
            var values = new RouteValueDictionary(htmlAttributes);
            values.Add("id", textBoxId);
            var textBox = BootstrapTextBoxHelper.BootstrapTextBox(htmlHelper, textBoxId, "", values).ToHtmlString();
            return new MvcHtmlString(string.Format("{0}", textBox));
        }

        /// <summary>
        /// Create the javascript for an autocomplete textbox
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="textBoxId">The id of the textbox that this autocomplete is for</param>
        /// <param name="dataUrl">The url used to get the autocomplete data</param>
        /// <param name="typeAheadLabel">The json data property name needs to be setup with typeahead label</param>
        /// <param name="typeAheadValue">The json data property name needs to be setup with typeahead value</param>
        /// <returns></returns>
        public static MvcHtmlString TypeAheadScript(this HtmlHelper helper, string textBoxId, string dataUrl, string typeAheadValue, string typeAheadLabel)
        {
            string ttadapter = "tagEngine.ttAdapter()";

            var ctrl = new StringBuilder();
            ctrl.Append("<script type=\"text/javascript\">");
            ctrl.Append("$(document).ready(function () {");

            //ctrl.Append(" $(function () {");

            ctrl.Append("   var token = [];");

            //ctrl.Append(string.Format(" $.get(\"{0}\",", dataUrl));
            ctrl.Append(string.Format(" $.getJSON(\"{0}\",", dataUrl));
            ctrl.Append("function (response) {");
            ctrl.Append(string.Format(" $.each(response,"));
            ctrl.Append(" function (i,v) {");
            ctrl.Append("token.push({ value: v");
            ctrl.Append(string.Format(".{0},", typeAheadValue));
            ctrl.Append("label: v");
            ctrl.Append(string.Format(".{0}", typeAheadLabel));
            ctrl.Append(" });});");

            ctrl.Append("var tagEngine = new Bloodhound({");
            ctrl.Append("local: token,");
            ctrl.Append("datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.label);},");

            ctrl.Append("queryTokenizer: Bloodhound.tokenizers.whitespace");
            ctrl.Append(" });");

            ctrl.Append("tagEngine.initialize();");

            ctrl.Append(string.Format("$(\'#{0}\')", textBoxId));
            ctrl.Append(".tokenfield({");

            ctrl.Append("typeahead: [null,{");

            ctrl.Append(string.Format("{0}:\'{1}\',", "name", "tagEngine"));

            ctrl.Append(string.Format("{0}:\'{1}\',", "displayKey", "label"));

            ctrl.Append(string.Format("{0}:{1}", "source", ttadapter));
            ctrl.Append("}]}).on(");

            ctrl.Append(string.Format("\'{0}:{1}\',", "tokenfield", "createtoken"));
            ctrl.Append("function(event){");
            ctrl.Append(" var existingTokens = $(this).tokenfield(");
            ctrl.Append(string.Format("\'{0}\');", "getTokens"));

            ctrl.Append(" $.each(existingTokens, function (index, token) {");
            ctrl.Append("if (token.value === event.attrs.value) {");
            ctrl.Append("event.preventDefault();");

            ctrl.Append(" }");
            ctrl.Append(" });");
            ctrl.Append(" });");
            ctrl.Append(" });");
            ctrl.Append(" });");

            ctrl.Append(" </script>");

            return new MvcHtmlString(ctrl.ToString());
        }

        #endregion Methods
    }
}