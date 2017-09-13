using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

namespace IBI.<%= Name %>.Plugin
{
    public class <%= Name %>Logger<T>
    {
        public static ILogAdapter GetLogger()
        {
            return PluginLoggingManager.GetLogger<T>("<%= Name %>");
        }
    }
}