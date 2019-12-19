using System;
using System.Collections.Generic;
using System.Linq;
using OOSCommon.Import;

namespace OutOfStock.KnownUploadWebService
{
    public class KnownUploadMapper : IKnownUploadMapper
    {
        public KnownUpload MapKnownUpload(KnownUploadDocument doc)
        {
            var upload = new KnownUpload(doc.UploadDate);
            MapItemData(doc.ItemData).ToList().ForEach(upload.AddItem);
            MapVendorRegionMap(doc.VendorRegionMap).ToList().ForEach(upload.AddVendorRegion);
            return upload;
        }

        private IEnumerable<OOSKnownItemData> MapItemData(IEnumerable<KnownItemData> itemData)
        {
            var result = new List<OOSKnownItemData>();
            itemData.ToList().ForEach(p => result.Add(new OOSKnownItemData(p.Name, p.ReasonCode, p.StartDate, p.Vin)));
            return result;
        }

        private IEnumerable<OOSKnownVendorRegionMap> MapVendorRegionMap(IEnumerable<KnownVendorRegionMap> regionMap)
        {
            var result = new List<OOSKnownVendorRegionMap>();
            regionMap.ToList().ForEach(p => result.Add(new OOSKnownVendorRegionMap(p.Name, p.Region, p.VendorKey)));
            return result;
        }
    }

}