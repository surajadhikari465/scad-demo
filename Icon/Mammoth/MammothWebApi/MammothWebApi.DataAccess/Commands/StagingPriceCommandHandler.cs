using FastMember;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data.SqlClient;
using System.Linq;

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
            if (data.Prices.Any())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
                {
                    using (var reader = ObjectReader.Create(
                        data.Prices,
                        nameof(StagingPriceModel.Region),
                        nameof(StagingPriceModel.ScanCode),
                        nameof(StagingPriceModel.BusinessUnitId),
                        nameof(StagingPriceModel.Multiple),
                        nameof(StagingPriceModel.Price),
                        nameof(StagingPriceModel.PriceType),
                        nameof(StagingPriceModel.StartDate),
                        nameof(StagingPriceModel.EndDate),
                        nameof(StagingPriceModel.PriceUom),
                        nameof(StagingPriceModel.CurrencyCode),
                        nameof(StagingPriceModel.Timestamp),
                        nameof(StagingPriceModel.TransactionId)))
                    {
                        bulkCopy.DestinationTableName = "stage.Price";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
