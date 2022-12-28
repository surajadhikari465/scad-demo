using Icon.DbContextFactory;
using InventoryProducer.Producer.Model.DBModel;
using Irma.Framework;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using InventoryProducer.Common;

namespace InventoryProducer.Producer.DataAccess
{
    internal class RepublishInventoryDAL: IRepublishInventoryDAL
    {
        IDbContextFactory<IrmaContext> dbContextFactory;
        InventoryProducerSettings inventoryProducerSettings;

        public RepublishInventoryDAL(IDbContextFactory<IrmaContext> dbContextFactory, 
            InventoryProducerSettings inventoryProducerSettings)
        {
            this.dbContextFactory = dbContextFactory;
            this.inventoryProducerSettings = inventoryProducerSettings;
        }

        public IList<ArchivedMessageModel> GetUnsentMessages()
        {
            using(var irmaContext = dbContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = 30;
                var batchSize = new SqlParameter("@batchSize", System.Data.SqlDbType.BigInt) { Value = 50 };
                return irmaContext.Database.SqlQuery<ArchivedMessageModel>(
                    "exec [amz].[GetUnsentInStockMessages] @batchSize",
                    batchSize
                ).ToList();
            }
        }

        public void UpdateMessageArchiveWithError(int messageArchiveId, int processTimes, string errorDescription)
        {
            using (var irmaContext = dbContextFactory.CreateContext($"Irma_{inventoryProducerSettings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = 30;
                string updateMessageArchiveQuery = @"
                    UPDATE [amz].[MessageArchive]
                    SET Status='F',
                        ProcessTimes = @processTimes,
                        LastProcessedTime = getdate(),
                        ErrorDescription = @errorDescription
                    WHERE MessageArchiveID = @messageArchiveId
                ";
                irmaContext.Database.ExecuteSqlCommand(
                    updateMessageArchiveQuery,
                    new SqlParameter("@processTimes", processTimes),
                    new SqlParameter("@errorDescription", errorDescription),
                    new SqlParameter("@messageArchiveId", messageArchiveId)
                );
            }
        }
    }
}
