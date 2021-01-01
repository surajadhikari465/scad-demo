using System;
using OOS.Model;
using OOSCommon;
using OutOfStock.ScanManagement;
using StructureMap;

namespace OutOfStock.WebService
{
    
    public class OosBackend : IOosBackend
    {
        private readonly IRawScanRepository rawScanRepo;
        private readonly IOOSLog logger;

        public OosBackend()
        {
            rawScanRepo = ObjectFactory.GetInstance<IRawScanRepository>();
            logger = ObjectFactory.GetInstance<ILogService>().GetLogger();

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

            logger.Debug($"[mobile:scan] {sessionId} upcs:{upcs.Length} user:{userName}");
            rawScanRepo.SaveRawScan(Newtonsoft.Json.JsonConvert.SerializeObject(scan));
        }

        public string Configure(string region, string store, string username, string useremail, string sessionId, string ipAddress)
        {
            rawScanRepo.Login(username, useremail, region,store, sessionId, ipAddress);
            return rawScanRepo.GetConfiguration(region, store,sessionId);
        }

        public string[] RegionNames()
        {
            return rawScanRepo.RegionNames();
        }

        public string[] StoreNamesFor(string regionName)
        {
            return rawScanRepo.StoreNamesFor(regionName);
        }

        public string[] RegionAbbreviations()
        {
            return rawScanRepo.RegionAbbreviations();
        }

        public string[] StoreAbbreviationsFor(string regionAbbrev)
        {
            return rawScanRepo.StoreAbbreviationsFor(regionAbbrev);
        }

        public string ValidateRegionStore(string region, string store, string sessionId)
        {
            return rawScanRepo.ValidateRegionStore(region, store, sessionId) ? "valid" : "invalid";
        }
        
    }
}
