﻿using Common;
using System;
using System.Collections.Generic;
using System.IO;
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

            ChannelFactory<ISecurityService> factory = new ChannelFactory<ISecurityService>(binding, address);
            ISecurityService channel;
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(address),
                EndpointIdentity.CreateUpnIdentity("wcfServer"));

            using (ClientProxy proxy = new ClientProxy(binding, endpointAddress))
            {
                while (true)
                {
                    channel = factory.CreateChannel();
                    ShowMenu();
                    var option = Console.ReadLine();
                    if (option == "8")
                        break;

                    switch (option)
                    {
                        case "1":
                            // Show folder content
                            Console.WriteLine("Ispisi sadrzaj foldera\n");
                            Console.WriteLine("Unesite ime foldera:");
                            string imeFoldera = Console.ReadLine();
                            (List<string> Files, List<string> Directories) folderContent = channel.ShowFolderContent(imeFoldera);
                            List<string> files = folderContent.Files;
                            List<string> directories = folderContent.Directories;
                            if (files.Count == 0)
                            {
                                Console.WriteLine("No files in this folder.");
                            }
                            else
                            {
                                Console.WriteLine("Files:");
                                foreach (string file in files)
                                {
                                    Console.WriteLine(file);
                                }
                            }
                            if (directories.Count == 0)
                            {
                                Console.WriteLine("No subdirectories in this folder.");
                            }
                            else
                            {
                                Console.WriteLine("Directories:");
                                foreach (string directory in directories)
                                {
                                    Console.WriteLine(directory);
                                }
                            }
                            break;
                        case "2":
                            // Read file
                            Console.WriteLine("Iscitaj fajl\n");
                            Console.WriteLine("Unesite ime fajla:");
                            string fileName = Console.ReadLine();
                            string content = channel.ReadFile(fileName);

                            if (!string.IsNullOrEmpty(content))
                            {
                                Console.WriteLine("Sadrzaj fajla:");
                                Console.WriteLine(content);
                            }
                            else
                            {
                                Console.WriteLine("Greska prilikom citanja.");
                            }
                            Console.ReadLine();
                            break;
                        case "3":
                            // Create folder
                            Console.WriteLine("Kreiraj folder\n");
                            Console.WriteLine("Unesite ime foldera:\n");
                            string imeFoldera1 = Console.ReadLine();
                            channel.CreateFile(imeFoldera1);
                            break;
                        case "4":
                            // Create file
                            Console.WriteLine("Kreiraj fajl\n");
                            Console.WriteLine("Unesite ime fajla:\n");
                            string imeFajla = Console.ReadLine();
                            channel.CreateFile(imeFajla);
                            break;
                        case "5":
                            // Delete
                            Console.WriteLine("Obrisi fajl\n");
                            Console.WriteLine("Unesite naziv fajla koji hocete da obrisete:");
                            string imeFajla1 = Console.ReadLine();
                            channel.Delete(imeFajla1);
                            break;
                        case "6":
                            // Rename
                            Console.WriteLine("preimenuj fajl\n");
                            break;
                        case "7":
                            // Move to
                            Console.WriteLine("premesti fajl\n");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                //testiranja
                //doda usera
                //proxy.AddUser("pera", "peric");
                //ne doda jer postoji vec
                //proxy.AddUser("pera", "peric");
                //pravi prazan txt fajl
                //proxy.CreateFile("fajl");
            }

            Console.ReadLine();

            
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

