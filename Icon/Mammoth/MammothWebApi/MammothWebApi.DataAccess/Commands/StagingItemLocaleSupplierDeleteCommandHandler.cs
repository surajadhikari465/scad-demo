using FastMember;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MammothWebApi.DataAccess.Commands
{
    public class StagingItemLocaleSupplierDeleteCommandHandler : ICommandHandler<StagingItemLocaleSupplierDeleteCommand>
    {
        private IDbProvider db;

        public StagingItemLocaleSupplierDeleteCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(StagingItemLocaleSupplierDeleteCommand data)
        {
            if (data.ItemLocaleSupplierDeletes.Any())
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(this.db.Connection as SqlConnection))
                {
                    using (var reader = ObjectReader.Create(
                        data.ItemLocaleSupplierDeletes,
                        nameof(StagingItemLocaleSupplierDeleteModel.Region),
                        nameof(StagingItemLocaleSupplierDeleteModel.BusinessUnitID),
                        nameof(StagingItemLocaleSupplierDeleteModel.ScanCode),
                        nameof(StagingItemLocaleModel.Timestamp),
                        nameof(StagingItemLocaleModel.TransactionId)))
                    {
                        bulkCopy.DestinationTableName = "[stage].[ItemLocaleSupplierDelete]";
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }
    }
    }
