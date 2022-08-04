using Icon.DbContextFactory;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using Irma.Framework;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InventoryProducer.Common.InstockDequeue
{
    public class DequeueEvents
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings settings;
        private readonly RetryPolicy retrypolicy;

        public DequeueEvents(IDbContextFactory<IrmaContext> irmaContextFactory, InventoryProducerSettings settings)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.settings = settings;
            this.retrypolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                settings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(settings.DbRetryDelayInMilliseconds)
                );
        }

        public List<InstockDequeueModel> DequeueEventsFromIrma()
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{settings.RegionCode}"))
            {
                int affectedRowCount = 0;
                string storedProcedure = $"EXEC amz.{settings.InstockDequeueStoredProcedureName} @maxRecords, @lastRunDateTime, @affectedRowCount";
                DateTime nowWithOffset = DateTime.Now.AddMinutes(settings.DequeueMinuteOffset);
                irmaContext.Database.CommandTimeout = settings.DbCommandTimeoutInSeconds;
                List<InstockDequeueModel> dequeuedEvents = this.retrypolicy.Execute(() => ExecuteStoreProcedure(irmaContext, affectedRowCount, storedProcedure, nowWithOffset));
                return dequeuedEvents;
            }
        }

        private List<InstockDequeueModel> ExecuteStoreProcedure(IrmaContext irmaContext, int affectedRowCount, string storedProcedure, DateTime nowWithOffset)
        {
            return irmaContext.Database.SqlQuery<InstockDequeueModel>(
                storedProcedure,
                new SqlParameter("@maxRecords", settings.DequeueMaxRecords),
                new SqlParameter("@lastRunDateTime", nowWithOffset),
                new SqlParameter("@affectedRowCount", affectedRowCount)
                ).ToList();
        }
    }
}
