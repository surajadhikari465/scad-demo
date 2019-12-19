using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.OOSCollector
{
    public class TurnOffStoreImportPolicy
    {
        private const string LAMAR_STORE_ABBREV = "LMR";
        private const string GATEWAY_STORE_ABBREV = "GWY";
        private const string BEECAVES_STORE_ABBREV = "BEE";


        private TurnOffStoreImportPolicy()
        {}

        public static bool ShouldTurnOff(string storeAbbreviation)
        {
            if (storeAbbreviation == LAMAR_STORE_ABBREV || storeAbbreviation == GATEWAY_STORE_ABBREV || storeAbbreviation == BEECAVES_STORE_ABBREV)
            {
                return true;
            }
            return false;

        }
    }
}
