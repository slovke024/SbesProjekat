using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecurityException = Common.SecurityException;

namespace Server
{
    internal class SecurityServer : ISecurityService
    {
        public void CreateFile(string fileName)
        {
            /*
            //server kreira fajl za klijenta pod svojim imenom

            Console.WriteLine($"Process Identity:{WindowsIdentity.GetCurrent().Name}");
            try
            {
                StreamWriter sw = File.CreateText(fileName);
                sw.Close();
            }
            catch (Exception e)
            {
                throw new FaultException<SecurityException>(new SecurityException(e.Message));
            }
            */


            //server kreira fajl u ime klijenta (implicitna impersonifikacija)
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            using (windowsIdentity.Impersonate())
            {
                Console.WriteLine($"Process Identity :{WindowsIdentity.GetCurrent().Name}");
                try
                {
                    
                    string currentDirectory = Environment.CurrentDirectory;
                    string solutionPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));
                    string bazePath = Path.Combine(solutionPath, "Baza");
                    string filePath = Path.Combine(bazePath,fileName);
                    StreamWriter sw = File.CreateText(filePath);
 
                    sw.Close();
                }
                catch (Exception e)
                {
                    throw new FaultException<SecurityException>(new SecurityException(e.Message));
                }

            }
        }

        public void CreateFolder(string folderName)
        {
            
            IIdentity identity = Thread.CurrentPrincipal.Identity;
            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            using (windowsIdentity.Impersonate())
            {
                Console.WriteLine($"Process Identity :{WindowsIdentity.GetCurrent().Name}");
                try
                {
                    string currentDirectory = Environment.CurrentDirectory;
                    string folderPath = Path.Combine(currentDirectory, folderName);
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e)
                {
                    throw new FaultException<SecurityException>(new SecurityException(e.Message));
                }

            }
        }

        public (List<string> Files, List<string> Directories) ShowFolderContent(string folderName)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string solutionPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));
            string folderPath = Path.Combine(solutionPath, folderName);
            List<string> files = Directory.GetFiles(folderPath).ToList();
            List<string> directories = Directory.GetDirectories(folderPath).ToList();
            return (files, directories);
        }

        public string ReadFile(string fileName)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string solutionPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));
            string bazePath = Path.Combine(solutionPath, "Baza");
            string folderPath = Path.Combine(bazePath, fileName);
            string content = "";
            try
            {
                using (StreamReader sr = new StreamReader(folderPath))
                {
                    content = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
            return content;
        }

        public void Delete(string fileName)
        {
            try
            {
                string currentDirectory = Environment.CurrentDirectory;
                string solutionPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));
                string bazePath = Path.Combine(solutionPath, "Baza");
                string filePath = Path.Combine(bazePath, fileName);
                File.Delete(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
            }
        }
        public bool Rename(string currentFileName, string newFileName)
        {
            try
            {
                string currentDirectory = Environment.CurrentDirectory;
                string solutionPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));
                string bazePath = Path.Combine(solutionPath, "Baza");
                string currentFilePath = Path.Combine(bazePath, currentFileName);
                string newFilePath = Path.Combine(bazePath, newFileName);
                if (File.Exists(currentFilePath))
                {
                    File.Move(currentFilePath, newFilePath);
                    return true;
                }
                else
                {
                    Console.WriteLine("Fajl ne postoji");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }

        public bool MoveTo(string fileName, string folderName)
        {
            try
            {
                string currentDirectory = Environment.CurrentDirectory;
                string solutionPath = Path.GetFullPath(Path.Combine(currentDirectory, "..", ".."));
                string bazePath = Path.Combine(solutionPath, "Baza");
                string filePath = Path.Combine(bazePath, fileName);
                string destinationPath = Path.Combine(bazePath, folderName);
                if (File.Exists(filePath))
                {
                    if (Directory.Exists(destinationPath))
                    {
                        string newFilePath = Path.Combine(destinationPath, fileName);
                        File.Move(filePath, newFilePath);
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid folder name, please try again");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("File doesn't exist, please check the name");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                return false;
            }
        }
    }
}
