using System;
using System.Runtime.Serialization;

namespace IBI.PlasticCardTesting.Plugin.Utils
{
    [Serializable]
    public class APIException : Exception
    {
        #region Constructors

        public APIException(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                this.ExceptionMessageInfo = info.GetString("ExceptionMessage");
                this.ErrorMessage = info.GetString("Message");
                this.StackTrackInfo = info.GetString("StackTrace");
                this.ExceptionTypeInfo = info.GetString("ExceptionType");
            }
        }

        public string ErrorMessage { get; set; }
        public string ExceptionMessageInfo { get; set; }
        public string ExceptionTypeInfo { get; set; }
        public string InnerExceptionInfo { get; set; }
        public override string Message => this.ExceptionMessageInfo;
        public override string StackTrace => this.StackTrackInfo;

        public string StackTrackInfo { get; set; }

        #endregion Constructors
    }
}