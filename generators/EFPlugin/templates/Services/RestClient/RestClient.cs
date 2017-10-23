<% if(isPlugin) { %>using IBI.<%= projectname %>.Plugin.Models.Entities;<% } else { %>using IBI.<%= projectname %>.Application.Models.Entities;<%}%>

/// <summary>
/// Created by Genie - Entity Scaffolding on <%= TodaysDate %> by verion <%= Version %>
///
/// Add any necessarsy custom functions to custom
/// entity controller actions
/// </summary>
<% if(isPlugin) { %>namespace IBI.<%= projectname %>.Plugin.Services.RestClient<% } else { %>namespace IBI.<%= projectname %>.Application.Services.RestClient<%}%>
{
    public class <%= entityinfo.PropertyName %>RestClient : Base<<%= entityinfo.PropertyName %>>
    {
        public <%= entityinfo.PropertyName %>RestClient(string url, string resource, string username, string role)
            : base(url, resource, username, role)
        {
        }
    }
}