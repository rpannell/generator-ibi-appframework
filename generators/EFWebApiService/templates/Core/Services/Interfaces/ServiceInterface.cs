using IBI.<%= projectname %>.Service.Core.Services.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;

namespace IBI.<%= projectname %>.Service.Services.Interfaces
{
    /// <summary>
    /// Service interface for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
    public partial interface I<%= entityinfo.PropertyName %>Service : IBaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}