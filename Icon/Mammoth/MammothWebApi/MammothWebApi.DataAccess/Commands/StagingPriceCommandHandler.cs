using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data;
using System.Data.SqlClient;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingPriceCommandHandler : ICommandHandler<StagingPriceCommand>
    {
        private IDbProvider db;

        public StagingPriceCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(StagingPriceCommand data)
        {
            // Sql Bulk Copy List into Staging Table
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
            {
                bulkCopy.DestinationTableName = "stage.Price";
                DataTable dataTable = data.Prices.ToDataTable<StagingPriceModel>();
                bulkCopy.WriteToServer(dataTable);
            }
        }
    }
}
