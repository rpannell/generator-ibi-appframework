using IBI.<%= projectname %>.Service.Entities;
using IBI.<%= projectname %>.Service.Repositories.Interfaces;
using IBI.<%= projectname %>.Service.Services.Interfaces;
using IBI.<%= projectname %>.Service.Core.Services;
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
///
///	DO NOT UPDATE THIS FILE, ANY CHANGES WILL BE OVERWRITTEN 
/// BY THE ENTITY SCAFFOLDING ON THE NEXT RUN, ADD ANY NECESSARY
/// CODE CHANGES SHOULD BE ADDED TO THE EXTENDED SERVICE
/// </summary>
namespace IBI.<%= projectname %>.Service.Services
{
    public partial class <%= entityinfo.PropertyName %>Service : BaseService<I<%= entityinfo.PropertyName %>Repository, <%= entityinfo.PropertyName %>, <%= entityinfo.PrimaryKey %>>, I<%= entityinfo.PropertyName %>Service
    {
		/* DO NOT UPDATE THIS FILE */
		
		#region Constructors
		
		public <%= entityinfo.PropertyName %>Service(I<%= entityinfo.PropertyName %>Repository repository)
        {
            this.Repository = repository;
        }
		
		#endregion Constructors
		
		/* DO NOT UPDATE THIS FILE */
    }
}