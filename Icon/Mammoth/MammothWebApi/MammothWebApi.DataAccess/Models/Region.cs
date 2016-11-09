using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Region
    {
        public byte regionID { get; set; }
        public string Region1 { get; set; }
        public string RegionName { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
