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
            var binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/Server";
            var factory = new ChannelFactory<IEchoService>(binding, address);
            var channel = factory.CreateChannel();

            while (true)
            {
                ShowMenu();
                var option = Console.ReadLine();
                if (option == "8")
                    break;

                switch (option)
                {
                    case "1":
                        // Show folder content
                        break;
                    case "2":
                        // Read file
                        break;
                    case "3":
                        // Create folder
                        break;
                    case "4":
                        // Create file
                        break;
                    case "5":
                        // Delete
                        break;
                    case "6":
                        // Rename
                        break;
                    case "7":
                        // Move to
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            factory.Close();
        }

        static void ShowMenu()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Show folder content");
            Console.WriteLine("2. Read file");
            Console.WriteLine("3. Create folder");
            Console.WriteLine("4. Create file");
            Console.WriteLine("5. Delete");
            Console.WriteLine("6. Rename");
            Console.WriteLine("7. Move to");
            Console.WriteLine("8. Exit");
        }
    }
}



[ServiceContract]
interface IEchoService
{
    [OperationContract]
    string Echo(string input);
}