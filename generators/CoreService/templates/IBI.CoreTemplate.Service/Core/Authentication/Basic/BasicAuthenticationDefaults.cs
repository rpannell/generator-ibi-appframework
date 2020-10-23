namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    /// <summary>
    /// Default values used by Basic Access Authentication scheme.
    /// </summary>
    public class BasicAuthenticationDefaults
    {
        #region Fields

        /// <summary>
        /// Basic authentication scheme
        /// </summary>
        public const string AuthenticationScheme = "Basic";

        /// <summary>
        /// Default authentication display name
        /// </summary>
        public static readonly string DisplayName = "Basic";

        #endregion Fields
    }
}