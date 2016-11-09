using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data;
using System.Data.SqlClient;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingItemLocaleCommandHandler : ICommandHandler<StagingItemLocaleCommand>
    {
        private IDbProvider db;

        public StagingItemLocaleCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(StagingItemLocaleCommand data)
        {
            // Sql Bulk Copy List into Staging Table
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                bulkCopy.DestinationTableName = "[stage].[ItemLocale]";
                DataTable dataTable = data.ItemLocales.ToDataTable<StagingItemLocaleModel>();
                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
