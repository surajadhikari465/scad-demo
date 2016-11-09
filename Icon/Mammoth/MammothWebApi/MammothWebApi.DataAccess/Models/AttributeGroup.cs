using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class AttributeGroup
    {
        public int AttributeGroupID { get; set; }
        public string AttributeGroupCode { get; set; }
        public string AttributeGroupDesc { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
