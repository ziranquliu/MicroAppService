using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ID4.IdServer
{
    public class Config
    {
        /// <summary>
        /// 返回应用列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            List<ApiResource> resources = new List<ApiResource>();
            //ApiResource第一个参数是应用的名字，第二个参数是显示名字
            resources.Add(new ApiResource("chatapi", "我的聊天软件"));
            resources.Add(new ApiResource("rpandroidapp", "安卓app"));
            resources.Add(new ApiResource("bdxcx", "百度小程序"));
            return resources;
        }

        /// <summary>
        /// 返回账号列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            List<Client> clients = new List<Client>(); clients.Add(new Client
            {
                ClientId = "wyt",//用户名
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
            {
                new Secret("123321".Sha256())//秘钥
            },
                AllowedScopes = { "chatapi", "rpandroidapp" }//这个账号支持访问哪些应用
            }); return clients;
        }
    }
}