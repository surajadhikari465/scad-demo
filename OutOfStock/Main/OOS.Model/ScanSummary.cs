using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class ScanSummary
    {
        private Dictionary<string, int> map = new Dictionary<string, int>();


        public void Add(string store, int scans)
        {
            if (!map.ContainsKey(store))
                map.Add(store, scans);
        }

        public void Remove(string store)
        {
            if (map.ContainsKey(store))
                map.Remove(store);
        }

        public int NumberOfScansFor(string store)
        {
            int scans;
            return (map.TryGetValue(store, out scans)) ? scans : 0;
        }
    }
}
