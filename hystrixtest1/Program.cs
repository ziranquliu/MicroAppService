using AspectCore.DynamicProxy;
using System;

namespace hystrixtest1
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
            }

            Console.ReadKey();
        }
    }
}