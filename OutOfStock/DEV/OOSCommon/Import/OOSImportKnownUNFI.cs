using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OOSCommon.Import
{
    public class OOSImportKnownUNFI : IOOSImportKnown
    {
        public List<OOSCommon.Import.OOSKnownItemData> itemData { get; set; }
        public List<OOSCommon.Import.OOSKnownVendorRegionMap> vendorRegionMap { get; set; }

        public OOSImportKnownUNFI()
        {
        }

        public OOSImportKnownUNFI(XElement doc)
        {
            GetKnownOOSFromXML(doc);
        }

        public bool GetKnownOOSFromFile(string filePath)
        {
            XElement doc = XElement.Load(filePath);
            return GetKnownOOSFromXML(doc);
        }

        public bool GetKnownOOSFromContent(string fileContent)
        {
            XElement doc = XElement.Parse(fileContent);
            return GetKnownOOSFromXML(doc);
        }

        public bool GetKnownOOSFromXML(XElement doc)
        {
            bool isOk = true;
            itemData = new List<OOSCommon.Import.OOSKnownItemData>();
            vendorRegionMap = new List<OOSCommon.Import.OOSKnownVendorRegionMap>();
            this.itemData = doc
                .Elements("item_data")
                .Select(item => new OOSCommon.Import.OOSKnownItemData(item.Attribute("name").Value, item.Attribute("reason_code").Value,
                    item.Attribute("start_date").Value, item.Attribute("vin").Value)).ToList();
            this.vendorRegionMap = doc
                .Elements("vendor_region_map")
                .Select(item => new OOSCommon.Import.OOSKnownVendorRegionMap(item.Attribute("name").Value, item.Attribute("region").Value,
                    item.Attribute("vendor_key").Value)).ToList();
            return isOk;
        }

    }
}
