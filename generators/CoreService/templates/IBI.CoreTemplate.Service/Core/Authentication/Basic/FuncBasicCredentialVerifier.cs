using System;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Core.Authentication.Basic
{
    internal class FuncBasicCredentialVerifier : IBasicCredentialVerifier
    {
        #region Fields

        private readonly Func<(string username, string password), Task<bool>> _func;

        #endregion Fields

        #region Constructors

        public FuncBasicCredentialVerifier(Func<(string username, string password), Task<bool>> func)
        {
            _func = func ?? throw new ArgumentNullException(nameof(func));
        }

        #endregion Constructors

        #region Methods

        public Task<bool> Authenticate(string username, string password)
        {
            return _func((username, password));
        }

        #endregion Methods
    }
}