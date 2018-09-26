using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    /// <summary>
    /// Basic Credentials Verifier
    /// </summary>
    public interface IBasicCredentialVerifier
    {
        #region Methods

        /// <summary>
        /// Verifies the credentials received via Basic Authentication
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<bool> Authenticate(string username, string password);

        #endregion Methods
    }
}