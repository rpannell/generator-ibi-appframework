using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
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