using System;
using System.Collections.Generic;
using System.Linq;

namespace IBI.<%= Name %>.Application
{
    [Serializable()]
    public class <%= Name %>
    {
        #region Fields

        /// <summary>
        /// Consts that can be used for authorization and other
        /// locations where knowing the list of roles and the
        /// name of the plugin would be useful
        /// </summary>
        public const string APPLICATIONNAME = "<%= Name %>";

        /// <summary>
        /// Roles should be comma delimited if you would like
        /// more than 1 role for the plugin
        /// </summary>
        public const string APPLICATIONROLES = "<%= Name %>Role";

        /// <summary>
        /// The path to the plugin in layout that can be used in this plugins
        /// views if the developer so chooses.
        ///
        /// Note:  since the layout is compiled into the dll using RazorGenerator
        /// the path must include the path through the areas for <%= Name %>
        /// </summary>
        public const string LAYOUTPATH = "~/Areas/<%= Name %>/Views/Shared/_Layout.cshtml";

        #endregion Fields

        #region Properties

        /// <summary>
        /// This is the area prefix you wish to give this plugin in the application
        /// framework.  This should be the same as the name above but can be
        /// changed if needed, but should be avoided.
        /// </summary>
        public string AreaRoutePrefix
        {
            get { return APPLICATIONNAME; }
        }

        /// <summary>
        /// Used to create the category
        /// this plugin should be a part of
        /// Once the plugin is installed into the
        /// database this value can be changed in the
        /// UI
        /// </summary>

        /// <summary>
        /// This is the string of the description of the plugin that shown on
        /// home page and inserted into the database when the plugin is
        /// initially installed
        /// </summary>
        public string Description
        {
            get { return "<%= Name %>"; }
        }

        /// <summary>
        /// This is the name that is displayed on the UI in the home page.
        /// Should be short and can have spaces and special characters.
        /// </summary>
        public string DisplayName
        {
            get { return "<%= Name %>"; }
        }

        /// <summary>
        /// This gives the css class of the FontAwesome icon that is
        /// shown on the home page.
        /// </summary>
        public string Icon
        {
            get
            {
                return "fa fa-pencil-square-o";
            }
        }

        /// <summary>
        /// This is the name of the plugin and shouldn’t include any spaces
        /// or special characters
        /// </summary>
        public string Name
        {
            get { return APPLICATIONNAME; }
        }

        /// <summary>
        /// This is a list of strings of the various roles that are used with in
        /// the plugin.  At least 1 role is required to setup permissions to the
        /// plugin.
        /// </summary>
        public IEnumerable<string> Roles
        {
            get
            {
                return APPLICATIONROLES.Split(',').Select(x => x.Trim());
            }
        }

        #endregion Properties
    }
}