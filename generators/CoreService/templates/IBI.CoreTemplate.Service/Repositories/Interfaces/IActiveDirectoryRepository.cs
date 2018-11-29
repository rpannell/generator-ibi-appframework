using IBI.<%= Name %>.Service.Core.Repositories.Interfaces;
using IBI.<%= Name %>.Service.Entities;
using System.Threading.Tasks;

namespace IBI.<%= Name %>.Service.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for <see cref="ActiveDirectory"/>
    /// </summary>
    public partial interface IActiveDirectoryRepository : IBaseRepository<ActiveDirectory, int>
    {
        #region Methods

        /// <summary>
        /// Get the active directory of the currently logged in user
        /// </summary>
        /// <returns><see cref="ActiveDirectory"/></returns>
        Task<ActiveDirectory> GetCurrentUser();

        /// <summary>
        /// Get ActiveDirectory by the username
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        Task<ActiveDirectory> GetUserByUserName(string userName);

        #endregion Methods

        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}