using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestTools;

namespace WebAppService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
            //CreateWebHostBuilder(args).Build().Run();
        }

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
            String ip = config["ip"];
            String port = config["port"];

            if (port == "0")
            {
                port = Tools.GetRandAvailablePort().ToString();
            }

            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls($"http://{ip}:{port}")
                .Build();
        }

        //public static IWebHost BuildWebHost(string[] args)
        //{
        //    var config = new ConfigurationBuilder()
        //        .AddCommandLine(args)
        //        .Build();
        //    String ip = config["ip"];
        //    String port = config["port"];

        //    return WebHost.CreateDefaultBuilder(args)
        //        .UseStartup<Startup>()
        //        .UseUrls($"http://{ip}:{port}")
        //        .Build();
        //}
    }
}