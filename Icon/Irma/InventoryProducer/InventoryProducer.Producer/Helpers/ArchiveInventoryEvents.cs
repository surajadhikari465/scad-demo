using Icon.DbContextFactory;
using InventoryProducer.Common;
using Irma.Framework;
using System;
using System.Data.SqlClient;

namespace InventoryProducer.Producer.Helpers
{
    public class ArchiveInventoryEvents: IArchiveInventoryEvents
    {
        private readonly IDbContextFactory<IrmaContext> irmaContextFactory;
        private readonly InventoryProducerSettings settings;

        public ArchiveInventoryEvents(IDbContextFactory<IrmaContext> irmaContextFactory, InventoryProducerSettings settings)
        {
            this.irmaContextFactory = irmaContextFactory;
            this.settings = settings;
        }

        public void Archive(string messageBody, string eventType, int businessUnitId, int keyID, int secondaryKeyID, char status, string errorDescription, string messageNumber,string LastReprocessID)
        {
            using (var irmaContext = irmaContextFactory.CreateContext($"Irma_{settings.RegionCode}"))
            {
                irmaContext.Database.CommandTimeout = 10;
                string archiveSqlStatement = @"INSERT INTO amz.MessageArchive(Message, EventType, BusinessUnitID, KeyID, SecondaryKeyID, InsertDate, Status, ErrorDescription, MessageNumber, LastReprocessID) VALUES (@Message, @EventType, @BusinessUnitID, @KeyID, @SecondaryKeyID, @InsertDate, @Status, @ErrorDescription, @MessageNumber, @LastReprocessID)";
                irmaContext
                    .Database
                    .ExecuteSqlCommand(
                    archiveSqlStatement,
                    new SqlParameter("@Message", messageBody.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "")),
                    new SqlParameter("@EventType", eventType),
                    new SqlParameter("@BusinessUnitID", businessUnitId),
                    new SqlParameter("@KeyID", keyID),
                    new SqlParameter("@SecondaryKeyID", (object) secondaryKeyID ?? DBNull.Value),
                    new SqlParameter("@InsertDate", DateTime.Now),
                    new SqlParameter("@Status", status),
                    new SqlParameter("@ErrorDescription", (object) errorDescription ?? DBNull.Value),
                    new SqlParameter("@MessageNumber", messageNumber),
                    new SqlParameter("@LastReprocessID", (object) LastReprocessID ?? DBNull.Value)
                    );
            }
        }
    }
}
