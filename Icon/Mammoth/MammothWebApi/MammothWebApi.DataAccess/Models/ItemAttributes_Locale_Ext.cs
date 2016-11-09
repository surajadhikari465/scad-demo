using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class ItemAttributes_Locale_Ext
    {
        public string Region { get; set; }
        public int ItemAttributeLocaleID { get; set; }
        public int ItemID { get; set; }
        public int LocaleID { get; set; }
        public Nullable<int> AttributeID { get; set; }
        public string AttributeValue { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public virtual Item Item { get; set; }
    }
}
