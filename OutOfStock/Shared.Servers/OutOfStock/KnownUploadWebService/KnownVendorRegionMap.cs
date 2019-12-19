using System.Runtime.Serialization;

namespace OutOfStock.KnownUploadWebService
{
    [DataContract(Namespace = "http://schemas.wholefoods.com/knownupload")]
    public class KnownVendorRegionMap
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Region { get; set; }
        [DataMember]
        public string VendorKey { get; set; }

    }

}