﻿namespace IrmaMobile.Domain.Models
{
    public class TransferOrderItemModel
    {
        public decimal QuantityOrdered { get; set; }
        public int ItemKey { get; set; }
        public int QuantityUnit { get; set; }
        public decimal AdjustedCost { get; set; }
        public int ReasonCodeDetailId { get; set; }
    }
}
