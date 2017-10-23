using InterlineBrands.Platform.Core.Attributes;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin.Controllers
{
    [FrameworkAuthorize(<%= Name %>.PLUGINNAME, <%= Name %>.PLUGINROLES)]
    public class BaseController : IBI.Plugin.Utilities.Controllers.AuthorizePluginController
    {
    }
}