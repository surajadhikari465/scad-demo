using System;
using System.Collections.Generic;
using OOSCommon.Import;

namespace OOS.Model.Commands
{
    public class UploadKnownUploadCommand
    {
        public IEnumerable<OOSKnownVendorRegionMap> VendorRegionMaps { get; private set; }
        public IEnumerable<OOSKnownItemData> Items { get; private set; }
        public DateTime UploadDate { get; private set; }

        public UploadKnownUploadCommand(DateTime uploadDate, IEnumerable<OOSKnownItemData> itemData, IEnumerable<OOSKnownVendorRegionMap> regionMap)
        {
            UploadDate = uploadDate;
            Items = itemData;
            VendorRegionMaps = regionMap;
        }
    }
}
