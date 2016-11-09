using Icon.Esb.ItemMovementListener.Models;
using System.Collections.Generic;

namespace Icon.Esb.ItemMovementListener.Commands
{
    public class SaveItemMovementTransactionCommand 
    {
        public List<IRMATransactionModel> ItemMovementTransactions { get; set; }
    }
}
