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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITestService" in both code and config file together.
    [ServiceContract]
    public interface ITestService
    {
    

        [OperationContract]
        void ScanProductsByStoreAbbreviation(DateTime scanDate, string regionAbbrev, string storeAbbrev, string[] upcs);

        [OperationContract]
        string Ping();
    }
}
