using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using System;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class PublishItemUpdatesCommandHandler : ICommandHandler<PublishItemUpdatesCommand>
    {
        private IDbProvider db;

        public PublishItemUpdatesCommandHandler(IDbProvider db)
        {
            this.db = db;
        }

        public void Execute(PublishItemUpdatesCommand data)
        {
            var itemIds = db.Connection.Query<int>(@"
                SELECT ItemId
                FROM dbo.ScanCode sc
                JOIN @ScanCodes tvp ON sc.scanCode = tvp.ScanCode",
                new
                {
                    ScanCodes = data.ScanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType")
                }, transaction: this.db.Transaction);

            var intList = itemIds
                .Select(i => new { I = i })
                .ToDataTable();
            AddRegionalEvents(intList);

            var messageQueueItems = itemIds
                  .Select(i => new
                  {
                      ItemId = i,
                      EsbReadyDateTimeUtc = DateTime.UtcNow,
                      InsertDateUtc = DateTime.UtcNow
                  })
                  .ToDataTable();
            AddMessageQueueItems(messageQueueItems);
        }

        private void AddRegionalEvents(DataTable itemIds)
        {
            db.Connection.Execute(
                "app.GenerateItemUpdateEvents",
                new { ItemIds = itemIds.AsTableValuedParameter("app.IntList") },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }

        private void AddMessageQueueItems(DataTable messageQueueItems)
        {
            db.Connection.Execute(
                "esb.AddMessageQueueItem",
                new { @MessageQueueItems = messageQueueItems.AsTableValuedParameter("esb.MessageQueueItemIdsType") },
                commandType: CommandType.StoredProcedure,
                transaction: this.db.Transaction);
        }
    }
}