using IBI.<%= Name %>.Plugin.Utils;
using System.Collections.Generic;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin.Services.RestClient
{
    public class Base<T> : RestClient<T>
    {
        /// <summary>
        /// Used as a location for Custom rest calls to the Web Api Service specifically used
        /// for this plugin
        /// </summary>
        /// <param name="url">The URL of the web service</param>
        /// <param name="resource">The resource</param>
        /// <param name="username">The user name</param>
        /// <param name="role">The role</param>
        public Base(string url, string resource, string username, string role)
            : base(url, resource, username, role)
        {
        }
    }
}