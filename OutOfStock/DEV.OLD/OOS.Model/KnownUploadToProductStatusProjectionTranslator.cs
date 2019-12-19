using System;
using System.Collections.Generic;
using System.Linq;
using OOSCommon;
using OOSCommon.Import;

namespace OOS.Model
{
    public class KnownUploadToProductStatusProjectionTranslator : ITranslateKnownUploadToProductStatusProjections
    {
        private OOSKnownItemData[] items;

        public IEnumerable<ProductStatus> Translate(IKnownUpload upload)
        {
            var result = new List<ProductStatus>();
            var maps = upload.VendorRegionMap;
            items = upload.ItemData;
            if (DoNotExistVendorRegionMap(maps) || DoesNotExistItemData(items)) return result;

            foreach (var map in maps)
            {
                var regionProjections = TranslateRegionProjections(map);
                result.AddRange(regionProjections);
            }
            return result;
        }

        private bool DoesNotExistItemData(IEnumerable<OOSKnownItemData> items)
        {
            return items == null || items.Count() == 0;
        }

        private bool DoNotExistVendorRegionMap(IEnumerable<OOSKnownVendorRegionMap> maps)
        {
            return maps == null || maps.Count() == 0;
        }

        private IEnumerable<ProductStatus> TranslateRegionProjections(OOSKnownVendorRegionMap map)
        {
            return (from item in items
                    let vendorIdentificationNumber = item.vin.ToString()
                    select new ProductStatus(map.region, map.vendor_key, vendorIdentificationNumber, item.name)).ToList();
        }
    }
}
