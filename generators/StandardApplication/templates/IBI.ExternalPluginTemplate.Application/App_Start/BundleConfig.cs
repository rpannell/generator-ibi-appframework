using System.Web.Optimization;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application
{
    public class BundleConfig
    {
        #region Methods

        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Style Bundles

            bundles.Add(new StyleBundle("~/Content/SmartAdminProductionPlugin").Include("~/Content/smartadmin-production-plugins.css"));
            bundles.Add(new StyleBundle("~/Content/SmartAdminProduction").Include("~/Content/smartadmin-production.css"));
            bundles.Add(new StyleBundle("~/Content/SmartAdminSkins").Include("~/Content/smartadmin-skins.css"));
            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include("~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/Content/Site").Include("~/Content/Site.css"));
            bundles.Add(new StyleBundle("~/Content/FontAwesome").Include("~/Content/font-awesome.css"));
            bundles.Add(new StyleBundle("~/Content/BootstrapTable").Include("~/Content/bootstrap-table.css"));

            #endregion Style Bundles

            #region Script Bundles

            bundles.Add(new ScriptBundle("~/bundles/jQuery").Include("~/Scripts/jquery-3.1.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/jQueryUI").Include("~/Scripts/jquery-ui-1.12.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/jQueryVal").Include("~/Scripts/jquery.validate.js", "~/Scripts/jquery.validate.unobtrusive.js"));
            bundles.Add(new ScriptBundle("~/bundles/jQueryAjax").Include("~/Scripts/jquery.unobtrusive-ajax.js"));
            bundles.Add(new ScriptBundle("~/bundles/Modernizr").Include("~/Scripts/modernizr-2.8.3"));
            bundles.Add(new ScriptBundle("~/bundles/Bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/BootstrapTable").Include("~/Scripts/bootstrap-table.js"));
            bundles.Add(new ScriptBundle("~/bundles/IBI").Include("~/Scripts/IBI.js"));
            bundles.Add(new ScriptBundle("~/bundles/Moment").Include("~/Scripts/moment.js", "~/Scripts/moment-with-locales.js"));
            bundles.Add(new ScriptBundle("~/bundles/APP").Include("~/Scripts/app.config.seed.js", "~/Scripts/app.seed.js", "~/Scripts/SmartNotification.js"));

            #endregion Script Bundles
        }

        #endregion Methods
    }
}