using Icon.Esb.Factory;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Infor.Services.NewItem.Constants;
using Infor.Services.NewItem.MessageBuilders;
using Infor.Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infor.Services.NewItem.Services
{
    public class InforItemService : IInforItemService
    {
        private IEsbConnectionFactory esbFactory;
        private IMessageBuilder<IEnumerable<NewItemModel>> messageBuilder;
        private IconContext context;
        private ILogger<InforItemService> logger;

        public InforItemService(IEsbConnectionFactory esbFactory, IMessageBuilder<IEnumerable<NewItemModel>> messageBuilder, IconContext context, ILogger<InforItemService> logger)
        {
            this.esbFactory = esbFactory;
            this.messageBuilder = messageBuilder;
            this.context = context;
            this.logger = logger;
        }

        public AddNewItemsToInforResponse AddNewItemsToInfor(AddNewItemsToInforRequest request)
        {
            var response = new AddNewItemsToInforResponse();
            if (request.NewItems.Any())
            {
                MessageHistory messageHistory = null;
                try
                {
                    var message = messageBuilder.BuildMessage(request.NewItems);
                    messageHistory = context.MessageHistory.Add(new MessageHistory
                    {
                        MessageTypeId = MessageTypes.InforNewItem,
                        Message = message,
                        MessageStatusId = MessageStatusTypes.Sent,
                        InsertDate = DateTime.Now,
                        ProcessedDate = DateTime.Now
                    });
                    context.SaveChanges();
                    response.MessageHistoryId = messageHistory.MessageHistoryId;

                    using (IEsbProducer producer = esbFactory.CreateProducer())
                    {
                        producer.Send(message, new Dictionary<string, string> { { "IconMessageID", messageHistory.MessageHistoryId.ToString() } });
                    }
                    LogMessageSent(messageHistory, request.Region, request.NewItems);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    response.ErrorOccurred = true;

                    //Only mark the message as failed if it has successfully been added to the database which means it's MessageHistoryId has been populated by the DbContext
                    if (messageHistory != null && messageHistory.MessageHistoryId != 0)
                    {
                        messageHistory.MessageStatusId = MessageStatusTypes.Failed;
                        context.SaveChanges();
                    }
                    foreach (var item in request.NewItems.Where(ni => ni.ErrorCode == null))
                    {
                        item.ErrorCode = ApplicationErrors.Codes.FailedToSendMessageToEsb;
                        item.ErrorDetails = string.Format(ApplicationErrors.Codes.FailedToSendMessageToEsb, ex.ToString());
                    }
                }
                finally
                {
                    foreach (var item in request.NewItems)
                    {
                        item.MessageHistoryId = messageHistory?.MessageHistoryId;
                    }
                }
            }
            return response;
        }

        private void LogMessageSent(MessageHistory message, string region, IEnumerable<NewItemModel> items)
        {
            logger.Info(
                string.Format(
                    "InforItemService sent message to Infor. MessageHistoryId: {0}, Region: {1}, NumberOfItems: {2}, ScanCodes: {3}",
                    message.MessageHistoryId,
                    region,
                    items.Count(),
                    string.Join(",", items.Select(i => i.ScanCode))));
        }
    }
}
