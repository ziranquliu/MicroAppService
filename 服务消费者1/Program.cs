using Consul;
using System;
using System.Linq;

namespace 服务消费者1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
            //{
            //    var services = consulClient.Agent.Services().Result.Response;
            //    foreach (var service in services.Values)
            //    {
            //        Console.WriteLine($"id={service.ID},name={service.Service},ip={service.Address},port={service.Port}");
            //    }
            //}

            #region 客户端负载均衡 使用当前 TickCount 进行取模的方式达到随机获取一台服务器实例

            using (var consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
            {
                var services = consulClient.Agent.Services().Result.Response.Values.Where(s => s.Service.Equals("MsgService", StringComparison.OrdinalIgnoreCase));
                if (!services.Any())
                {
                    Console.WriteLine("找不到服务的实例");
                }
                else
                {
                    var service = services.ElementAt(Environment.TickCount % services.Count());
                    Console.WriteLine($"{service.Address}:{service.Port}");
                }
            }

            #endregion 客户端负载均衡 使用当前 TickCount 进行取模的方式达到随机获取一台服务器实例

            Console.ReadKey();
        }
    }
}