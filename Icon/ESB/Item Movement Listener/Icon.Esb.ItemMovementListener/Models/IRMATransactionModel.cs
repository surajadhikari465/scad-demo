using System;

namespace Icon.Esb.ItemMovementListener.Models
{
    public class IRMATransactionModel
    {
        public string ESBMessageID { get; set; } // ESB generated unique ID
        public int BusinessUnitId { get; set; }        
        public int RegisterNumber { get; set; }
        public int TransactionSequenceNumber { get; set; } // Transaction sequence number for a register
        public int LineItemNumber { get; set; } 
        public string Identifier { get; set; } // Scancode     
        public DateTime TransDate { get; set; } 
        public int Quantity { get; set; }
        public bool ItemVoid { get; set; } // Y or N
        public int ItemType { get; set; } // 0 = Retail Sale, 2 = Refund
        public decimal BasePrice { get; set; }
        public decimal? Weight { get; set; }
        public decimal? MarkDownAmount { get; set; }
        
    }
}
