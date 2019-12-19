using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace OOS.Model
{
    public class UploadMessage
    {
        private static readonly XNamespace uploadNamespace = "http://tempuri.org/";
        private static readonly XNamespace uploadStringNamespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";

        private readonly List<string> upcs = new List<string>();
        public string RegionAbbreviation { get; private set; }
        public string StoreAbbreviation { get; private set; }
        public DateTime ScanDate { get; private set; }
        public IEnumerable<string> Scans { get { return upcs.ToArray(); } }
        
        private UploadMessage(DateTime scanDate, string region, string store)
        {
            ScanDate = scanDate;
            RegionAbbreviation = region;
            StoreAbbreviation = store;
        }

        public static UploadMessage From(string xml)
        {
            var doc = XElement.Parse(xml);
            var scanDate = ScanDateFrom(doc);
            var region = RegionFrom(doc);
            var store = StoreFrom(doc);
            var upload = new UploadMessage(scanDate, region, store);
            UpcsFrom(doc).ForEach(upload.Add);
            return upload;
        }

        private void Add(string upc)
        {
            upcs.Add(upc);
        }

        private static DateTime ScanDateFrom(XElement doc)
        {
            var scanDateElement = doc.Element(uploadNamespace + "scanDate");
            if (scanDateElement == null) throw new XmlSchemaValidationException("Bad scan date");
            return Convert.ToDateTime(scanDateElement.Value);
        }

        private static string RegionFrom(XElement doc)
        {
            var regionElement = doc.Element(uploadNamespace + "regionAbbrev");
            if (regionElement == null) throw new XmlSchemaValidationException("Bad region abbreviation");
            return regionElement.Value;
        }

        private static string StoreFrom(XElement doc)
        {
            var storeElement = doc.Element(uploadNamespace + "storeAbbrev");
            if (storeElement == null) throw new XmlSchemaValidationException("Bad store abbreviation");
            return storeElement.Value;
        }

        private static List<string> UpcsFrom(XElement doc)
        {
            var list = new List<string>();
            var upcNodes = doc.Element(uploadNamespace + "upcs");
            if (upcNodes != null)
            {
                upcNodes.Elements(uploadStringNamespace + "string").ToList().ForEach(p => list.Add(p.Value));
            }
            return list;
        }
    }
}
