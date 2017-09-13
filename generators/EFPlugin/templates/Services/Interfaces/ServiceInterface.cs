using IBI.<%= projectname %>.Plugin.Models.Entities;
using IBI.<%= projectname %>.Plugin.Models;
using System.Collections.Generic;

namespace IBI.<%= projectname %>.Plugin.Services.Interfaces
{
    public interface I<%= entityinfo.PropertyName %>Service
    {
        PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int limit, int offset, string search, string sort, string order);

		void Delete(<%= entityinfo.PrimaryKey %> id);

		<%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> id);

		PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0);

		<%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> entity);

		void Update(<%= entityinfo.PropertyName %> entity);
    }
}