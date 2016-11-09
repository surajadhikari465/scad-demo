using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class ItemType
    {
        public int ItemTypeID { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeDesc { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
