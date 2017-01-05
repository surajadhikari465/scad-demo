using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Models;
using MoreLinq;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Price.Controller.DataAccess.Commands
{
    public class ArchiveEventsCommandHandler : ICommandHandler<ArchiveEventsCommand>
    {
        private IDbProvider dbProvider;

        public ArchiveEventsCommandHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }
        
        public void Execute(ArchiveEventsCommand data)
        {
            if (data.Events.Any())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.dbProvider.Connection as SqlConnection))
                {
                    bulkCopy.DestinationTableName = "mammoth.ChangeQueueHistory";
                    DataTable dataTable = data.Events.ToDataTable();
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }
    }
}
