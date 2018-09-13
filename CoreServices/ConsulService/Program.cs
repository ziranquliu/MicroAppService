using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PeterKottas.DotNetCore.WindowsService;
using System.Diagnostics;

namespace ConsulService
{
    internal class Program
    {
        internal static IConfigurationRoot Configuration;

        public static void Main(string[] args)
        {
#if !DEBUG
            Configuration = new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
#else
            Configuration = new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();
#endif

            var svcProvider = new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder
                    .SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace)
                    .AddProvider(new LogFileProvider()); // Implemented vanilla LogFile provider but is easily swapped for Nlog or SeriLog (et al.) providers
                })
                .AddOptions()
                .AddSingleton(new LoggerFactory()
                .AddConsole())
                .BuildServiceProvider();

            var _logger = svcProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();

            //string ConsulPath = Program.Configuration["Consul:Path"];
            //string CmdArg = Program.Configuration["Consul:CmdArg"];

            ServiceRunner<FileService>.Run(config =>
                {
                    var name = config.GetDefaultName();
                    config.Service(serviceConfig =>
                    {
                        serviceConfig.ServiceFactory((extraArguments, controller) =>
                        {
                            return new FileService(controller, svcProvider.GetRequiredService<ILoggerFactory>().CreateLogger<FileService>());
                        });

                        serviceConfig.OnStart((service, extraParams) =>
                        {
                            _logger.LogTrace("Service {0} started", name);
                            service.Start();
                        });

                        serviceConfig.OnStop(service =>
                        {
                            _logger.LogTrace("Service {0} stopped", name);
                            service.Stop();
                        });

                        serviceConfig.OnShutdown(service =>
                        {
                            _logger.LogTrace("Service {0} Shutdown", name);
                            service.Stop();
                        });

                        serviceConfig.OnUnInstall(service =>
                        {
                            _logger.LogTrace("Service {0} UnInstall", name);
                            service.Stop();
                        });

                        serviceConfig.OnError(e =>
                        {
                            _logger.LogError(e, string.Format("Service {0} errored with exception", name));
                        });
                    });
                });
        }
    }
}