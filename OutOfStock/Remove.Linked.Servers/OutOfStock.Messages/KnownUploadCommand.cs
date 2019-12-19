using System;
using System.Collections.Generic;

namespace OutOfStock.Messages
{
    [Serializable]
    public class KnownUploadCommand
    {
        public IEnumerable<KnownUploadVendorRegion> VendorRegionMaps { get; set; }
        public IEnumerable<KnownUploadItem> Items { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
