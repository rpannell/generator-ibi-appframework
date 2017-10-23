using InterlineBrands.Platform.Core;
using InterlineBrands.Platform.Core.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin
{
    [Serializable()]
    public class <%= Name %> : InterlinePlugIn
    {
        /// <summary>
        /// The path to the plugin in layout that can be used in this plugins
        /// views if the developer so chooses.
        ///
        /// Note:  since the layout is compiled into the dll using RazorGenerator
        /// the path must include the path through the areas for <%= Name %>
        /// </summary>
        public const string LAYOUTPATH = "~/Areas/<%= Name %>/Views/Shared/_Layout.cshtml";

        /// <summary>
        /// Consts that can be used for authorization and other
        /// locations where knowing the list of roles and the
        /// name of the plugin would be useful
        /// </summary>
        public const string PLUGINNAME = "<%= Name %>";

        /// <summary>
        /// Roles should be comma delimited if you would like
        /// more than 1 role for the plugin
        /// </summary>
        public const string PLUGINROLES = "<%= Name %>";

        /// <summary>
        /// This is the name of the plugin and shouldn’t include any spaces
        /// or special characters
        /// </summary>
        public override string Name
        {
            get { return PLUGINNAME; }
        }

        /// <summary>
        /// Used to create the category
        /// this plugin should be a part of
        /// Once the plugin is installed into the
        /// database this value can be changed in the
        /// UI
        /// </summary>
        public override string Category
        {
            get
            {
                return base.Category;
            }
        }

        /// <summary>
        /// This is the name that is displayed on the UI in the home page.
        /// Should be short and can have spaces and special characters.
        /// </summary>
        public override string DisplayName
        {
            get { return PLUGINNAME; }
        }

        /// <summary>
        /// This is the area prefix you wish to give this plugin in the application
        /// framework.  This should be the same as the name above but can be
        /// changed if needed, but should be avoided.
        /// </summary>
        public override string AreaRoutePrefix
        {
            get { return PLUGINNAME; }
        }

        /// <summary>
        /// This is the string of the description of the plugin that shown on
        /// home page and inserted into the database when the plugin is
        /// initially installed
        /// </summary>
        public override string Description
        {
            get { return "Description for <%= Name %>"; }
        }

        /// <summary>
        /// This is a list of strings of the various roles that are used with in
        /// the plugin.  At least 1 role is required to setup permissions to the
        /// plugin.
        /// </summary>
        public override IEnumerable<string> Roles
        {
            get
            {
                return PLUGINROLES.Split(',').Select(x => x.Trim());
            }
        }

        /// <summary>
        /// This gives the css class of the FontAwesome icon that is
        /// shown on the home page.
        /// </summary>
        public override string Icon
        {
            get
            {
                return "fa fa-home";
            }
        }

        /// <summary>
        /// This creates the menu on the left hand side of the application when
        /// a user goes to the plugin based on their roles.  Please note that
        /// more than 1 role can be associated to a menu item.
        /// </summary>
        public override List<MenuOption> Menu
        {
            get
            {
                var _menu = new List<MenuOption>();
                _menu.Add(new MenuOption()
                {
                    name = "Home",
                    url = string.Format("/{0}/", this.AreaRoutePrefix),
                    controller = "<%= Name %>",
                    action = "Index",
                    description = string.Empty,
                    icon = "fa fa-lg fa-fw fa-home",
                    roles = new List<string> { "<%= Name %>" }
                });
                return _menu;
            }
        }

        /// <summary>
        /// Registers the area and controllers into the application framework
        /// </summary>
        /// <param name="routes"></param>
        public override void RegisterPlugin(RouteCollection routes)
        {
            var context = new AreaRegistrationContext(this.AreaRoutePrefix, routes, null);

            context.MapRoute(
                this.Name + "_default",
                this.AreaRoutePrefix + "/{controller}/{action}/{id}",
                new { controller = "<%= Name %>", action = "Index", id = UrlParameter.Optional },
                new[] { "IBI.<%= Name %>.Plugin.Controllers" }
                );
        }

        /// <summary>
        /// Used to call static functions when the plugin is initially loaded into
        /// the framework
        /// </summary>
        /// <param name="config"></param>
        public override void Configure(System.Web.Http.HttpConfiguration config)
        {
            base.Configure(config);

            ConfigSettings.AutoMapperConfiguration.Configure();
        }

        /// <summary>
        /// Used to setup the anonymous role if the
        /// plugin chooses to have one
        /// </summary>
        public override string AnonymousRole
        {
            get
            {
                return base.AnonymousRole;
            }
        }

        /// <summary>
        /// Marks the plugin to be shown in the home screen
        /// or not
        /// </summary>
        public override bool ShowInHome
        {
            get
            {
                return base.ShowInHome;
            }
        }
    }
}