using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace emailservice1.Controllers
{
    [Route("api/[controller]")]
    public class EmailController : Controller
    {
        [Route("Send")]
        public bool Send(string msg)
        {
            Console.WriteLine("发送邮件" + msg);
            return true;
        }
    }
}