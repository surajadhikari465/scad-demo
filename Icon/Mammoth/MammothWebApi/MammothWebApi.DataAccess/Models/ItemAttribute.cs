using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class ItemAttribute
    {
        public int ItemAttributeID { get; set; }
        public int ItemID { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public virtual Item Item { get; set; }
    }
}
