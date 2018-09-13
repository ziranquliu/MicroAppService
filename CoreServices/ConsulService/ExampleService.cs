using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ConsulService
{
    public class ExampleService : MicroService, IMicroService
    {
        private IMicroServiceController _controller;
        private ILogger<ExampleService> _logger;

        public ExampleService()
        {
            _controller = null;
        }

        public ExampleService(IMicroServiceController controller, ILogger<ExampleService> logger)
        {
            _controller = controller;
            _logger = logger;
        }

        public void Start()
        {
            StartBase();

            string ConsulPath = Program.Configuration["Consul:Path"];
            string CmdArg = Program.Configuration["Consul:CmdArg"];

            Timers.Start("Poller", 1000, () =>
            {
                string processName = ConsulPath.Substring(ConsulPath.LastIndexOf("\\") + 1).Replace(".exe", "");
                Process[] processs = Process.GetProcessesByName(processName);
                if (processs.Length > 1)
                {
                    for (int i = 1; i < processs.Length; i++)
                    {
                        processs[i].Close();
                        processs[i].Dispose();
                    }
                }
                if (processs.Length == 0)
                {
                    string cmd = "\"" + ConsulPath + "\" " + CmdArg;
                    Process.Start("cmd.exe", cmd);
                }
                //_logger.LogInformation(string.Format("Polling at {0}\n", DateTime.Now.ToString("o")));
            });
            _logger.LogTrace("Started\n");
        }

        public void Stop()
        {
            StopBase();
            _logger.LogTrace("Stopped\n");
        }
    }
}