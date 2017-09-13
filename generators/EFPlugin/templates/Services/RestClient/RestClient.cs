using IBI.<%= projectname %>.Plugin.Models.Entities;

namespace IBI.<%= projectname %>.Plugin.Services.RestClient
{
    public class <%= entityinfo.PropertyName %>RestClient : Base<<%= entityinfo.PropertyName %>>
    {
        public <%= entityinfo.PropertyName %>RestClient(string url, string resource, string username, string role)
            : base(url, resource, username, role)
        {
        }
    }
}