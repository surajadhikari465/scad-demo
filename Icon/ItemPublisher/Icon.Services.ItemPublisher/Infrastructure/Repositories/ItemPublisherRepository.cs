using Dapper;
using Icon.Common;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    public class ItemPublisherRepository : AbstractRepository, IItemPublisherRepository
    {
        private IMessageQueueItemModelBuilder messageQueueItemModelBuilder;

        public ItemPublisherRepository(IProviderFactory dbProviderFactory,
            IMessageQueueItemModelBuilder messageQueueItemModelBuilder)
            : base(dbProviderFactory)
        {
            this.messageQueueItemModelBuilder = messageQueueItemModelBuilder;
        }

        /// <summary>
        /// Inserts a record in to the [esb].[MessageQueueItemArchive] table
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task AddMessageQueueHistoryRecords(List<MessageQueueItemArchive> history)
        {
            foreach (MessageQueueItemArchive historyItem in history)
            {
                await this.AddMessageQueueHistoryRecord(historyItem);
            }
        }

        public async Task AddMessageQueueHistoryRecord(MessageQueueItemArchive history)
        {
            string query = $@"INSERT INTO [esb].[MessageQueueItemArchive]
               ([MessageQueueItemJson]
               ,[ErrorOccurred]
               ,[ErrorMessage]
               ,[MessageId]
               ,[Message]
               ,[MessageHeader]
               ,[WarningMessage]
               ,[InsertDateUtc],
                [Machine])
                VALUES
               (@MessageQueueItemJson
               ,@ErrorOccurred
               ,@ErrorMessage
               ,@MessageId
               ,@Message
               ,@MessageHeader
               ,@WarningMessage
               ,@InsertDateUtc
               ,@Machine)";

            var parameters = new
            {
                MessageQueueItemJson = new DbString { Value = JsonConvert.SerializeObject(history.MessageQueueItemJson) },
                history.ErrorOccurred,
                history.ErrorMessage,
                MessageId = history.MessageId,
                Message = new DbString { Value = history.Message },
                MessageHeader = new DbString { Value = history.MessageHeader },
                WarningMessage = new DbString { Value = history.WarningMessage },
                InsertDateUtc = history.InsertDateUTC,
                Machine = history.Machine
            };

            CommandDefinition command = new CommandDefinition(query, parameters, this.DbProviderFactory.Provider.Transaction);

            await this.DbProviderFactory.Provider.Connection.ExecuteAsync(command);
        }

        /// <summary>
        /// Dequeues records from the [esb].[MessageQueueItem] table and then loads all of the data needed
        /// to create a list of MessageQueueItemModel models
        /// </summary>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task<List<MessageQueueItemModel>> DequeueMessageQueueRecords(int batchSize = 1)
        {
            IEnumerable<MessageQueueItem> messageQueueItems = await this.DbProviderFactory.Provider.Connection.QueryAsync<MessageQueueItem>($@"exec [esb].[DequeueMessageQueueItem] {batchSize}", null, DbProviderFactory.Provider.Transaction);

            List<MessageQueueItemModel> response = new List<MessageQueueItemModel>();

            var itemIds = (from item in messageQueueItems
                           group item by item.ItemId into distinctItems
                           select new
                           {
                               itemId = distinctItems.First().ItemId,
                               esbReadyDatetimeUtc = distinctItems.First().EsbReadyDatetimeUtc,
                               insertDateUtc = distinctItems.First().InsertDateUtc,
                           }).ToDataTable();

            SqlMapper.GridReader resultSet = await this.DbProviderFactory.Provider.Connection.QueryMultipleAsync("esb.RetrieveMessageQueueItems",
                new { @itemIds = itemIds.AsTableValuedParameter("[esb].[MessageQueueItemIdsType]") },
                DbProviderFactory.Provider.Transaction,
                commandType: CommandType.StoredProcedure);

            List<Item> items = (await resultSet.ReadAsync<Item>()).ToList();
            List<Nutrition> nutrition = (await resultSet.ReadAsync<Nutrition>()).ToList();
            List<Hierarchy> hierarchies = (await resultSet.ReadAsync<Hierarchy>()).ToList();

            foreach (Item item in items)
            {
                MessageQueueItemModel model = this.messageQueueItemModelBuilder.Build(
                     item,
                     hierarchies.Where(x => x.ItemId == item.ItemId).ToList(),
                     nutrition.FirstOrDefault(x => x.Plu == item.ScanCode));
                response.Add(model);
            }

            return response;
        }

        public async Task AddDeadLetterMessageQueueRecord(MessageDeadLetterQueue messageDeadLetterQueue)
        {
            string query = $@"INSERT INTO [esb].[MessageDeadLetterQueue]
            ([JsonObject])
            VALUES
            (@JsonObject)";
            var parameters = new
            {
                JsonObject = JsonConvert.SerializeObject(messageDeadLetterQueue)
            };

            CommandDefinition command = new CommandDefinition(query, parameters, this.DbProviderFactory.Provider.Transaction);

            await this.DbProviderFactory.Provider.Connection.ExecuteAsync(command);
            
        }
    }
}