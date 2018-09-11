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
                //注意p指向的对象是AspectCore生成的Person的动态子类的对象，直接new Person是无法被拦截的。
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