using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOS.Services.DAL;
using OOS.Services.DataModels;

namespace OOS.Services.Tests.Fakes
{
    public class FakeOosRepo : IOosRepo
    {

        public List<StoreDb> TestStores;

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed) { if (disposing) { } } this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<StoreDb> GetExistingStores()
        {
            return new List<StoreDb>
            {
                new StoreDb { STATUS_ID = 1, STORE_NAME = "astronautilus", STORE_ABBREVIATION = "AAA"},
                new StoreDb { STATUS_ID = 1, STORE_NAME = "badAstronaut", STORE_ABBREVIATION = "BBB"},
                new StoreDb { STATUS_ID = 1, STORE_NAME = "cantankerousCoot", STORE_ABBREVIATION = "CCC"},
                new StoreDb { STATUS_ID = 1, STORE_NAME = "doooooooooomed", STORE_ABBREVIATION = "DDD"},
            };
        }

        public IEnumerable<StoreStatus> GetStoreStatuses()
        {
            return new List<StoreStatus>
            {
                new StoreStatus {ID = 1, STATUS = "OPEN"},
                new StoreStatus {ID = 2, STATUS = "CLOSED"},
                new StoreStatus {ID = 3, STATUS = "NEW"},
                new StoreStatus {ID = 4, STATUS = "SOON"},
            };
        }

        public IEnumerable<Region> GetRegions()
        {
            return new List<Region>
            {
                new Region {ID = 1, REGION_ABBR = "AA"},
                new Region {ID = 2, REGION_ABBR = "BB"},
                new Region {ID = 3, REGION_ABBR = "CC"},
                new Region {ID = 4, REGION_ABBR = "DD"},
                new Region {ID = 5, REGION_ABBR = "EE"}
            };
        }

        public void SaveNewStores(List<StoreDb> stores)
        {

            TestStores = stores;

        }

        public void UpdateStores(List<StoreDb> stores)
        {
            throw new NotImplementedException();
        }
    }
}
