using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace ZKWeb.MVVMDemo.AspNetCore
{
    //(Server=(localdb)\mssqllocaldb;Database=sedb;trusted_Connection=True;
    //Server=localhost;Database=ZKWebdb;User Id=root;Password=99B3AD6E;
    /// <summary>
    /// Asp.Net Core Main Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry Point
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            try
            {
                var host = new WebHostBuilder()
                     .UseConfiguration(new ConfigurationBuilder()
                        .AddJsonFile("hosting.json", optional: true)
                       // .AddCommandLine(args)
                        .Build())
                    .UseKestrel()
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();
                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
