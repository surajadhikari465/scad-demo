using System;
using System.Runtime.Serialization;

namespace OutOfStock.KnownUploadWebService
{
    [DataContract(Namespace = "http://schemas.wholefoods.com/knownupload")]
    public class KnownItemData
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ReasonCode { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string Vin { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public int? Psbu { get; set; }
        [DataMember]
        public string TeamName { get; set; }
        [DataMember]
        public string SubteamName { get; set; }
    }

}