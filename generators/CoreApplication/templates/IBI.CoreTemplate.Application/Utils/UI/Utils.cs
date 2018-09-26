using Microsoft.AspNetCore.Html;
using System.Text.Encodings.Web;

namespace IBI.<%= Name %>.Application.Utils.UI
{
    public class Utils
    {
        #region Methods

        public static string GetString(IHtmlContent content)
        {
            var writer = new System.IO.StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }

        #endregion Methods
    }
}