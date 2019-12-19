using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public struct SummaryKey
    {
        private string store;
        private string team;

        public SummaryKey(string store, string team)
        {
            this.store = store;
            this.team = team;
        }

        public override bool Equals(object obj)
        {
            var key = (SummaryKey) obj;
            return key.store == store && key.team == team;
        }

        public override int GetHashCode()
        {
            int storeHash = (store == null) ? 0 : store.GetHashCode();
            int teamHash = (team == null) ? 0 : team.GetHashCode();
            return storeHash ^ teamHash;
        }

        public string Store
        {
            get { return string.Copy(store); }
        }

        public string Team
        {
            get { return string.Copy(team); }
        }

    }
}
