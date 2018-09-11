using System;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;

namespace ThriftTest1.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //TServerTransport transport = new TServerSocket(8888);
            //var processor = new ThriftTest1.Contract.UserService.Processor(new
            //UserServiceImpl());
            //TServer server = new TThreadPoolServer(processor, transport);
            //server.Serve();

            TServerTransport transport = new TServerSocket(8888);
            var processorUserService = new ThriftTest1.Contract.UserService.Processor(new UserServiceImpl());
            var processorCalcService = new ThriftTest1.Contract.CalcService.Processor(new CalcServiceImpl());
            var processorMulti = new TMultiplexedProcessor();
            processorMulti.RegisterProcessor("userService", processorUserService);
            processorMulti.RegisterProcessor("calcService", processorCalcService);

            TServer server = new TThreadPoolServer(processorMulti, transport);
            server.Serve();

            Console.WriteLine("Hello World!");
        }
    }
}