using Polly;
using Polly.Caching;
using Polly.Timeout;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace pollytest1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //TEST1();
            没返回值的例子();

            Console.ReadKey();
        }

        public static async void 没返回值的例子()
        {
            Policy policy = Policy
       .Handle<Exception>()
       .FallbackAsync(async c =>
       {
           Console.WriteLine("执行出错");
       }, async ex =>
       {//对于没有返回值的，这个参数直接是异常
           Console.WriteLine(ex);
       });
            policy = policy.WrapAsync(Policy.TimeoutAsync(3, TimeoutStrategy.Pessimistic, async (context, timespan, task) =>
            {
                Console.WriteLine("timeout");
            }));
            await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("开始任务");
                await Task.Delay(5000);//注意不能用Thread.Sleep(5000);
                Console.WriteLine("完成任务");
            });
        }

        public static async void 带返回值的例子()
        {
            Policy<byte[]> policy = Policy<byte[]>.Handle<Exception>()
            .FallbackAsync(async c =>
            {
                Console.WriteLine("执行出错");
                return new byte[0];
            }, async r =>
            {
                Console.WriteLine(r.Exception);
            });
            policy = policy.WrapAsync(Policy.TimeoutAsync(20, TimeoutStrategy.Pessimistic, async (context, timespan, task) =>
            {
                Console.WriteLine("timeout");
            }));
            var bytes = await policy.ExecuteAsync(async () =>
            {
                Console.WriteLine("开始任务");
                HttpClient httpClient = new HttpClient();
                var result = await httpClient.GetByteArrayAsync("http://static.rupeng.com/upload/chatimage/20183/07EB793A4C247A654B31B4D14EC64BCA.png");
                Console.WriteLine("完成任务");
                return result;
            });
            Console.WriteLine("bytes长度" + bytes.Length);
        }

        private static void TEST1()
        {
            //策略封装
            //可以把多个ISyncPolicy合并到一起执行：
            //policy3 = policy1.Wrap(policy2);
            //执行policy3就会把policy1、policy2封装到一起执行
            //policy9 = Policy.Wrap(policy1, policy2, policy3, policy4, policy5); 把更多一起封装。

            //Policy<string> policy = Policy<string>.Handle<Exception>() //故障
            //    .Fallback(() =>//动作
            //    {
            //        Console.WriteLine("执行出错");
            //        return "降级的值";
            //    });
            //string value = policy.Execute(() =>
            //{
            //    Console.WriteLine("开始任务");
            //    throw new Exception("Hello world!");
            //    Console.WriteLine("完成任务");
            //    return "正常的值";
            //});
            //Console.WriteLine("返回值：" + value);

            //Policy policyRetry = Policy.Handle<Exception>().Retry(3);
            //Policy policyFallback = Policy.Handle<Exception>()
            //     .Fallback(() =>
            //     {
            //         Console.WriteLine("降级");
            //     });
            ////Wrap：包裹。policyRetry在里面，policyFallback裹在外面。
            ////如果里面出现了故障，则把故障抛出来给外面
            //Policy policy = policyFallback.Wrap(policyRetry);
            //policy.Execute(() =>
            //{
            //    Console.WriteLine("开始任务");
            //    if (DateTime.Now.Second % 10 != 0)
            //    {
            //        throw new Exception("出错");
            //    }
            //    Console.WriteLine("完成任务");
            //});

            Policy policy = Policy
            .Handle<Exception>()    //定义所处理的故障
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
           }, ex =>
           {
               Console.WriteLine("详细异常对象" + ex);
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
                    }, ex =>
                    {
                        Console.WriteLine("详细异常对象" + ex);
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