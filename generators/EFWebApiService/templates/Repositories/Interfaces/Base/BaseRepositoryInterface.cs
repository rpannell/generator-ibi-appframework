using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Core.Repositories.Interfaces;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
/// </summary>
namespace IBI.<%= projectname %>.Service.Repositories.Interfaces
{
    public partial interface I<%= entityinfo.PropertyName %>Repository : IBaseRepository<<%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
    }
}