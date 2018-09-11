using IdentityServer4;
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

        /// <summary>
        /// 返回应用列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources1()
        {
            List<ApiResource> resources = new List<ApiResource>();
            //ApiResource第一个参数是应用的名字，第二个参数是描述
            resources.Add(new ApiResource("MsgAPI", "消息服务API"));
            resources.Add(new ApiResource("ProductAPI", "产品API"));
            return resources;
        }

        /// <summary>
        /// 返回账号列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients1()
        {
            List<Client> clients = new List<Client>();
            clients.Add(new Client
            {
                ClientId = "clientPC1",//API账号、客户端Id
                AllowedGrantTypes = GrantTypes.ClientCredentials,//认证方式
                ClientSecrets =
            {
                new Secret("123321".Sha256())//秘钥
            },
                AllowedScopes = { "MsgAPI", "ProductAPI" }//这个账号支持访问哪些应用
            });
            return clients;
        }

        /// <summary> 
        /// 返回应用列表 
        /// </summary> 
        /// <returns></returns> 
        public static IEnumerable<ApiResource> GetApiResources2()
        {
            List<ApiResource> resources = new List<ApiResource>();
            //ApiResource第一个参数是应用的名字，第二个参数是描述 
            resources.Add(new ApiResource("MsgAPI", "消息服务API"));
            resources.Add(new ApiResource("ProductAPI", "产品API"));
            return resources;
        }

        /// <summary> 
        /// 返回账号列表 
        /// </summary>         
        /// <returns></returns>         
        public static IEnumerable<Client> GetClients2()
        {
            List<Client> clients = new List<Client>();
            clients.Add(new Client
            {
                ClientId = "clientPC1",//API账号、客户端Id 
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,//认证方式
                ClientSecrets =
            {
                new Secret("123321".Sha256())//秘钥 
            },
                AllowedScopes = {
                "MsgAPI",
                "ProductAPI",
                IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误 
                IdentityServerConstants.StandardScopes.Profile
            }//这个账号支持访问哪些应用 
            });
            return clients;
        }
    }
}