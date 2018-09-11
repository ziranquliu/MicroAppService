using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RuPeng.HystrixCore;

namespace hystrixTest11
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // services.AddSingleton<Person>();
            RegisterServices(this.GetType().Assembly, services);
            //return services.BuildAspectCoreServiceProvider();
            return services.BuildDynamicProxyServiceProvider();
        }

        /// <summary>
        /// 扫描asm程序集中所有的public类，对于类看看是否含有标注了HystrixCommand的方法
        /// 如果有，则AddSingleton到services
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="services"></param>
        private void RegisterServices(Assembly asm, IServiceCollection services)
        {
            foreach (Type type in asm.GetExportedTypes())
            {
                //判断type类型中是否有至少一个方法含有HystrixCommandAttribute
                bool hasHystrixCmd = type.GetMethods().Any(m => m.GetCustomAttribute(typeof(HystrixCommandAttribute)) != null);
                //type.GetMethods().Where(m => m.GetCustomAttribute(typeof(HystrixCommandAttribute)) != null).Count();
                if (hasHystrixCmd)
                {
                    services.AddSingleton(type);
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}