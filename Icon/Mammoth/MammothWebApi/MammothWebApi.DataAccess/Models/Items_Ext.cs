using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Items_Ext
    {
        public int ItemAttributeID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual Item Item { get; set; }
    }
}
