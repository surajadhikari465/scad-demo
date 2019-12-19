using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOS.Model.Repository
{
    public class SQLStoreFactory
    {
        public static List<Store> Reconstitute(List<STORE> openStoresDb)
        {
            return openStoresDb.Select(store => new Store(store.ID) {Name = store.STORE_NAME, Abbrev = store.STORE_ABBREVIATION, RegionId = store.REGION_ID, BusinessUnitNo = store.PS_BU}).ToList();
        }
    }
}
