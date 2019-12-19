using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class OOSSummary
    {
        private OOSCountSummary countSummary;
        private SKUSummary skuSummary;
        private ScanSummary scanSummary;

        public OOSSummary(OOSCountSummary countSummary, SKUSummary skuSummary, ScanSummary scanSummary)
        {
            this.countSummary = countSummary;
            this.skuSummary = skuSummary;
            this.scanSummary = scanSummary;
        }

        public int NumberOfScansFor(string store)
        {
            return scanSummary.NumberOfScansFor(store);
        }

        public int OOSCountFor(string store, string team)
        {
            return countSummary.For(store, team);
        }

        public int NumberOfSKUsFor(string store, string team)
        {
            return skuSummary.For(store, team);
        }

        public List<string> GetStores()
        {
            return countSummary.GetStores();
        }
    }
}
