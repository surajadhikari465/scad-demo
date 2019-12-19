using System;
using System.Runtime.Serialization;

namespace OutOfStock.KnownUploadWebService
{
    [DataContract(Namespace = "http://schemas.wholefoods.com/knownupload")]
    public class KnownUploadDocument
    {
        [DataMember]
        public DateTime UploadDate { get; set; }

        [DataMember]
        public KnownItemData[] ItemData { get; set; }

        [DataMember]
        public KnownVendorRegionMap[] VendorRegionMap { get; set; }
    }
}