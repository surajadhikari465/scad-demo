using MessageGenerationWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Icon.Esb.Producer;
using System.Data.SqlClient;
using System.Configuration;
using MessageGenerationWeb.MessageBuilders;
using Icon.Esb;
using Dapper;

namespace MessageGenerationWeb.Services
{
    public class ItemPriceService : IItemPriceService
    {
        IEsbProducer producer;
        ItemPriceMessageBuilder messageBuilder;

        public ItemPriceService()
        {
            producer = new MockEsbProducer();
            messageBuilder = new ItemPriceMessageBuilder();
        }

        public void DeleteItemPrices(List<ItemPriceDeleteModel> itemPrices)
        {
            var messages = messageBuilder.BuildDeleteMessages(itemPrices);

            using (var db = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString))
            {
                foreach (var message in messages)
                {
                    string sql = @" insert into app.MessageHistory(MessageTypeId, MessageStatusId, Message, InsertDate)
                                    values (4, 3, @MessageText, GETDATE())
                                    select cast(scope_identity() as int)";
                    var id = db.Query<int>(sql, new { MessageText = message }).Single();

                    producer.Send(message, new Dictionary<string, string> { { "IconMessageID", id.ToString() } });

                    sql = @"update app.MessageHistory
                            set MessageStatusId = 2
                            where MessageHistoryId = @MessageHistoryId";
                    db.Execute(sql, new { MessageHistoryId = id });
                }
            }
        }
    }
}