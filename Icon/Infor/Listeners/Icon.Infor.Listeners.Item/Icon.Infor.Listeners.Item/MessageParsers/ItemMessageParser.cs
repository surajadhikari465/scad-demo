using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Infor.Listeners.Item.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.Item.Extensions;
using Icon.Logging;
using Newtonsoft.Json;
using Icon.Infor.Listeners.Item.Constants;

namespace Icon.Infor.Listeners.Item.MessageParsers
{
    public class ItemMessageParser : MessageParserBase<items, IEnumerable<ItemModel>>
    {
        private ItemListenerSettings settings;
        private ILogger<ItemMessageParser> logger;

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
                var messageParseTime = DateTime.Now;

                List<ItemModel> models = new List<ItemModel>();
                foreach (var item in items.item)
                {
                    try
                    {
                        models.Add(item.ToItemModel(messageId, messageParseTime, sequenceId));
                    }
                    catch (Exception ex)
                    {
                        logger.Error(JsonConvert.SerializeObject(
                            new
                            {
                                ErrorCode = ApplicationErrors.Codes.UnableToParseItem,
                                ErrorDetails = ApplicationErrors.Messages.UnableToParseItem,
                                InforMessageId = messageId,
                                Item = item,
                                Exception = ex
                            }));
                    }
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
