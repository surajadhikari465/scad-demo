using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OutOfStock.WebService
{
    [ServiceContract]
    public interface IScanOutOfStock
    {
        [OperationContract]
        string[] Validate(string[] upcs);

        [OperationContract]
        string CreateEventsFor(string storeName, string[] upcs);

        [FaultContract(typeof(ValidationFaultException))]
        [FaultContract(typeof(MovementDataReadFaultException))]
        [FaultContract(typeof(ProductDataReadFaultException))]
        [FaultContract(typeof(ScanProductFaultException))]
        [FaultContract(typeof(NoProductDataForAnyScanFaultException))]
        [OperationContract]
        void ScanProducts(DateTime scanDate, string regionName, string storeName, string[] upcs);

        [OperationContract]
        string ValidateFor(string upc);

        [OperationContract]
        string CreateEventFor(string storeName, string upc);

        [OperationContract]
        string Ping(string echo);

        [OperationContract]
        string[] RegionNames();

        [OperationContract]
        string[] StoreNamesFor(string regionName);

        [OperationContract]
        string[] RegionAbbreviations();

        [OperationContract]
        string[] StoreAbbreviationsFor(string regionAbbrev);


        [FaultContract(typeof(ValidationFaultException))]
        [FaultContract(typeof(MovementDataReadFaultException))]
        [FaultContract(typeof(ProductDataReadFaultException))]
        [FaultContract(typeof(ScanProductFaultException))]
        [FaultContract(typeof(NoProductDataForAnyScanFaultException))]
        [OperationContract]
        void ScanProductsByStoreAbbreviation(DateTime scanDate, string regionAbbrev, string storeAbbrev, string[] upcs);
    }
}
