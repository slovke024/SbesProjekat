using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
            string address = "net.tcp://localhost:9999/SecurityService";

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            Console.WriteLine("Korisnik koji je pokrenuo klijenta je : " + WindowsIdentity.GetCurrent().Name);

            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));

            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                proxy.AddUser("pera", "peric");
                proxy.AddUser("pera", "peric");
            }

            Console.ReadLine();

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

