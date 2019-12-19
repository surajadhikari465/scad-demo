
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Helpers;
using OOS.Model;
using OOSCommon;
using OutOfStock.ScanManagement;
using StructureMap;


namespace OutOfStock.WebService
{
    
    public class OosBackend : IOosBackend
    {
        private readonly IRawScanRepository _rawScanRepo;
        private ILogService logService;
        private IOOSLog _logger;

        public OosBackend()
        {
            _rawScanRepo = ObjectFactory.GetInstance<IRawScanRepository>();
            _logger = ObjectFactory.GetInstance<ILogService>().GetLogger();

        }


        public void ScanProductsByStoreAbbreviation(DateTime scanDate, string regionAbbrev, string storeAbbrev, string[] upcs, string userName, string userEmail, string sessionId)
        {

            var scan = new ScanData
            {
                ScanDate  = DateTime.Now, // override scangun supplied date/time
                RegionAbbrev  = regionAbbrev,
                StoreAbbrev = storeAbbrev,
                Upcs = upcs, 
                UserName = userName,
                UserEmail = userEmail,
                SessionId = sessionId
            };

            _logger.Debug($"[mobile:scan] {sessionId} upcs:{upcs.Length} user:{userName}");
            _rawScanRepo.SaveRawScan(Newtonsoft.Json.JsonConvert.SerializeObject(scan));
        }

        public string Configure(string region, string store, string username, string useremail, string sessionId, string ipAddress)
        {
            _rawScanRepo.Login(username, useremail, region,store, sessionId, ipAddress);
            return _rawScanRepo.GetConfiguration(region, store,sessionId);
        }

        public string[] RegionNames()
        {
            return _rawScanRepo.RegionNames();
        }

        public string[] StoreNamesFor(string regionName)
        {
            return _rawScanRepo.StoreNamesFor(regionName);
        }

        public string[] RegionAbbreviations()
        {
            return _rawScanRepo.RegionAbbreviations();
        }

        public string[] StoreAbbreviationsFor(string regionAbbrev)
        {
            return _rawScanRepo.StoreAbbreviationsFor(regionAbbrev);
        }

        public string ValidateRegionStore(string region, string store, string sessionId)
        {
            return _rawScanRepo.ValidateRegionStore(region, store, sessionId) ? "valid" : "invalid";
        }
        
    }
}
