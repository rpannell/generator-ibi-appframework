using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin
{
    /// <summary>
    /// Creates a logger by the name of the plugin
    /// </summary>
    public class <%= Name %>Logger<T>
    {
        public static ILogAdapter GetLogger()
        {
            return PluginLoggingManager.GetLogger<T>("<%= Name %>");
        }
    }
}