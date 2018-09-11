using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LoginService.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> RequestToken(RequestTokenParam model)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict["client_id"] = "clientPC1";
            dict["client_secret"] = "123321";
            dict["grant_type"] = "password";
            dict["username"] = model.username;
            dict["password"] = model.password;

            //由登录服务器向IdentityServer发请求获取Token 
            using (HttpClient http = new HttpClient())
            using (var content = new FormUrlEncodedContent(dict))
            {
                var msg = await http.PostAsync("http://localhost:9500/connect/token", content);
                string result = await msg.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
        }
    }
}