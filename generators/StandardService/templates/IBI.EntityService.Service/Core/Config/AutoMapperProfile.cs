using AutoMapper;

// Created by Genie <%= TodaysDate %> by verion <%= Version %>
namespace IBI.<%= Name %>.Service.Utils.Config
{
    /// <summary>
    /// Used to create the profile for AutoMapper (http://automapper.readthedocs.io/en/latest/)
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        #region Constructors

        /// <summary>
        /// Create the maps
        /// </summary>
        public AutoMapperProfile()
        {
            // http://automapper.readthedocs.io/en/latest/Configuration.html
            // ADD ANY CUSTOM MAPPINGS HERE
            

            /* GENIE HOOK */
            /* DO NOT DELETE THE ABOVE LINE */
        }

        #endregion Constructors
    }
}