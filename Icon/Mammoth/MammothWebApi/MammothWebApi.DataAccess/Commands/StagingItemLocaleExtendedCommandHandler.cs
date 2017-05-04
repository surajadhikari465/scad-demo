using FastMember;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data.SqlClient;
using System.Linq;

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
            if (data.ItemLocalesExtended.Any())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
                {
                    using (var reader = ObjectReader.Create(
                        data.ItemLocalesExtended,
                        nameof(StagingItemLocaleExtendedModel.Region),
                        nameof(StagingItemLocaleExtendedModel.ScanCode),
                        nameof(StagingItemLocaleExtendedModel.BusinessUnitId),
                        nameof(StagingItemLocaleExtendedModel.AttributeId),
                        nameof(StagingItemLocaleExtendedModel.AttributeValue),
                        nameof(StagingItemLocaleExtendedModel.Timestamp),
                        nameof(StagingItemLocaleExtendedModel.TransactionId)))
                    {
                        bulkCopy.DestinationTableName = "[stage].[ItemLocaleExtended]";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
