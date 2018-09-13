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
    public class FileService : MicroService, IMicroService
    {
        private IMicroServiceController _controller;
        private ILogger<FileService> _logger;

        private string ConsulPath = Program.Configuration["Consul:Path"];
        private string CmdArg = Program.Configuration["Consul:CmdArg"];

        public FileService()
        {
            _controller = null;
        }

        public FileService(IMicroServiceController controller, ILogger<FileService> logger)
        {
            _controller = controller;
            _logger = logger;
        }

        public void Start()
        {
            StartBase();

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
                    //string cmd = "\"" + ConsulPath + "\" " + CmdArg;
                    //Process.Start("cmd.exe", cmd);

                    using (Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo.FileName = ConsulPath;
                        process.StartInfo.Arguments = CmdArg;
                        // 必须禁用操作系统外壳程序
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;

                        process.Start();

                        //string output = process.StandardOutput.ReadToEnd();

                        //if (String.IsNullOrEmpty(output) == false)
                        //    Console.WriteLine(output);

                        //process.WaitForExit();
                        //process.Close();

                        // 异步获取命令行内容
                        process.BeginOutputReadLine();

                        // 为异步获取订阅事件
                        process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
                    }
                }
                //_logger.LogInformation(string.Format("Polling at {0}\n", DateTime.Now.ToString("o")));
            });
            _logger.LogTrace("Started\n");
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (String.IsNullOrEmpty(e.Data) == false)
                _logger.LogInformation(e.Data);
        }

        public void Stop()
        {
            StopBase();
            string processName = ConsulPath.Substring(ConsulPath.LastIndexOf("\\") + 1).Replace(".exe", "");
            Process[] processs = Process.GetProcessesByName(processName);
            if (processs.Length > 0)
            {
                for (int i = 0; i < processs.Length; i++)
                {
                    processs[i].Kill();
                }
            }
            _logger.LogTrace("Stopped\n");
        }
    }
}