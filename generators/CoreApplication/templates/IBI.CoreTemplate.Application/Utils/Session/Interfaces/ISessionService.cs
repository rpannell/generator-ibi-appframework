using System.Threading;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Application.Utils.Session.Interfaces
{
    public interface ISessionService
    {
        #region Methods

        Task<byte[]> Get(string key, CancellationToken token = default(CancellationToken));

        Task Refresh(string key, CancellationToken token = default(CancellationToken));

        Task Set(string key, byte[] data, CancellationToken token = default(CancellationToken));

        #endregion Methods
    }
}