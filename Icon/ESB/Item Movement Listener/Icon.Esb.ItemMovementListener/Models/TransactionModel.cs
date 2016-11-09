using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.ItemMovementListener.Models
{
    public class TransactionModel
    {
        public RetailTransactionModel RetailTransaction { get; set; }
        public string StoreNumber { get; set; }
        public string ESBMessageID { get; set; }
        public string RegisterNumber { get; set; }

        public string TransactionSequenceNumber { get; set; }

        public string CashierID { get; set; }        
    }
}
