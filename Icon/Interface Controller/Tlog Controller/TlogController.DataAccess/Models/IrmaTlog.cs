using Irma.Framework;
using System.Collections.Generic;

namespace TlogController.DataAccess.Models
{
    public class IrmaTlog
    {
        public string RegionCode { get; set; }
        public List<ItemMovementToIrma> ItemMovementToIrmaList { get; set; }
        public List<TlogReprocessRequest> TlogReprocessRequestList { get; set; }
        public List<ItemMovementTransaction> ItemMovementTransactionList { get; set; } //Group ItemMovementToIrma data into transactions
    }
}
