using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        void AddUser(string username, string password);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void CreateFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void CreateFolder(string folderName);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        (List<string> Files, List<string> Directories) ShowFolderContent(string folderName);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        string ReadFile(string fileName);

        [OperationContract]
        [FaultContract(typeof(SecurityException))]
        void Delete(string fileName);

    }
}
