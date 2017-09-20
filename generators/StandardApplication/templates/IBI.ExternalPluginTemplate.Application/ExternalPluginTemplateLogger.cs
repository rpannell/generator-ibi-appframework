using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

namespace IBI.<%= Name %>.Application
{
    public class <%= Name %>Logger<T>
    {
        #region Methods

        public static ILogAdapter GetLogger()
        {
            return PluginLoggingManager.GetLogger<T>("<%= Name %>");
        }

        #endregion Methods
    }
}