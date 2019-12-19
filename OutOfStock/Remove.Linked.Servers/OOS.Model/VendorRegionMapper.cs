using System;
using System.Collections.Generic;
using System.Linq;
using OOSCommon.Import;
using OutOfStock.Messages;

namespace OOS.Model
{
    public class VendorRegionMapper
    {
        public static IEnumerable<KnownUploadVendorRegion> ToKnownUploadVendorRegions(IEnumerable<OOSKnownVendorRegionMap> vendorRegionMaps)
        {
            return vendorRegionMaps.Select(ToKnownUploadVendorRegion).ToList();
        }

        public static KnownUploadVendorRegion ToKnownUploadVendorRegion(OOSKnownVendorRegionMap vendorRegionMap)
        {
            return new KnownUploadVendorRegion { Region = vendorRegionMap.region, Vendor = vendorRegionMap.vendor_key, };
        }

        public static IEnumerable<OOSKnownVendorRegionMap> ToOOSKnownVendorRegionMaps(IEnumerable<KnownUploadVendorRegion> vendorRegionMaps)
        {
            return vendorRegionMaps.Select(ToOOSKnownVendorRegionMap).ToList();
        }

        public static OOSKnownVendorRegionMap ToOOSKnownVendorRegionMap(KnownUploadVendorRegion vendorRegion)
        {
            return new OOSKnownVendorRegionMap(vendorRegion.Vendor+"-"+vendorRegion.Region, vendorRegion.Region, vendorRegion.Vendor);
        }

    }
}
