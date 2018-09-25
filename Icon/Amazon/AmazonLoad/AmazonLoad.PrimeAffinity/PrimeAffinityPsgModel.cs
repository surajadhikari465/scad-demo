using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonLoad.PrimeAffinityPsg
{
    internal class PrimeAffinityPsgModel
    {
        public string RegionCode { get; set; }
        public int BusinessUnit { get; set; }
        public string LocaleName { get; set; }
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public bool PrimeEligible { get; set; }
    }
}
