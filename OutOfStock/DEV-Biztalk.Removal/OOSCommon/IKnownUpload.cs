using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon.Import;

namespace OOSCommon
{
    public interface IKnownUpload
    {
        DateTime UploadDate { get; }
        void AddItem(OOSKnownItemData item);
        void AddVendorRegion(OOSKnownVendorRegionMap map);
        OOSKnownItemData[] ItemData { get; }
        OOSKnownVendorRegionMap[] VendorRegionMap { get; }
    }
}
