using OOS.Services.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using OOS.Services.DataModels;

namespace OOS.Services
{
  public   class StoreUpdater
    {
        public IOosRepo Repo;
        public IVimRepo Vim;

        public IEnumerable<StoreDb> Existing;
        public IEnumerable<StoreStatus> StoreStatuses;
        public IEnumerable<Region> Regions;

        public List<StoreDb> UpdateList;
        public List<StoreDb> InsertList;
        public List<StoreDb> CloseList;
        public List<VimStore> VimStores;

        public int UpdateCounter = 0;
        public int InsertCounter = 0;
        public int CloseCounter = 0;

        
       

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public StoreUpdater(IOosRepo oosRepo, IVimRepo vimRepo)
        {
            Repo = oosRepo;
            Vim = vimRepo;
            Populate();
        }

        private void Populate()
        {
            VimStores = Vim.GetVimStores();
            Existing = Repo.GetExistingStores();
            StoreStatuses = Repo.GetStoreStatuses().ToList();
            Regions = Repo.GetRegions().ToList();
        }


        public void Compare()
        {
            UpdateList = new List<StoreDb>();
            InsertList = new List<StoreDb>();
            CloseList = new List<StoreDb>();

            foreach (var store in VimStores)
            {
                var status =
                    StoreStatuses.FirstOrDefault(
                        x => x.STATUS.Equals(store.STATUS, StringComparison.CurrentCultureIgnoreCase));
                var region =
                    Regions.FirstOrDefault(
                        x => x.REGION_ABBR.Equals(store.REGION, StringComparison.CurrentCultureIgnoreCase));

                if (status != null && region != null)
                {
                    var sageStore = new StoreDb
                    {
                        CREATED_DATE = store.TIMESTAMP.ToShortDateString(),
                        PS_BU = store.PS_BU,
                        STORE_ABBREVIATION = store.STORE_ABBR,
                        STORE_NAME = store.STORE_NAME,
                        STATUS_ID = status.ID,
                        REGION_ID = region.ID
                    };

                    var existing =
                        Existing.FirstOrDefault(
                            x => x.STORE_ABBREVIATION.Equals(store.STORE_ABBR, StringComparison.CurrentCultureIgnoreCase));
                    if (existing != null)
                    {
                        if (existing.STATUS_ID != sageStore.STATUS_ID)
                        {
                            sageStore.ID = existing.ID;
                            UpdateList.Add(sageStore);
                            UpdateCounter++;
                        }
                    }
                    else
                    {
                        InsertList.Add(sageStore);
                        InsertCounter++;
                    }
                }
                else
                {
                    if (region == null)
                    {
                        Logger.Warn("{0} store had {1} as Region which was not found in OOS", store.STORE_ABBR, store.REGION);
                    }

                    if (status == null)
                    {
                        Logger.Warn("{0} store had {1} as status which was not found in OOS", store.STORE_ABBR, store.STATUS);
                    }
                }

            }

            Logger.Info("VIM Store Count - {0}", VimStores.Count);

            var closedStatus =
                StoreStatuses.FirstOrDefault(x => x.STATUS.Equals("CLOSED", StringComparison.CurrentCultureIgnoreCase));
            if (closedStatus != null)
            {
                var closedId = closedStatus.ID;
                foreach (var exists in Existing)
                {
                    var sagey = VimStores.FirstOrDefault(x => x.PS_BU == exists.PS_BU);
                    if (sagey == null)
                    {
                        if (exists.STATUS_ID != closedId)
                        {
                            exists.STATUS_ID = closedId;
                            UpdateList.Add(exists);
                            UpdateCounter++;
                        }

                    }
                }
            }

        }
        public void UpdateDatabase()
        {
            Repo.SaveNewStores(InsertList);
            Repo.UpdateStores(UpdateList);
        }

        public void GenerateReport()
        {
            foreach (var store in InsertList)
            {
                Console.WriteLine($"Createing New Store: [{store.PS_BU}] [{store.STATUS_ID}] {store.STORE_NAME}  ");
            }
            foreach (var store in UpdateList)
            {
                
                Console.WriteLine($"Updating Store: [{store.PS_BU}] [{store.STATUS_ID}] {store.STORE_NAME}  ");
            }
           
        }
    }
}
