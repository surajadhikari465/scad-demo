using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Models
{
    public partial class ItemStorePriceModel
    {
        public int ItemId { get; set; }
        public string ScanCode { get; set; }
        public int BusinessUnitID { get; set; }
        public bool Authorized { get; set; }
        public string BrandName { get; set; }
        public string ItemDescription { get; set; }
        public string PackageUnit { get; set; }
        public string RetailSize { get; set; }
        public string RetailUom { get; set; }
        public bool FoodStamp { get; set; }
        public string SubTeam { get; set; }
        public string SignDescription { get; set; }
        public string PriceType { get; set; }
        public int Multiple { get; set; }
        public decimal Price { get; set; }
        public string SellableUom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Currency { get; set; }
        public string PriceAttribute { get; set; }

    }
}
