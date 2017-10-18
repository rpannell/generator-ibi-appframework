<% if(isPlugin) { %>using IBI.<%= projectname %>.Plugin.Models.Entities;<% } else { %>using IBI.<%= projectname %>.Application.Models.Entities;<%}%>
<% if(isPlugin) { %>using IBI.<%= projectname %>.Plugin.Models;<% } else { %>using IBI.<%= projectname %>.Application.Models;<%}%>
using System.Collections.Generic;

/// <summary>
/// Created by Entity Scaffolding on <%= TodaysDate %>
/// </summary>
<% if(isPlugin) { %>namespace IBI.<%= projectname %>.Plugin.Services.Interfaces<% } else { %>namespace IBI.<%= projectname %>.Application.Services.Interfaces<%}%>
{
    public interface I<%= entityinfo.PropertyName %>Service
    {
        PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int limit, int offset, string search, string sort, string order);

		void Delete(<%= entityinfo.PrimaryKey %> id);
		
		List<<%= entityinfo.PropertyName %>> Get();

		<%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> id);

		PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0);

		<%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> entity);

		void Update(<%= entityinfo.PropertyName %> entity);
		
		List<<%= entityinfo.PropertyName %>> GetAutocomplete(string term, int length = 20, int type = 0);

		/* GENIE HOOK */
        /* DO NOT DELETE THE ABOVE LINE */
    }
}