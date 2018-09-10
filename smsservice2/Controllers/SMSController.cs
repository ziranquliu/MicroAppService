using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace smsservice2.Controllers
{
    [Route("api/[Controller]")]
    public class SMSController : Controller
    {
        [Route("Send")]
        public bool Send(string msg)
        {
            string value = Request.Headers["X-Hello"];
            Console.WriteLine($"x-hello={value}");
            Console.WriteLine("发送短信" + msg);
            return true;
        }
    }
}