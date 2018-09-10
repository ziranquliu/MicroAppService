using AspectCore.DynamicProxy;
using System;

namespace aoptest1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                Person p = proxyGenerator.CreateClassProxy<Person>();
                p.Say("Hello World");
                Console.WriteLine(p.GetType());
                Console.WriteLine(p.GetType().BaseType);
            }

            Console.ReadKey();
        }
    }

    public class Person
    {
        [CustomInterceptor]
        public virtual void Say(string msg)
        {
            Console.WriteLine("service calling..." + msg);
        }
    }
}