using System;

namespace MammothWebApi.DataAccess.Models
{
    public partial class Prices
    {
        public string Region { get; set; }
        public int ItemID { get; set; }
        public int BusinessUnitID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public string PriceType { get; set; }
        public string PriceUOM { get; set; }
        public int CurrencyID { get; set; }
        public int Multiple { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
