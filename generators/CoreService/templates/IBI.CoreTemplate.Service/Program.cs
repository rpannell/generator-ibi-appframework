using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace IBI.<%= Name %>.Service
{
    public class Program
    {
        #region Methods

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseApplicationInsights()
                .Build();

        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        #endregion Methods
    }
}