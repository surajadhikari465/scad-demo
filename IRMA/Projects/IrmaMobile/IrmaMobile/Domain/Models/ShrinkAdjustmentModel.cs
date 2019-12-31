using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IrmaMobile.Domain.Models
{
    public class ShrinkAdjustmentModel
    {
        public int ItemKey { get; set; }
        public int StoreNo { get; set; }
        public int SubteamNo { get; set; }
        public int ShrinkSubTypeId { get; set; }
        public int AdjustmentId { get; set; }
        public string AdjustmentReason { get; set; }
        public int CreatedByUserId { get; set; }
        public string UserName { get; set; }
        public string InventoryAdjustmentCodeAbbreviation { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
    }
}
