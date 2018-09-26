using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace IBI.<%= Name %>.Application
{
    public class Program
    {
        #region Methods

        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseApplicationInsights()
                .Build();

            host.Run();
        }

        #endregion Methods
    }
}