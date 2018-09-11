using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTTokenServer1
{
    public class APIResult<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
        public String Message { get; set; }
    }
}