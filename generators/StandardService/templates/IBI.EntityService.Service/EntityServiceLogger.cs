using IBI.Plugin.Utilities.Logging;
using IBI.Plugin.Utilities.Logging.Interfaces;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service
{
    /// <summary>
    /// Creates a Logger for the service by the name of the service
    /// </summary>
    public class <%= Name %>Logger<T>
    {
        #region Methods

        /// <summary>
        /// Gets <see cref="ILogAdapter"/> for the service
        /// </summary>
        /// <returns><see cref="ILogAdapter"/></returns>
        public static ILogAdapter GetLogger()
        {
            return ServiceLoggingManager.GetLogger<T>("<%= Name %>");
        }

        #endregion Methods
    }
}