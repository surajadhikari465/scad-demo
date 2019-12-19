using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OOSCommon.Import
{
    public interface IOOSImportKnown
    {
        List<OOSKnownItemData> itemData { get; set; }
        List<OOSKnownVendorRegionMap> vendorRegionMap { get; set; }

        bool GetKnownOOSFromFile(string filePath);
        bool GetKnownOOSFromContent(string fileContent);
        bool GetKnownOOSFromXML(XElement doc);

    }

    public class OOSKnownItemData
    {
        public string name { get; set; }
        public eReasons reason_code { get; set; }
        public DateTime start_date { get; set; }
        public int vin { get; set; }
        public DateTime? end_date { get; set; }
        public int? ps_bu { get; set; }
        public string team_name { get; set; }
        public string subteam_name { get; set; }

        public DateTime? ExpirationDate { get; private set; }
        public string ProductStatus { get; private set; }

        public OOSKnownItemData(string name, string reason_code, string start_date, string vin)
        {
            this.name = name;
            this.reason_code = eReasons.None;
            {
                int iVal;
                if (int.TryParse(reason_code, out iVal))
                {
                    if (iVal >= 0 && iVal <= 6)
                        this.reason_code = (eReasons)iVal;
                }
            }
            this.start_date = DateTime.Now;
            {
                DateTime dtVal;
                if (DateTime.TryParse(start_date, out dtVal))
                    this.start_date = dtVal;
            }
            this.vin = 0;
            {
                int iVal;
                if (int.TryParse(vin, out iVal))
                    this.vin = iVal;
            }
        }

        public OOSKnownItemData(string name, string reasonCode, string startDate, string vinNumber, string productStatus, DateTime? expirationDate) :
            this(name, reasonCode, startDate, vinNumber)
        {
            ProductStatus =  productStatus;
            ExpirationDate = expirationDate;
        }

    }

    public class OOSKnownVendorRegionMap
    {
        public string name { get; set; }
        public string region { get; set; }
        public string vendor_key { get; set; }

        public OOSKnownVendorRegionMap(string name, string region, string vendor_key)
        {
            this.name = name;
            this.region = region;
            this.vendor_key = vendor_key;
        }
    }

}
