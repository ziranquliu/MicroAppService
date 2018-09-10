using Polly;
using Polly.Caching;
using Polly.Timeout;
using System;
using System.Threading;

namespace pollytest1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //策略封装
            //可以把多个ISyncPolicy合并到一起执行：
            //policy3 = policy1.Wrap(policy2);
            //执行policy3就会把policy1、policy2封装到一起执行
            //policy9 = Policy.Wrap(policy1, policy2, policy3, policy4, policy5); 把更多一起封装。

            Console.ReadKey();
        }

        /// <summary>
        /// 缓存
        /// </summary>
        private static void Caching()
        {
            //Install-Package Microsoft.Extensions.Caching.Memory
            Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
            //Install-Package Polly.Caching.MemoryCache
            Polly.Caching.MemoryCache.MemoryCacheProvider memoryCacheProvider = new Polly.Caching.MemoryCache.MemoryCacheProvider(memoryCache);

            CachePolicy policy = Policy.Cache(memoryCacheProvider, TimeSpan.FromSeconds(5));
            Random rand = new Random();
            while (true)
            {
                int i = rand.Next(5);
                Console.WriteLine("产生" + i);
                var context = new Context("doublecache" + i);
                int result = policy.Execute(ctx =>
                {
                    Console.WriteLine("Execute计算" + i);
                    return i * 2;
                }, context);
                Console.WriteLine("计算结果：" + result);
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 超时处理
        /// 用途：请求网络接口，避免接口长期没有响应造成系统卡死。
        /// </summary>
        private static void Timeout()
        {
            ISyncPolicy policy = Policy.Handle<Exception>()
           .Fallback(() =>
           {
               Console.WriteLine("执行出错");
           });
            policy = policy.Wrap(Policy.Timeout(2, TimeoutStrategy.Pessimistic));

            policy.Execute(() =>
            {
                Console.WriteLine("开始任务");
                Thread.Sleep(5000);
                Console.WriteLine("完成任务");
            });

            //        ISyncPolicy policy = Policy.Handle<TimeoutRejectedException>()
            //.Retry(1);
            //        policy = policy.Wrap(Policy.Timeout(3, TimeoutStrategy.Pessimistic));
            //        policy.Execute(() =>
            //        {
            //            Console.WriteLine("开始任务");
            //            Thread.Sleep(5000);
            //            Console.WriteLine("完成任务");
            //        });
        }

        /// <summary>
        /// 短路保护Circuit Breaker
        /// </summary>
        private static void CircuitBreaker()
        {
            ISyncPolicy policy = Policy.Handle<Exception>().CircuitBreaker(6, TimeSpan.FromSeconds(5));//连续出错6次之后熔断5秒(不会再去尝试执行业务代码）。
            while (true)
            {
                Console.WriteLine("开始Execute");
                try
                {
                    policy.Execute(() =>
                    {
                        Console.WriteLine("开始任务");
                        throw new Exception("出错");
                        Console.WriteLine("完成任务");
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("execute出错" + ex.GetType() + ":" + ex.Message);
                }
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// 重试处理
        /// </summary>
        private static void Retry()
        {
            int i = 0;
            try
            {
                ISyncPolicy policy = Policy.Handle<Exception>()
                    .RetryForever();//一直重试
                policy.Execute(() =>
                {
                    Console.WriteLine("开始任务" + (++i));
                    if (DateTime.Now.Second % 2 != 0)
                    {
                        throw new Exception("出错");
                    }

                    Console.WriteLine("完成任务");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未处理异常:{ex}");
            }

            //RetryForever()是一直重试直到成功
            //Retry()是重试最多一次；
            //Retry(n)是重试最多n次；
            //WaitAndRetry()可以实现“如果出错等待100ms再试还不行再等150ms秒。。。。”，重载方法很多，一看就懂，不再一一介绍。还有WaitAndRetryForever。
        }

        /// <summary>
        /// Polly简单使用
        /// </summary>
        private static void SimpleUse()
        {
            try
            {
                ISyncPolicy policy = Policy.Handle<ArgumentException>(ex => ex.Message == "年龄参数错误")
                    .Fallback(() =>
                    {
                        Console.WriteLine("出错了");
                    });
                policy.Execute(() =>
                {
                    //这里是可能会产生问题的业务系统代码
                    Console.WriteLine("开始任务");
                    throw new ArgumentException("年龄参数错误");
                    //throw new Exception("haha");
                    //Console.WriteLine("完成任务");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"未处理异常:{ex}");
            }
        }
    }
}