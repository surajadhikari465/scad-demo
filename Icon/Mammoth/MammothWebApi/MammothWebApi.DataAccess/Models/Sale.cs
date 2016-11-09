using System;
using System.Collections.Generic;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Sale
    {
        public string Region { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public decimal Price { get; set; }
        public string PriceType { get; set; }
        public byte Multiple { get; set; }
        public System.DateTime AddedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
