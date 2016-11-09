using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Commands
{
    public class DeleteStagingCommand
    {
        public string StagingTableName { get; set; }
        public Guid TransactionId { get; set; }
    }
}
