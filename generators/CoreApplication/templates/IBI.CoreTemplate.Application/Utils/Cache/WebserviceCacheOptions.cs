using System;

namespace IBI.<%= Name %>.Application.Utils.Cache
{
    public class WebserviceCacheOptions
    {
        #region Properties

        public string InstanceName { get; set; }
        public TimeSpan? MemoryTimeSpan { get; set; }
        public string WebServiceURL { get; set; }

        #endregion Properties
    }
}