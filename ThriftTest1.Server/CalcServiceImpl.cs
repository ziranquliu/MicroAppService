using System;
using System.Collections.Generic;
using System.Text;
using ThriftTest1.Contract;

namespace ThriftTest1.Server
{
    public class CalcServiceImpl : CalcService.Iface
    {
        public int Add(int i1, int i2)
        {
            return i1 + i2;
        }
    }
}