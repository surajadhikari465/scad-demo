using Dapper;
using Icon.Common;
using Icon.Common.DataAccess;
using System;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class PublishItemUpdatesCommandHandler : ICommandHandler<PublishItemUpdatesCommand>
    {
        private IDbConnection dbConnection;

        public PublishItemUpdatesCommandHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public void Execute(PublishItemUpdatesCommand data)
        {
            var itemIds = dbConnection.Query<int>(@"
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
            dbConnection.Execute(
                "app.GenerateItemUpdateEvents",
                new { ItemIds = itemIds.AsTableValuedParameter("app.IntList") },
                commandType: CommandType.StoredProcedure);
        }

        private void AddMessageQueueItems(DataTable messageQueueItems)
        {
            dbConnection.Execute(
                "esb.AddMessageQueueItem",
                new { @MessageQueueItems = messageQueueItems.AsTableValuedParameter("esb.MessageQueueItemIdsType") },
                commandType: CommandType.StoredProcedure);
        }
    }
}
