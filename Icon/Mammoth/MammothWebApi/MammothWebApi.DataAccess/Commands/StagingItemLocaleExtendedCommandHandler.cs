using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data;
using System.Data.SqlClient;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingItemLocaleExtendedCommandHandler : ICommandHandler<StagingItemLocaleExtendedCommand>
    {
        private IDbProvider db;

        public StagingItemLocaleExtendedCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(StagingItemLocaleExtendedCommand data)
        {
            // Sql Bulk Copy List into Staging Table
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                bulkCopy.DestinationTableName = "[stage].[ItemLocaleExtended]";
                DataTable dataTable = data.ItemLocalesExtended.ToDataTable<StagingItemLocaleExtendedModel>();
                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
