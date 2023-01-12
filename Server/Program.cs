using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Server";

            ServiceHost host = new ServiceHost(typeof(EchoService));
            host.AddServiceEndpoint(typeof(IEchoService), binding, address);
            host.Open();

            Console.WriteLine("Server is running...");
            Console.WriteLine("Press enter to stop.");
            Console.ReadLine();

            host.Close();
        }
    }
}

[ServiceContract]
interface IEchoService
{
    [OperationContract]
    string Echo(string input);
}

class EchoService : IEchoService
{
    public string Echo(string input)
    {
        // Get the IP address and port of the client
        var remoteEndpointMessageProperty = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
        var clientIP = remoteEndpointMessageProperty.Address;
        var clientPort = remoteEndpointMessageProperty.Port;

        Console.WriteLine("Client IP: {0}, Port: {1}", clientIP, clientPort);
        return input;
    }
}