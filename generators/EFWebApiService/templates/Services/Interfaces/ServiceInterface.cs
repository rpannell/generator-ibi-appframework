using IBI.<%= projectname %>.Service.Core.Services.Interfaces;
using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
/// </summary>
namespace IBI.<%= projectname %>.Service.Services.Interfaces
{
    public partial interface I<%= entityinfo.PropertyName %>Service : IBaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
    }
}