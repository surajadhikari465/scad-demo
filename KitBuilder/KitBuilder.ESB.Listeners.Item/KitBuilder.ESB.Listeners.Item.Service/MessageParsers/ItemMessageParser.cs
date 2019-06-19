using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Icon.Logging;
using KitBuilder.ESB.Listeners.Item.Service.Constants;
using KitBuilder.ESB.Listeners.Item.Service.Extensions;
using KitBuilder.ESB.Listeners.Item.Service.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
namespace KitBuilder.ESB.Listeners.Item.Service.MessageParsers
{
    public class ItemMessageParser : MessageParserBase<items, IEnumerable<ItemModel>>
    {
        private ItemListenerSettings settings;
        private ILogger<ItemMessageParser> logger;

        public ItemMessageParser()
        {
        }

        public ItemMessageParser(ItemListenerSettings settings, ILogger<ItemMessageParser> logger)
        {
            this.settings = settings;
            this.logger = logger;
        }

        public override IEnumerable<ItemModel> ParseMessage(IEsbMessage message)
        {
            try
            {
                var items = DeserializeMessage(message);
                var messageId = message.GetProperty("IconMessageID");
                var sequenceId = GetSequenceId(message);
                var messageParseTime = DateTime.UtcNow;

                List<ItemModel> models = new List<ItemModel>();

                try
                {

                    if (messageId.StartsWith("Icon_")){
                        Parallel.ForEach(items.item, new ParallelOptions() {MaxDegreeOfParallelism = 2},
                            (currentItem) =>
                            {
                                var itemModel = currentItem.ToItemModel(messageId, messageParseTime, sequenceId);
                                if (itemModel != null)
                                {

                                    logger.Info($"Parsed {messageId}");
                                    models.Add(itemModel);
                                }
                            });
                    }

                    message.Acknowledge();
                } catch (Exception ex)
                {
                    logger.Error(JsonConvert.SerializeObject(
                            new
                            {
                                ErrorCode = ApplicationErrors.Codes.UnableToParseItem,
                                ErrorDetails = ApplicationErrors.Messages.UnableToParseItem,
                                InforMessageId = messageId,
                                Exception = ex
                            }));
                }

                return models;
            }
            catch(Exception e)
            {
                throw new Exception(ApplicationErrors.Messages.UnableToParseMessage, e);
            }
        }

        private decimal? GetSequenceId(IEsbMessage message)
        {
            if(settings.ValidateSequenceId)
            {
                return decimal.Parse(message.GetProperty("SequenceID"));
            }
            else
            {
                return null;
            }
        }
    }
}
