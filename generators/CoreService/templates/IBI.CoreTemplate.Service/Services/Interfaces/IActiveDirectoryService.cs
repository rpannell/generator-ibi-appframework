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
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}