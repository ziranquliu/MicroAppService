using AspectCore.DynamicProxy;
using System;
using System.Threading;

namespace aspnetcorehystrix1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                Person p = proxyGenerator.CreateClassProxy<Person>();
                Console.WriteLine(p.HelloAsync("Hello World").Result);
                Console.WriteLine(p.Add(1, 2));
                while (true)
                {
                    Console.WriteLine(p.HelloAsync("Hello World").Result);
                    Thread.Sleep(100);
                }
            }
        }
    }
}