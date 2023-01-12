using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Server";

            ChannelFactory<IEchoService> factory = new ChannelFactory<IEchoService>(binding, address);
            IEchoService channel = factory.CreateChannel();

            Console.WriteLine(channel.Echo("Hello, World!"));

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();

            factory.Close();
        }
    }
}

[ServiceContract]
interface IEchoService
{
    [OperationContract]
    string Echo(string input);
}