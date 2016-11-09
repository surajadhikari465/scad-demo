using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Commands
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
            // Sql Bulk Copy List into Staging Table
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.dbProvider.Connection as SqlConnection))
            {
                bulkCopy.DestinationTableName = "mammoth.ChangeQueueHistory";
                DataTable dataTable = data.Events.ToDataTable();
                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
