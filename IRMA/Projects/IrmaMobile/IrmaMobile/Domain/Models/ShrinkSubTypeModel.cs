using System;

namespace IrmaMobile.Domain.Models
{
    public class ShrinkSubTypeModel
    {
        public int ShrinkSubTypeId { get; set; }
        public string Abbreviation { get; set; }
        public int InventoryAdjustmentCodeId { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public int LastUpdateUserId  { get; set; }
        public string ReasonCode { get; set; }
        public string ShrinkSubTypeMember { get; set; }
        public string ShrinkType { get; set; }
    }
}
