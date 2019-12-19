using System;
using System.Data;
using System.Linq;
using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Commands
{
    public class PublishItemUpdatesCommandHandler : ICommandHandler<PublishItemUpdatesCommand>
    {
        private readonly IDbConnection DbConnection;

        public PublishItemUpdatesCommandHandler(IDbConnection dbConnection)
        {
            this.DbConnection = dbConnection;
        }

        public void Execute(PublishItemUpdatesCommand data)
        {
            var itemIds = DbConnection.Query<int>(@"
                SELECT ItemId
                FROM dbo.ScanCode sc
                JOIN @ScanCodes tvp ON sc.scanCode = tvp.ScanCode",
                new
                {
                    ScanCodes = data.ScanCodes
                        .Select(sc => new { ScanCode = sc })
                        .ToDataTable()
                        .AsTableValuedParameter("app.ScanCodeListType")
                });

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
            DbConnection.Execute(
                "app.GenerateItemUpdateEvents",
                new { ItemIds = itemIds.AsTableValuedParameter("app.IntList") },
                commandType: CommandType.StoredProcedure);
        }

        private void AddMessageQueueItems(DataTable messageQueueItems)
        {
            DbConnection.Execute(
                "esb.AddMessageQueueItem",
                new { @MessageQueueItems = messageQueueItems.AsTableValuedParameter("esb.MessageQueueItemIdsType") },
                commandType: CommandType.StoredProcedure);
        }
    }
}