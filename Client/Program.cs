using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
                                Console.WriteLine("Nema fajlova u ovom folderu.");
                                Console.WriteLine("****************************\n");
                            }
                            else
                            {
                                Console.WriteLine("Txt fajlovi:");
                                foreach (string file in files)
                                {
                                    string file1 = Path.GetFileName(file);
                                    Console.WriteLine(file1);
                                }
                                Console.WriteLine("****************************\n");
                            }
                            if (directories.Count == 0)
                            {
                                Console.WriteLine("Nema foldera unutar ovog foldera.");
                                Console.WriteLine("****************************\n");
                            }
                            else
                            {
                                Console.WriteLine("Folderi:");
                                foreach (string directory in directories)
                                {
                                    string folder = Path.GetFileName(directory);
                                    Console.WriteLine(folder);
                                }
                                Console.WriteLine("****************************\n");
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
                                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                                {
                                    aes.BlockSize = 128;
                                    aes.KeySize = 256;
                                    aes.Mode = CipherMode.CBC;
                                    aes.Padding = PaddingMode.PKCS7;
                                    string key = "skrfjcmbksodjeskdosesmvkdsdtrksd";
                                    string iv = "sjtrvkdsktmbgfsk";
                                    aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
                                    aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(iv);

                                    byte[] plainText = Convert.FromBase64String(content);
                                    byte[] cipherText;

                                    ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                                    cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
                                    crypto.Dispose();
                                    Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(cipherText));
                                }
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
                            channel.CreateFolder(imeFoldera1);
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
                            Console.WriteLine("Preimenuj fajl\n");
                            Console.Write("Unesi trenutno ime fajla: ");
                            string currentFileName = Console.ReadLine();
                            Console.Write("Unesi novo ime fajla: ");
                            string newFileName = Console.ReadLine();
                            if (channel.Rename(currentFileName, newFileName))
                            {
                                Console.WriteLine("Ime fajla uspesno promenjeno!");
                                Console.WriteLine("****************************\n");
                            }
                            else
                            {
                                Console.WriteLine("Neuspesno preimenovanje");
                                Console.WriteLine("****************************\n");
                            }
                            break;
                        case "7":
                            // Move to
                            Console.WriteLine("Premesti fajl\n");
                            Console.Write("Unesite ime fajla: ");
                            string fileName2 = Console.ReadLine();
                            Console.Write("Unesite ime foldera: ");
                            string folderName = Console.ReadLine();
                            if (channel.MoveTo(fileName2, folderName))
                            {
                                Console.WriteLine("File uspesno premesten");
                                Console.WriteLine("****************************\n");
                            }
                            else
                            {
                                Console.WriteLine("Fajl neuspesno premesten");
                                Console.WriteLine("****************************\n");
                            }
                            break;
                        
                        default:
                            Console.WriteLine("Nevalidna opcija!");
                            break;
                    }
                }
            }

            Console.ReadLine();

            
        }

        static void ShowMenu()
        {
            Console.WriteLine("Meni:");
            Console.WriteLine("1. Prikazi sadrzaj foldera");
            Console.WriteLine("2. Procitaj fajl");
            Console.WriteLine("3. Kreiraj folder");
            Console.WriteLine("4. Kreiraj fajl");
            Console.WriteLine("5. Obrisi");
            Console.WriteLine("6. Preimenuj");
            Console.WriteLine("7. Premesti");
            Console.WriteLine("8. IZLAZ");
            Console.WriteLine("****************************\n");
        }
    }
}

