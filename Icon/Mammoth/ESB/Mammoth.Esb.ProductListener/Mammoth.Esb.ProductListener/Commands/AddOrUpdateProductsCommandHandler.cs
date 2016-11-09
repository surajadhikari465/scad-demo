using Dapper;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Commands
{
    public class AddOrUpdateProductsCommandHandler : ICommandHandler<AddOrUpdateProductsCommand>
    {
        IDbProvider db;

        public AddOrUpdateProductsCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(AddOrUpdateProductsCommand data)
        {
            var timeStamp = DateTime.Now;
            Guid transactionId = Guid.NewGuid();
            SetTimeStamp(data, timeStamp);
            CopyToStaging(data, transactionId);
            MergeIntoMammoth(data, transactionId);
            DeleteItemsFromStaging(transactionId);
        }

        private void SetTimeStamp(AddOrUpdateProductsCommand data, DateTime timestamp)
        {
            foreach (var product in data.Products)
            {
                product.Timestamp = timestamp;
            }
        }

        private void CopyToStaging(AddOrUpdateProductsCommand data, Guid transactionId)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                            db.Connection as SqlConnection,
                            SqlBulkCopyOptions.Default,
                            db.Transaction as SqlTransaction))
            {
                bulkCopy.DestinationTableName = "stage.Items";
                DataTable dataTable = data.Products.Select(
                    p => new
                    {
                        p.ItemID,
                        p.ItemTypeID,
                        p.ScanCode,
                        p.SubBrickID,
                        p.NationalClassID,
                        p.BrandHCID,
                        p.TaxClassHCID,
                        p.Desc_Product,
                        p.Desc_POS,
                        p.PackageUnit,
                        p.RetailSize,
                        p.RetailUOM,
                        p.PSNumber,
                        p.FoodStampEligible,
                        p.Timestamp,
                        transactionId
                    }).ToList().ToDataTable();

                bulkCopy.WriteToServer(dataTable);
            }
        }

        private void MergeIntoMammoth(AddOrUpdateProductsCommand data, Guid transactionId)
        {
            string sql = @"dbo.AddOrUpdateItems_FromStaging";
            int rowCount = this.db.Connection.Execute(sql,
                new { transactionId = transactionId },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }

        private void DeleteItemsFromStaging(Guid transactionId)
        {
            string sql = @"DELETE FROM stage.Items WHERE TransactionId = @transactionId";
            db.Connection.Execute(sql, new { transactionId = transactionId }, db.Transaction);
        }
    }
}