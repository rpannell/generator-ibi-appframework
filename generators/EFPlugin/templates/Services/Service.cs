using IBI.<%= projectname %>.Plugin.Models.Entities;
using IBI.<%= projectname %>.Plugin.Models;
using InterlineBrands.Platform.Core.Settings;
using System.Collections.Generic;

namespace IBI.<%= projectname %>.Plugin.Services
{
    public class <%= entityinfo.PropertyName %>Service : BaseService, Interfaces.I<%= entityinfo.PropertyName %>Service
    {
        private RestClient.<%= entityinfo.PropertyName %>RestClient serviceClient = null;

        public <%= entityinfo.PropertyName %>Service(IPluginSettings pluginSettings)
            : base(pluginSettings)
        {
            this.serviceClient = new RestClient.<%= entityinfo.PropertyName %>RestClient(this.URL, "api/<%= entityinfo.PropertyName %>/", this.UserName, this.UserName);
        }
		
		public <%= entityinfo.PropertyName %> Get(<%= entityinfo.PrimaryKey %> Id)
        {
            return this.serviceClient.Get(Id);
        }

		public void Delete(<%= entityinfo.PrimaryKey %> Id)
        {
            this.serviceClient.Delete(Id);
        }

		public <%= entityinfo.PropertyName %> Insert(<%= entityinfo.PropertyName %> entity)
        {
            return this.serviceClient.PostReturnId(entity);
        }

		public void Update(<%= entityinfo.PropertyName %> entity)
        {
            this.serviceClient.Put(entity.<%= entityinfo.PrimaryName %>, entity);
        }

		public PaginationResult<<%= entityinfo.PropertyName %>> GetPage(int limit, int offset, string search, string sort, string order)
        {
            return this.serviceClient.GetPage(limit, offset, search, sort, order);
        }

		public PaginationResult<<%= entityinfo.PropertyName %>> GetAdvancedPage(List<AdvancedSearch> advancedSearch, int limit, int offset, string search = null, string sort = null, string order = null, int type = 0)
        {
            return this.serviceClient.GetAdvancedPage(advancedSearch, limit, offset, search, sort, order, type);
        }
    }
}