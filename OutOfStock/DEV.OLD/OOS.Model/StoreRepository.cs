using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private IOOSEntitiesFactory dbFactory;
        private IUserProfile userProfile;
        private IConfigurator config;

        public StoreRepository(IConfigurator config, IOOSEntitiesFactory dbFactory, IUserProfile userProfile)
        {
            this.config = config;
            this.dbFactory = dbFactory;
            this.userProfile = userProfile;
        }

        public Store ForAbbrev(string storeAbbrev)
        {
            using (var db = dbFactory.New())
            {
                var storeList = (from s in db.STORE join ss in db.STATUS on s.STATUS_ID equals ss.ID 
                                 where s.STORE_ABBREVIATION == storeAbbrev
                                 select new { Id = s.ID, Name = s.STORE_NAME, Abbrev = s.STORE_ABBREVIATION, Status = ss.STATUS1, BusinessUnitNo = s.PS_BU }).ToList();
                var stores = storeList.Select(store => new Store(store.Id) { Name = store.Name, Abbrev = store.Abbrev, Status = store.Status, BusinessUnitNo = store.BusinessUnitNo}).ToList();
                return stores.FirstOrDefault();
            }
        }

        public Store ForName(string storeName)
        {
            var storeAbbrev = StoreAbbreviationFor(storeName);
            return ForAbbrev(storeAbbrev);
        }

        private string StoreAbbreviationFor(string storeName)
        {
            var storeWithName = GetStoreNameAbbreviations().Where(s => s.Key == storeName);
            return storeWithName.Any() ? storeWithName.First().Value : string.Empty;
        }

        private IEnumerable<KeyValuePair<string, string>> GetStoreNameAbbreviations()
        {
            using (var dbContext = dbFactory.New())
            {
                var stores = (from r in dbContext.STORE orderby r.STORE_ABBREVIATION select new { Key = r.STORE_NAME.Trim(), Value = r.STORE_ABBREVIATION.Trim() }).ToList();
                return stores.Select(r => new KeyValuePair<string, string>(r.Key, r.Value));
            }
        }

        public void Add(Store store)
        {
            if (store == null) return;

            var storeDB = NewStoreDBFrom(store);
            if (storeDB == null) return;

            SaveStoreDB(storeDB);
        }


        private STORE NewStoreDBFrom(Store store)
        {
            var currentDate = DateTime.Now;
            var userName = userProfile.UserName;
            var storeId = FindPSBusinessUnitNumber(store.Abbrev);
            var regionId = store.RegionId;
            var statusId = FindStatusId(store.Status);
            if (storeId == null || regionId == 0 || statusId == 0) return null;

            var storeDB = new STORE
            {
                CREATED_BY = userName,
                PS_BU = storeId,
                REGION_ID = regionId,
                CREATED_DATE = currentDate,
                LAST_UPDATED_BY = userName,
                LAST_UPDATED_DATE = currentDate,
                STATUS_ID = statusId,
                STORE_ABBREVIATION = store.Abbrev,
                STORE_NAME = store.Name
            };
            return storeDB;
        }

        internal string FindPSBusinessUnitNumber(string storeAbbrev)
        {
            var vimServiceName = config.GetVIMServiceName();
            var sql = GetPeopleSoftBusinessUnitNumberSQL();
            var query = string.Format(sql, vimServiceName, storeAbbrev);
            return ExecutePSBusinessUnitNumber(query).FirstOrDefault();
        }

        private IEnumerable<string> ExecutePSBusinessUnitNumber(string query)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var storeNumbers = oosDataContext.ExecuteQuery<string>(query, new object[] { });
                return storeNumbers.ToList();
            }
        }
        
        private string GetPeopleSoftBusinessUnitNumberSQL()
        {
            return "SELECT PS_BU FROM OPENQUERY({0}, 'SELECT PS_BU from vim.store WHERE STORE_ABBR=''{1}''')";
        }

        internal int FindStatusId(string status)
        {
            var sql = GetStatusIDSQL();
            var query = string.Format(sql, status);
            var id = ExecuteFindEnumerableInts(query).FirstOrDefault();
            return id;
        }

        private string GetStatusIDSQL()
        {
            return "select ID from STATUS where STATUS='{0}'";
        }

        private IEnumerable<int> ExecuteFindEnumerableInts(string query)
        {
            using (var oosDataContext = new System.Data.Linq.DataContext(config.GetOOSConnectionString()))
            {
                var status = oosDataContext.ExecuteQuery<int>(query, new object[] { });
                return status.ToList();
            }
        }

        private void SaveStoreDB(STORE store)
        {
            using (var dbContext = dbFactory.New())
            {
                dbContext.STORE.AddObject(store);
                dbContext.SaveChanges();
            }
        }

        public void Update(Store store)
        {
            if (store == null) return;

            UpdateDB(store);
        }

        private void UpdateDB(Store store)
        {
            var statusId = FindStatusId(store.Status);
            if (statusId == 0) return;

            using (var dbContext = dbFactory.New())
            {
                var storeDB = dbContext.STORE.Where(p => p.STORE_ABBREVIATION == store.Abbrev).FirstOrDefault();
                if (storeDB == null) return;

                storeDB.STATUS_ID = statusId;
                storeDB.STORE_NAME = store.Name;
                dbContext.SaveChanges();
            }
        }
    }
}
