using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Commands
{
    public class DeletePricesCommand
    {
        public DateTime Timestamp { get; set; }
        public string Region { get; set; }
        public Guid TransactionId { get; set; }
    }
}
