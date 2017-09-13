var <%= Name %>Module = (function (IBI) {
    IBI.CONST.<%= Name %>Area = "<%= Name %>";
    IBI.<%= Name %> = {
        CreateStaticContentUrl: function (contentpath) {
            /// <summary>
            /// Create a static content path for <%= Name %>
            /// </summary>
            /// <param name="contentpath" type="string"></param>
            /// <returns type=""></returns>
            return IBI.Utils.CreateStaticContentUrl(IBI.CONST.<%= Name %>Area, contentpath);
        },
        Create<%= Name %>Url: function (controller, action, parameters) {
            /// <summary>
            /// Create an URL for the <%= Name %> area for a controller,
            /// action and an array of parameters
            /// </summary>
            /// <param name="controller" type="string">The controller</param>
            /// <param name="action" type="string">The name of the action</param>
            /// <param name="parameters" type="array">Array of parameters</param>
            /// <returns type="string">The URL</returns>
            return IBI.Utils.CreateMVCUrl(IBI.CONST.<%= Name %>Area, controller, action, parameters);
        },
        CreateId<%= Name %>Url: function (controller, action, id) {
            /// <summary>
            /// Create an URL for the <%= Name %> that passes
            /// in a parameter by the name of id
            /// </summary>
            /// <param name="controller" type="string">The controller</param>
            /// <param name="action" type="string">The name of the action</param>
            /// <param name="id" type="int">The value to pass into the Id parameter</param>
            /// <returns type="string">The URL</returns>
            return IBI.Utils.CreateIdMVCUrl(IBI.CONST.<%= Name %>Area, controller, action, id);
        }
    };
})(IBI || {});