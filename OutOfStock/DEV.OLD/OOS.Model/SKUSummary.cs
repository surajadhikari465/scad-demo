using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class SKUSummary
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

        public List<SummaryKey> KeyList()
        {
            return summary.Keys.Select(p => new SummaryKey(p.Store, p.Team)).ToList();
        }
    }
}
