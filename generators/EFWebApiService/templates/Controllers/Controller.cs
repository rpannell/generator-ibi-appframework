using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Controllers;
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding Version 1.17 on 8/24/2017 3:29 PM
/// </summary>
namespace IBI.<%= projectname %>.Service.Controllers
{
    public partial class <%= entityinfo.PropertyName %>Controller : BaseController<I<%= entityinfo.PropertyName %>Service, I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>
    {
    }
}