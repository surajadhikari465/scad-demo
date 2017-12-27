using FastMember;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingItemLocaleSupplierCommandHandler : ICommandHandler<StagingItemLocaleSupplierCommand>
    {
        private IDbProvider db;

        public StagingItemLocaleSupplierCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(StagingItemLocaleSupplierCommand data)
        {
            if (data.ItemLocaleSuppliers.Any())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
                {
                    using (var reader = ObjectReader.Create(
                        data.ItemLocaleSuppliers,
                        nameof(StagingItemLocaleSupplierModel.Region),
                        nameof(StagingItemLocaleSupplierModel.ScanCode),
                        nameof(StagingItemLocaleSupplierModel.BusinessUnitID),
                        nameof(StagingItemLocaleSupplierModel.SupplierName),
                        nameof(StagingItemLocaleSupplierModel.SupplierItemId),
                        nameof(StagingItemLocaleSupplierModel.SupplierCaseSize),
                        nameof(StagingItemLocaleSupplierModel.IrmaVendorKey),
                        nameof(StagingItemLocaleSupplierModel.Timestamp),
                        nameof(StagingItemLocaleSupplierModel.TransactionId)))
                    {
                        bulkCopy.DestinationTableName = "[stage].[ItemLocaleSupplier]";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
}
