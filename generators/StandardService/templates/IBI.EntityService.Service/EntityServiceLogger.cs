using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

namespace IBI.<%= Name %>.Service
{
    public class <%= Name %>Logger<T>
    {
        public static ILogAdapter GetLogger()
        {
            return ServiceLoggingManager.GetLogger<T>("<%= Name %>");
        }
    }
}