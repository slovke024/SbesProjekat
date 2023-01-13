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
        public static Dictionary<string, User> UserAccountsDB = new Dictionary<string, User>();

        /// <summary>
        /// Add new user to UserAccountsDB. Dictionary Key is "username"
        /// </summary>
        public void AddUser(string username, string password)
        {
            if (!UserAccountsDB.ContainsKey(username))
            {
                UserAccountsDB.Add(username, new User(username, password));
            }
            else
            {
                Console.WriteLine($"Korisnik sa korisnickim imenom {username} vec postoji u bazi");
            }

            IIdentity identity = Thread.CurrentPrincipal.Identity;

            Console.WriteLine("Tip autentifikacije : " + identity.AuthenticationType);

            WindowsIdentity windowsIdentity = identity as WindowsIdentity;

            Console.WriteLine("Ime klijenta koji je pozvao metodu : " + windowsIdentity.Name);
            Console.WriteLine("Jedinstveni identifikator : " + windowsIdentity.User);

            Console.WriteLine("Grupe korisnika:");
            foreach (IdentityReference group in windowsIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                string name = (sid.Translate(typeof(NTAccount))).ToString();
                Console.WriteLine(name);
            }
        }

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
    }
}
