/// <summary>
/// Created by Genie <%= TodaysDate %> by verion <%= Version %>
/// </summary>
namespace IBI.<%= Name %>.Plugin.ConfigSettings
{
    /// <summary>
    /// Set up AutoMapper
    /// </summary>
    public class AutoMapperConfiguration
    {
        public static bool loaded = false;

        public static void Configure()
        {
            if (AutoMapperConfiguration.loaded == false)
            {
                ConfigureProjectMapping();
            }
            AutoMapperConfiguration.loaded = true;
        }

        private static void ConfigureProjectMapping()
        {
            //ADD THE AUTOMAPPER CONFIGURATIONS HERE
        }
    }
}