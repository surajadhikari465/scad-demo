using System;

namespace Icon.Testing.CustomModels
{
    public class ItemMovementToIrma
    {
        public int ItemMovementID { get; set; }
        public string ESBMessageID { get; set; }
        public DateTime TransDate { get; set; }
        public int BusinessUnitId { get; set; }
        public string Identifier { get; set; }
        public int ItemType { get; set; }
        public bool? ItemVoid { get; set; }
        public int? Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal BasePrice { get; set; }
        public decimal? MarkdownAmount { get; set; }
    }
}
