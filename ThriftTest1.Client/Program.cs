using System;
using Thrift.Protocol;
using Thrift.Transport;
using ThriftTest1.Contract;

namespace ThriftTest1.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //using (TTransport transport = new TSocket("127.0.0.1", 8888))
            //using (TProtocol protocol = new TBinaryProtocol(transport))
            //using (var clientUser = new UserService.Client(protocol))
            //{
            //    transport.Open();
            //    User u = clientUser.Get(1);
            //    Console.WriteLine($"{u.Id},{u.Name}");
            //}

            using (TTransport transport = new TSocket("127.0.0.1", 8888))
            using (TProtocol protocol = new TBinaryProtocol(transport))
            using (var protocolUserService = new TMultiplexedProtocol(protocol, "userService"))
            using (var clientUser = new UserService.Client(protocolUserService))
            using (var protocolCalcService = new TMultiplexedProtocol(protocol, "calcService"))
            using (var clientCalc = new CalcService.Client(protocolCalcService))
            {
                transport.Open();
                User u = clientUser.Get(1);
                Console.WriteLine($"{u.Id},{u.Name}");
                Console.WriteLine(clientCalc.Add(1, 2));
            }

            Console.ReadKey();
        }
    }
}