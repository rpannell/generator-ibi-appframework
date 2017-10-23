using IBI.<%= Name %>.Application.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Application.Services.RestClient
{
    public class Base<T> : RestClient<T>
    {
        #region Constructors

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
        /// Used to create a base service using a token
        /// </summary>
        /// <param name="url"></param>
        /// <param name="resource"></param>
        /// <param name="token"></param>
        public Base(string url, string resource, string token)
           : base(url, resource, token)
        {
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Parses the string and deserialize it back to a type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse"></param>
        /// <returns></returns>
        public T ParseResponseStringToEntity(string parse)
        {
            return (T)JsonConvert.DeserializeObject(parse, typeof(T));
        }

        /// <summary>
        /// Parses the string and deserialize it back to a list of type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parse"></param>
        /// <returns></returns>
        public List<T> ParseResponseStringToListOfEntity(string parse)
        {
            return (List<T>)JsonConvert.DeserializeObject(parse, typeof(List<T>));
        }

        #endregion Methods
    }
}