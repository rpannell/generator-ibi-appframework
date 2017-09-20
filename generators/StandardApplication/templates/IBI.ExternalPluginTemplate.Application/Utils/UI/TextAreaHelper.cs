using System.Text;
using System.Web.Mvc;

namespace IBI.<%= Name %>.Application.Utils.UI
{
    public static class TextAreaHelper
    {
        #region Methods

        /// <summary>
        /// Text-area extension with maximum limit using legend
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns>MvcHtmlString</returns>
        public static MvcHtmlString TextAreaExtension(this HtmlHelper htmlHelper, string id, string name, int rows, int maxlimit, string classname, string placeholder, string value)
        {
            var placeHolder = " placeholder=" + "\"" + placeholder + "\"" + " id=" + "\"" + id + "\"" + " name=" + "\"" + name + "\"" + " class=" + "\"" + "form-control\"" + " onkeyup=" + "\"" + "textCounter(this, 'counter', 1000);" + "\"" + ">" + value + "</textarea>";
            var textareaCtrl = new StringBuilder();
            textareaCtrl.Append("<textarea rows=\"");
            textareaCtrl.Append(rows);
            textareaCtrl.Append("\"");
            textareaCtrl.Append(placeHolder);

            var inputCtrl = new StringBuilder();
            var max = maxlimit + 1;

            var inputplaceHolder = "<input disabled maxlength=" + "\"" + max + "\"" + " size=" + "\"" + "50" + "\"" + " value=" + "\"" + "Enter up to " + maxlimit + " characters" + "\"" + " id=" + "\"" + "counter" + "\"" + ">";
            inputCtrl.Append(inputplaceHolder);

            textareaCtrl.Append(inputCtrl);
            return new MvcHtmlString(textareaCtrl.ToString());
        }

        public static MvcHtmlString TextAreaExtensionScript(this HtmlHelper htmlHelper)
        {
            var ctrl = new StringBuilder();
            ctrl.Append("<script type=\"text/javascript\">");
            ctrl.Append("function textCounter(field, field2, maxlimit) {");
            ctrl.Append(" var countfield = document.getElementById(field2);");
            ctrl.Append("  if (field.value.length > maxlimit) {");
            ctrl.Append("field.value = field.value.substring(0, maxlimit);");
            ctrl.Append(" countfield.value = ");
            ctrl.Append("\"Enter up to 1000 characters ( 0 ");
            ctrl.Append("characters remaining)\";");
            ctrl.Append("return false;");
            ctrl.Append("} else {");
            ctrl.Append(" var remaining = maxlimit - parseInt(field.value.length);");
            ctrl.Append(" countfield.value = ");
            ctrl.Append("\"Enter up to 1000 characters ( \" +");
            ctrl.Append("remaining +  ");
            ctrl.Append("\" characters remaining)\";");
            ctrl.Append("}}</script>");
            return new MvcHtmlString(ctrl.ToString());
        }

        #endregion Methods
    }
}