using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model.Commands
{
    public class UpdateStoreSkuCommand
    {
        public UpdateStoreSkuCommand(string storeAbbrev, string team, int count)
        {
            StoreAbbreviation = storeAbbrev;
            Team = team;
            SKUCount = count;
        }

        public string StoreAbbreviation { get; private set; }
        public string Team { get; private set; }
        public int SKUCount { get; private set; }
    }
}
