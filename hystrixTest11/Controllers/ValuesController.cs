using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace hystrixTest11.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private Person p;

        public ValuesController(Person p)
        {
            this.p = p;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            /*
            ProxyGeneratorBuilder proxyGeneratorBuilder = new ProxyGeneratorBuilder();
            using (IProxyGenerator proxyGenerator = proxyGeneratorBuilder.Build())
            {
                //Person p = new Person();
                Person p = proxyGenerator.CreateClassProxy<Person>();
                await p.HelloAsync("rupeng.com");
            }*/
            await p.HelloAsync("rupeng.com");
            return new string[] { "value1", "value2" };
        }
    }
}