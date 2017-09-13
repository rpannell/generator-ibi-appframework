using IBI.<%= Name %>.Plugin.Utils;
using System.Collections.Generic;

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

        /// <summary>
        /// Parses the string and deserialize it back to a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse"></param>
        /// <returns></returns>
        public T ParseResponseStringToEntity(string parse)
        {
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject(parse, typeof(T));
        }

        /// <summary>
        /// Parses the string and deserialize it back to a list of type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse"></param>
        /// <returns></returns>
        public List<T> ParseResponseStringToListOfEntity(string parse)
        {
            return (List<T>)Newtonsoft.Json.JsonConvert.DeserializeObject(parse, typeof(List<T>));
        }

        /*
         *  ADD CUSTOM REST FUNCTION SPECIFIC TO THE <%= Name %> HERE
         */
    }
}