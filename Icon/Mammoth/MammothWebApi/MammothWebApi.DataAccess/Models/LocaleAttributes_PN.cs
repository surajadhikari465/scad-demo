using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class LocaleAttributes_PN
    {
        public string Region { get; set; }
        public int LocaleAttributeID { get; set; }
        public Nullable<int> AttributeID { get; set; }
        public string AttributeValue { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
