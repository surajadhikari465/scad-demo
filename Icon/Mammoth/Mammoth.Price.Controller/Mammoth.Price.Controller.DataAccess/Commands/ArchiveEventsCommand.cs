using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mammoth.Price.Controller.DataAccess.Commands
{
    public class ArchiveEventsCommand
    {
        public IEnumerable<ChangeQueueHistoryModel> Events { get; set; }
    }
}
