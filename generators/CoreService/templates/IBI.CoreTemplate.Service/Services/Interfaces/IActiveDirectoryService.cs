using IBI.<%= Name %>.Service.Core.Services.Interfaces;
using IBI.<%= Name %>.Service.Entities;
using IBI.<%= Name %>.Service.Repositories.Interfaces;

namespace IBI.<%= Name %>.Service.Services.Interfaces
{
    /// <summary>
    /// Service interface for <see cref="ActiveDirectory"/>
    /// </summary>
    public partial interface IActiveDirectoryService : IBaseService<IActiveDirectoryRepository, ActiveDirectory, int>
    {
        #region Methods

        /// <summary>
        /// Get the active directory of the currently logged in user
        /// </summary>
        /// <returns><see cref="ActiveDirectory"/></returns>
        ActiveDirectory GetCurrentUser();

        /// <summary>
        /// Get ActiveDirectory by the username
        /// </summary>
        /// <param name="userName">The user name</param>
        /// <returns><see cref="ActiveDirectory"/></returns>
        ActiveDirectory GetUserByUserName(string userName);

        #endregion Methods
                
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}