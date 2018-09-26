using IBI.<%= projectname %>.Service.Core.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Entities;

namespace IBI.<%= projectname %>.Service.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for <see cref="<%= entityinfo.PropertyName %>"/>
    /// </summary>
    public partial interface I<%= entityinfo.PropertyName %>Repository : IBaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
        /* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}