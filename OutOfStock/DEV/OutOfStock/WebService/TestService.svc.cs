
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Helpers;
using OOS.Model;
using OutOfStock.ScanManagement;
using StructureMap;


namespace OutOfStock.WebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TestService.svc or TestService.svc.cs at the Solution Explorer and start debugging.
    public class TestService : ITestService
    {
        private readonly IRawScanRepository _rawScanRepo;


        public TestService()
        {
            _rawScanRepo = ObjectFactory.GetInstance<IRawScanRepository>();
        }

        public void ScanProductsByStoreAbbreviation(DateTime scanDate, string regionAbbrev, string storeAbbrev, string[] upcs)
        {

            var scan = new
            {
                scanDate,
                regionAbbrev,
                storeAbbrev,
                upcs
            };

            _rawScanRepo.SaveRawScan( Newtonsoft.Json.JsonConvert.SerializeObject(scan));
        }

        public string Ping()
        {
            return DateTime.Now.ToLongDateString();
        }
    }
}
