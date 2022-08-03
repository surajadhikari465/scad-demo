using Icon.DbContextFactory;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace InventoryProducer.Common.InstockDequeue
{
    public class DequeueEvents
    {
        private IDbContextFactory<IrmaContext> irmaContextFactory;
        private InventoryProducerSettings settings;

        public DequeueEvents(IDbContextFactory<IrmaContext> irmaContextFactory, InventoryProducerSettings settings)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.settings = settings;
        }

        public List<InstockDequeueModel> DequeueEventsFromIrma()
        {
            using (var irmaContext = irmaContextFactory.CreateContext("Irma_" + settings.RegionCode))
            {
                int affectedRowCount = 0;
                string storedProcedure = @"EXEC amz." + settings.InstockDequeueStoredProcedureName + " @maxRecords, @lastRunDateTime, @affectedRowCount";
                DateTime nowWithOffset = DateTime.Now.AddMinutes(settings.DequeueMinuteOffset);
                irmaContext.Database.CommandTimeout = settings.DbCommandTimeoutInSeconds;
                List<InstockDequeueModel> dequeuedEvents = irmaContext.Database.SqlQuery<InstockDequeueModel>(
                    storedProcedure, 
                    new SqlParameter("@maxRecords", settings.DequeueMaxRecords),
                    new SqlParameter("@lastRunDateTime", nowWithOffset),
                    new SqlParameter("@affectedRowCount", affectedRowCount)
                    ).ToList();
                return dequeuedEvents;
            }
        }
    }
}
