using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WFM.OutOfStock.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // TODO: The following binding doesn't work when deploying using Docker.
                //.UseUrls("http://0.0.0.0:9191")
                .UseStartup<Startup>();
    }
}
