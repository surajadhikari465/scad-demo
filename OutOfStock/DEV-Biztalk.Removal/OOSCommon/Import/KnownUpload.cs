using System;
using System.Collections.Generic;
using OutOfStock.Messages;

namespace OOSCommon.Import
{
    public class KnownUpload : IKnownUpload
    {
        private List<OOSKnownItemData> items { get; set; }
        private List<OOSKnownVendorRegionMap> vendorMaps { get; set; }
        private DateTime uploadDate;
        private List<Event> events = new List<Event>();


        public OOSKnownItemData[] ItemData { get { return items.ToArray(); } }
        public OOSKnownVendorRegionMap[] VendorRegionMap { get { return vendorMaps.ToArray(); } }
        public DateTime UploadDate
        {
            get { return uploadDate; }
        }
        
        public KnownUpload(DateTime date) : this(new List<OOSKnownItemData>(), new List<OOSKnownVendorRegionMap>())
        {
            uploadDate = date;
        }

        private KnownUpload(List<OOSKnownItemData> itemData, List<OOSKnownVendorRegionMap> vendorRegionMap)
        {
            items = itemData;
            vendorMaps = vendorRegionMap;
        }


        public void AddItem(OOSKnownItemData oosKnownItemData)
        {
            items.Add(oosKnownItemData);
        }

        public void AddVendorRegion(OOSKnownVendorRegionMap oosKnownVendorRegionMap)
        {
            vendorMaps.Add(oosKnownVendorRegionMap);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var upload = obj as KnownUpload;
            return upload != null && Equals(upload);
        }

        public bool Equals(KnownUpload upload)
        {
            if (upload == null) return false;
            return uploadDate == upload.uploadDate;
        }

        public static bool operator == (KnownUpload leftSide, KnownUpload rightSide)
        {
            if (ReferenceEquals(leftSide, rightSide)) return true;
            if (((object)leftSide == null) || ((object)rightSide == null)) return false;

            return leftSide.Equals(rightSide);
        }

        public static bool operator != (KnownUpload leftSide, KnownUpload rightSide)
        {
            return !(leftSide == rightSide);
        }

        public override int GetHashCode()
        {
            return uploadDate.GetHashCode();
        }

        public IEnumerable<Event> GetChanges()
        {
            return events.ToArray();
        }

        public void AddEvent(Event emit)
        {
            events.Add(emit);
        }
    }
}
