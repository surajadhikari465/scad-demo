using System;

namespace Warp.ProcessPrices.Common
{
    public class PriceModel
    {

        /// <summary>
        /// 
        /// </summary>
        public int ItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>        
        public string Region { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BusinessUnitId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid GpmId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ScanCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal PercentOff { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PriceType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PriceTypeAttribute { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SellableUom { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CurrencyCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Multiple { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime TagExpirationDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime InsertDateUtc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifiedDateUtc { get; set; }
    }
}
