using System;

namespace MammothWebApi.DataAccess.Commands
{
    public class AddOrUpdatePricesCommand
    {
        public string Region { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid TransactionId { get; set; }
    }
}
