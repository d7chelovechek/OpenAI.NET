using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace OpenAI.NET.Web
{
    /// <summary>
    /// A class that has an entry point in the form of <see cref="Main(string[])"/>.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry point to the application.
        /// </summary>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Creating and configuring host builder.
        /// </summary>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(x =>
            {
                x.UseStartup<Startup>();
            });
        }
    }
}