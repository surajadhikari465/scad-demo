using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace OutOfStock.WebService
{
    
    [ServiceContract]
    public interface IOosBackend
    {
        [OperationContract]
        void ScanProductsByStoreAbbreviation(DateTime scanDate, string regionAbbrev, string storeAbbrev, string[] upcs, string userName, string userEmail, string sessionId);
        [OperationContract]
        string Configure(string region, string store, string username, string useremail, string sessionId, string ipAddress);
        [OperationContract]
        string[] RegionNames();
        [OperationContract]
        string[] StoreNamesFor(string regionName);
        [OperationContract]
        string[] RegionAbbreviations();
        [OperationContract]
        string[] StoreAbbreviationsFor(string regionAbbrev);
        [OperationContract]
        string ValidateRegionStore(string region, string store, string sessionId);
    }
}
