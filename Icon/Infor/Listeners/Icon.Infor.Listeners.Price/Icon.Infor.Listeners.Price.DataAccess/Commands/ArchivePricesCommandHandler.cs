using FastMember;
using Icon.Infor.Listeners.Price.DataAccess.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Infor.Listeners.Price.DataAccess.Commands
{
    public class ArchivePricesCommandHandler : ICommandHandler<ArchivePricesCommand>
    {
        private IDbProvider db;

        public ArchivePricesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(ArchivePricesCommand data)
        {
            if (!data.Prices.Any())
            {
                return;
            }

            using (var reader = ObjectReader.Create(
                data.Prices,
                nameof(ArchivePriceModel.MessageArchivePriceID),
                nameof(ArchivePriceModel.EsbMessageID),
                nameof(ArchivePriceModel.GpmID),
                nameof(ArchivePriceModel.ItemID),
                nameof(ArchivePriceModel.BusinessUnitID),
                nameof(ArchivePriceModel.Region),
                nameof(ArchivePriceModel.PriceType),
                nameof(ArchivePriceModel.StartDate),
                nameof(ArchivePriceModel.MessageAction),
                nameof(ArchivePriceModel.JsonObject),
                nameof(ArchivePriceModel.ErrorCode),
                nameof(ArchivePriceModel.ErrorDetails),
                nameof(ArchivePriceModel.InsertDate)))
            {
                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(db.Connection as SqlConnection))
                {
                    sqlBulkCopy.DestinationTableName = "gpm.MessageArchivePrice";
                    sqlBulkCopy.WriteToServer(reader);
                }
            }
        }
    }
}
