using System;

namespace IBI.<%= Name %>.Application.Utils
{
    public static class DateTimeFormatter
    {
        #region Methods

        public static string ConvertUtcToLocalDateTime(DateTime? dateTime)
        {
            return dateTime != null ? dateTime.Value.ToLocalTime().ToString("g") : string.Empty;
        }

        #endregion Methods
    }
}