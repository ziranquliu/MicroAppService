using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyHystrix1
{
    public class Person//需要public类
    {
        [HystrixCommand(nameof(Hello1FallBackAsync))]
        public virtual async Task<string> HelloAsync(string name)//需要是虚方法
        {
            Console.WriteLine("hello" + name); String s = null; s.ToString(); return "ok";
        }

        [HystrixCommand(nameof(Hello2FallBackAsync))]
        public virtual async Task<string> Hello1FallBackAsync(string name)
        {
            Console.WriteLine("Hello降级1" + name); String s = null; s.ToString(); return "fail_1";
        }

        public virtual async Task<string> Hello2FallBackAsync(string name)
        {
            Console.WriteLine("Hello降级2" + name);

            return "fail_2";
        }

        [HystrixCommand(nameof(AddFall))]
        public virtual int Add(int i, int j)
        {
            String s = null; s.ToString(); return i + j;
        }

        public int AddFall(int i, int j)
        {
            return 0;
        }
    }

    //public class Person//需要public类
    //{
    //    [HystrixCommand(nameof(HelloFallBackAsync))]
    //    public virtual async Task<string> HelloAsync(string name)//需要是虚方法
    //    {
    //        Console.WriteLine("hello" + name);
    //        String s = null;
    //        // s.ToString();
    //        return "ok";
    //    }

    //    public async Task<string> HelloFallBackAsync(string name)
    //    {
    //        Console.WriteLine("执行失败" + name); return "fail";
    //    }

    //    [HystrixCommand(nameof(AddFall))]
    //    public virtual int Add(int i, int j)
    //    {
    //        String s = null;
    //        //  s.ToArray();
    //        return i + j;
    //    }

    //    public int AddFall(int i, int j)
    //    {
    //        return 0;
    //    }
    //}
}