using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ID4.IdServer.用户名密码登录;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ID4.IdServer
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
            //services.AddIdentityServer()
            //.AddDeveloperSigningCredential()
            //.AddInMemoryApiResources(Config.GetApiResources())
            //.AddInMemoryClients(Config.GetClients());

            //    services.AddIdentityServer()
            //.AddDeveloperSigningCredential()
            //.AddInMemoryApiResources(Config.GetApiResources1())
            //.AddInMemoryClients(Config.GetClients1());

            var idResources = new List<IdentityResource>
                {
                    new IdentityResources.OpenId(), //必须要添加，否则报无效的 scope 错误           
                    new IdentityResources.Profile()
                };
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(idResources)
                .AddInMemoryApiResources(Config.GetApiResources2())
                .AddInMemoryClients(Config.GetClients2())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();

            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseMvc();

            app.UseIdentityServer();
        }
    }
}