using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Creates a Logger for the service by the name of the service
    /// </summary>
    public class <%= Name %>Logger<T>
    {
        public static ILogAdapter GetLogger()
        {
            return ServiceLoggingManager.GetLogger<T>("<%= Name %>");
        }
    }
}