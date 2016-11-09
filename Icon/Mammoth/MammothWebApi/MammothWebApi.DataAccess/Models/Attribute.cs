using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Attribute
    {
        public int AttributeID { get; set; }
        public Nullable<int> AttributeGroupID { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeDesc { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
