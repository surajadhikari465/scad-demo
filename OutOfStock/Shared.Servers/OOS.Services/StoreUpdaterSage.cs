//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Script.Serialization;
//using System.Xml.Linq;
//using Dapper;
//using NLog;
//using OOS.Services.DAL;
//using OOS.Services.DataModels;

//namespace OOS.Services
//{
//    public class StoreUpdaterSage
//    {
//        public IOosRepo Repo;
//        public IVimRepo Vim;

//        public IEnumerable<StoreDb> Existing;
//        public IEnumerable<StoreStatus> StoreStatuses;
//        public IEnumerable<Region> Regions;

//        public List<StoreDb> UpdateList;
//        public List<StoreDb> InsertList;
//        public List<StoreDb> CloseList;
//        public List<VimStore> VimStores;

//        public int UpdateCounter = 0;
//        public int InsertCounter = 0;
//        public int CloseCounter = 0;

//        public string TokenResponse { get; set; }
//        private const string PublicKey = "pGsFJU9kYGeOrSLsRZfw7ekFsreZ6u72";
//        private const string Secret = "hIuu47QQUBwGWFtr";
//        private const string AuthUrl = "https://api.wholefoodsmarket.com/v2/oauth20/";
//        private const string StoreUrl = "https://api.wholefoodsmarket.com/v2/stores";

//        private static Logger _logger = LogManager.GetCurrentClassLogger();

//        public StoreUpdaterSage(IOosRepo oosRepo, IVimRepo vimRepo)
//        {
//            Repo = oosRepo;
//            Vim = vimRepo;
//            Populate();
//        }


//        public void Compare()
//        {
//            UpdateList = new List<StoreDb>();
//            InsertList = new List<StoreDb>();
//            CloseList = new List<StoreDb>();

//            foreach (var store in VimStores)
//            {
//                if (store.PS_BU == "11103")
//                {
//                    _logger.Info("Notting hill status = {0}, name = {1}", store.status, store.STORE_NAME);
//                }


//                var status =
//                    StoreStatuses.FirstOrDefault(
//                        x => x.STATUS.Equals(store.status, StringComparison.CurrentCultureIgnoreCase));
//                var region =
//                    Regions.FirstOrDefault(
//                        x => x.REGION_ABBR.Equals(store.region, StringComparison.CurrentCultureIgnoreCase));

//                if (status != null && region != null)
//                {
//                    var sageStore = new StoreDb
//                    {
//                        CREATED_DATE = store.created_at,
//                        PS_BU = store.bu,
//                        STORE_ABBREVIATION = store.tlc,
//                        STORE_NAME = store.name,
//                        STATUS_ID = status.ID,
//                        REGION_ID = region.ID
//                    };

//                    var existing =
//                        Existing.FirstOrDefault(
//                            x => x.STORE_ABBREVIATION.Equals(store.tlc, StringComparison.CurrentCultureIgnoreCase));
//                    if (existing != null)
//                    {
//                        if (existing.STATUS_ID != sageStore.STATUS_ID || existing.STORE_NAME != sageStore.STORE_NAME)
//                        {
//                            sageStore.ID = existing.ID;
//                            UpdateList.Add(sageStore);
//                            UpdateCounter++;
//                        }
//                    }
//                    else
//                    {
//                        InsertList.Add(sageStore);
//                        InsertCounter++;
//                    }
//                }
//                else
//                {
//                    if (region == null)
//                    {
//                        _logger.Warn("{0} store had {1} as Region which was not found in OOS", store.tlc, store.region);
//                    }

//                    if (status == null)
//                    {
//                        _logger.Warn("{0} store had {1} as status which was not found in OOS", store.tlc, store.status);
//                    }
//                }
                
//            }

//            _logger.Info("Sage Store Count - {0}", ModifiedStores.Count);

//            var closedStatus =
//                StoreStatuses.FirstOrDefault(x => x.STATUS.Equals("CLOSED", StringComparison.CurrentCultureIgnoreCase));
//            if (closedStatus != null)
//            {
//                var closedId = closedStatus.ID;
//                foreach (var exists in Existing)
//                {
//                    var sagey = ModifiedStores.FirstOrDefault(x => x.bu == exists.PS_BU);
//                    if (sagey == null)
//                    {
//                        if (exists.STATUS_ID != closedId)
//                        {
//                            exists.STATUS_ID = closedId;
//                            UpdateList.Add(exists);
//                            UpdateCounter++;
//                        }
                        
//                    }
//                }
//            }
            
//        }


//        public void UpdateDatabase()
//        {
//            Repo.SaveNewStores(InsertList);
//            Repo.UpdateStores(UpdateList);
//        }

//        private void Populate()
//        {
//            VimStores = Vim.GetVimStores();
//            Existing = Repo.GetExistingStores();
//            StoreStatuses = Repo.GetStoreStatuses().ToList();
//            Regions = Repo.GetRegions().ToList();
//        }


        
        

        

//    }
//}
