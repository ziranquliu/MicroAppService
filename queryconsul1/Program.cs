using Consul;
using RestTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace queryconsul1
{
    internal class Progaram
    {
        private static void Main(string[] args)
        {
            using (ConsulClient consulClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500")))
            {
                //consulClient.Agent.Services()获取consul中注册的所有的服务
                Dictionary<String, AgentService> services = consulClient.Agent.Services().Result.Response;
                foreach (KeyValuePair<String, AgentService> kv in services)
                {
                    Console.WriteLine($"key={kv.Key},{kv.Value.Address},{kv.Value.ID},{kv.Value.Service},{kv.Value.Port}");
                }

                //获取所有服务名字是"apiservice1"所有的服务
                var agentServices = services.Where(s => s.Value.Service.Equals("apiservice1", StringComparison.CurrentCultureIgnoreCase))
                   .Select(s => s.Value);
                //根据当前TickCount对服务器个数取模，“随机”取一个机器出来，避免“轮询”的负载均衡策略需要计数加锁问题
                var agentService = agentServices.ElementAt(Environment.TickCount % agentServices.Count());
                Console.WriteLine($"{agentService.Address},{agentService.ID},{agentService.Service},{agentService.Port}");
            }

            Console.ReadKey();

            //RestTemplate rest = new RestTemplate("http://127.0.0.1:8500");
            ////RestTemplate把服务的解析和发请求以及响应反序列化帮我们完成
            //ResponseEntity<String[]> resp = rest.GetForEntityAsync<String[]>("http://apiservice1/api/values").Result;
            //Console.WriteLine(resp.StatusCode);
            //Console.WriteLine(String.Join(",", resp.Body));

            //Console.ReadKey();
        }
    }
}