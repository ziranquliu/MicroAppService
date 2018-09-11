using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ocelotserver3
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
            services.AddOcelot(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var configuration = new OcelotPipelineConfiguration
            {
                PreErrorResponderMiddleware = async (ctx, next) =>
                {
                    if (!ctx.HttpContext.Request.Path.Value.StartsWith("/auth"))//不以auth开头的一律校验
                    {
                        String token = ctx.HttpContext.Request.Headers["token"].FirstOrDefault();
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            ctx.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (StreamWriter writer = new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("token required");
                            }
                            return;
                        }
                        var secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
                        try
                        {
                            IJsonSerializer serializer = new JsonNetSerializer();
                            IDateTimeProvider provider = new UtcDateTimeProvider();
                            IJwtValidator validator = new JwtValidator(serializer, provider);
                            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                            var json = decoder.Decode(token, secret, verify: true);
                            Console.WriteLine(json);
                            dynamic payload = JsonConvert.DeserializeObject<dynamic>(json);
                            string userName = payload.UserName; ctx.HttpContext.Request.Headers.Add("X-UserName", userName);//将解析出来的用户名传输给后端服务器。
                        }
                        catch (TokenExpiredException)
                        {
                            ctx.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (StreamWriter writer = new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("Token has expired");
                            }
                        }
                        catch (SignatureVerificationException)
                        {
                            ctx.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (StreamWriter writer = new StreamWriter(ctx.HttpContext.Response.Body))
                            {
                                writer.Write("Token has invalid signature");
                            }
                        }
                    }
                    await next.Invoke();
                }
            };

            //app.UseMvc();
            //app.UseOcelot().Wait();//不要忘了写Wait
            app.UseOcelot(configuration).Wait();//不要忘了写Wait
        }
    }
}