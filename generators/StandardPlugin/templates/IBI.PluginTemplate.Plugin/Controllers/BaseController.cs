using InterlineBrands.Platform.Core.Attributes;

namespace IBI.<%= Name %>.Plugin.Controllers
{
    [FrameworkAuthorize(<%= Name %>.PLUGINNAME, <%= Name %>.PLUGINROLES)]
    public class BaseController : IBI.Plugin.Utilities.Controllers.AuthorizePluginController
    {
    }
}