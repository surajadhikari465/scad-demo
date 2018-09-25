using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.PrimeAffinityPsg.Tests
{
    internal class TestData
    {
        internal PrimeAffinityPsgModel Item_MA_10181_NonPrime_A = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10181,
            LocaleName = "Pittsburgh",
            ItemId = 234539,
            ScanCode = "85204300305",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = false
        };

        internal PrimeAffinityPsgModel Item_MA_10181_NonPrime_B = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10181,
            LocaleName = "Pittsburgh",
            ItemId = 1991072,
            ScanCode = "402082902466",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = false
        };

        internal PrimeAffinityPsgModel Item_MA_10181_Prime_C = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10181,
            LocaleName = "Pittsburgh",
            ItemId = 35979,
            ScanCode = "8425324048",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = true,
        };

        internal PrimeAffinityPsgModel Item_MA_10181_Prime_D = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10181,
            LocaleName = "Pittsburgh",
            ItemId = 4101369,
            ScanCode = "72822901554",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = true
        };

        internal PrimeAffinityPsgModel Item_FL_10130_NonPrime_A = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10130,
            LocaleName = "Boca Raton",
            ItemId = 297802,
            ScanCode = "89649900141",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = false
        };

        internal PrimeAffinityPsgModel Item_FL_10130_NonPrime_B = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10130,
            LocaleName = "Boca Raton",
            ItemId = 4060042,
            ScanCode = "66639283824",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = false
        };

        internal PrimeAffinityPsgModel Item_FL_10130_Prime_C = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10130,
            LocaleName = "Boca Raton",
            ItemId = 14098,
            ScanCode = "2999200100",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = true
        };

        internal PrimeAffinityPsgModel Item_FL_10130_Prime_D = new PrimeAffinityPsgModel
        {
            RegionCode = "MA",
            BusinessUnit = 10130,
            LocaleName = "Boca Raton",
            ItemId = 214828,
            ScanCode = "85107900254",
            ItemTypeCode = "RTL",
            ItemTypeDesc = "Retail Sale",
            PrimeEligible = true
        };
    }
}
