using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ocelot_id4server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            services.AddAuthentication()//对配置文件中使用ChatKey配置了AuthenticationProviderKey=ChatKey的路由规则使用如下的验证方式
                .AddIdentityServerAuthentication("ChatKey", o =>
                {//IdentityService认证服务的地址
                    o.Authority = "http://127.0.0.1:9500";//!!!!!!!!!!!!!!!!!(切记，这里不可用localhost)
                    o.ApiName = "chatapi";//要连接的应用的名字
                    o.RequireHttpsMetadata = false;
                    o.SupportedTokens = SupportedTokens.Both;
                    o.ApiSecret = "123321";//秘钥
                });
            services.AddOcelot();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseMvc();
            app.UseOcelot().Wait();
        }
    }
}