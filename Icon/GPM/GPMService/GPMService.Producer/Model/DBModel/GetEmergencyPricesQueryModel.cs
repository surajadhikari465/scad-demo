using System;

namespace GPMService.Producer.Model.DBModel
{
    internal class GetEmergencyPricesQueryModel
    {
        public int Itemid { get; set; }
        public int BusinessUnitId { get; set; }
        public string PriceType { get; set; }
        public string MammothPriceXml { get; set; }
        public DateTime InsertDateUtc { get; set; }
    }
}
