namespace IBI.<%= Name %>.Plugin.ConfigSettings
{
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
        }
    }
}