using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class OOSCountSummary
    {
        private Dictionary<SummaryKey, int> summary = new Dictionary<SummaryKey, int>();

        public void Add(string store, string team, int count)
        {
            if (!summary.ContainsKey(new SummaryKey(store, team)))
                summary.Add(new SummaryKey(store, team), count);
        }

        public void Remove(string store, string team)
        {
            if (summary.ContainsKey(new SummaryKey(store, team)))
                summary.Remove(new SummaryKey(store, team));
        }

        public int Count()
        {
            return summary.Count;
        }

        public int For(string store, string team)
        {
            int count;
            return summary.TryGetValue(new SummaryKey(store, team), out count) ? count : 0;
        }

        public List<string> GetStores()
        {
            return summary.Keys.Select(key => key.Store).Distinct().ToList();
        }

        public OOSCountSummary Overlay(OOSCountSummary overlayCountSummary)
        {
            var keys = overlayCountSummary.Keys;
            OOSCountSummary copy = Copy();
            foreach (var key in keys.Where(key => summary.ContainsKey(key)))
            {
                copy.Remove(key.Store, key.Team);
                var count = overlayCountSummary.For(key.Store, key.Team);
                copy.Add(key.Store, key.Team, count);
            }
            return copy;
        }


        public Dictionary<SummaryKey, int>.KeyCollection Keys
        {
            get { return summary.Keys; }
        }

        private OOSCountSummary Copy()
        {
            var result = new OOSCountSummary();
            foreach (var key in Keys)
            {
                result.Add(key.Store, key.Team, summary[key]);
            }
            return result;
        }

    }
}
