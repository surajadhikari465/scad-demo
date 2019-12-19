using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace OOS.Model
{
    public interface IScanOutOfStockItemService
    {
        string[] Validate(string[] upcs);
        bool Validate(string upc);
        bool CreateEventsFor(string storeName, string[] upc, DateTime scanDate);
        bool CreateEventFor(string storeName, string upc, DateTime scanDate);
        string[] RegionAbbreviations();
        string[] StoreAbbreviationsFor(string regionAbbrev);
        string[] RegionNames();
        string[] StoreNamesFor(string regionName);
        void ScanProducts(string regionName, string storeName, string[] upcs, DateTime scanDate);
        void ScanProductsByStoreAbbreviation(string regionAbbrev, string storeAbbrev, string[] upcs, DateTime scanDate);
    }
}
