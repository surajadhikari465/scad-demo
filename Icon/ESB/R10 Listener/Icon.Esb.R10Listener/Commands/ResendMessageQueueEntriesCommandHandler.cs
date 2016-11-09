using Icon.Common.DataAccess;
using Icon.Esb.ListenerApplication;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.Context;
using Icon.Esb.R10Listener.Infrastructure.Cache;
using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ResendMessageQueueEntriesCommandHandler : ICommandHandler<ResendMessageQueueEntriesCommand>
    {
        private IRenewableContext<IconContext> context;
        private IMessageQueueResendStatusCache messageQueueResendStatusCache;
        private R10ListenerApplicationSettings applicationSettings;

        public ResendMessageQueueEntriesCommandHandler(IRenewableContext<IconContext> context,
            IMessageQueueResendStatusCache messageQueueResendStatusCache,
            R10ListenerApplicationSettings applicationSettings)
        {
            this.context = context;
            this.messageQueueResendStatusCache = messageQueueResendStatusCache;
            this.applicationSettings = applicationSettings;
        }

        public void Execute(ResendMessageQueueEntriesCommand data)
        {
            if (data.MessageHistory != null)
            {
                SetAssociatedMessageQueueEntriesAsReady(data.MessageHistory, data.MessageResponse.BusinessErrors);
            }
        }

        private void SetAssociatedMessageQueueEntriesAsReady(MessageHistory messageHistory, IEnumerable<BusinessErrorModel> businessErrors)
        {
            List<IItemMessageQueue> messageQueueEntries = GetMessageQueueEntries(messageHistory);
            if (messageQueueEntries.Any())
            {
                var itemIds = GetItemIdsForThresholdExceededErrors(businessErrors);
                var messages = messageQueueEntries.Where(mqe => itemIds.Contains(mqe.ItemId));

                foreach (var message in messages)
                {
                    var resendStatus = messageQueueResendStatusCache.Get(messageHistory.MessageTypeId, message.MessageQueueId);

                    if (resendStatus.NumberOfResends < applicationSettings.ResendMessageCount)
                    {
                        message.MessageStatusId = MessageStatusTypes.Ready;

                        resendStatus.NumberOfResends++;
                        messageQueueResendStatusCache.AddOrUpdate(messageHistory.MessageTypeId, resendStatus);
                    }
                }
            }
        }

        private List<IItemMessageQueue> GetMessageQueueEntries(MessageHistory messageHistory)
        {
            List<IItemMessageQueue> messageQueueEntries = new List<IItemMessageQueue>();

            switch (messageHistory.MessageTypeId)
            {
                case MessageTypes.Product:
                    messageQueueEntries = messageHistory.MessageQueueProduct.Cast<IItemMessageQueue>().ToList();
                    break;
                case MessageTypes.ItemLocale:
                    messageQueueEntries = messageHistory.MessageQueueItemLocale.Cast<IItemMessageQueue>().ToList();
                    break;
                case MessageTypes.Price:
                    messageQueueEntries = messageHistory.MessageQueuePrice.Cast<IItemMessageQueue>().ToList();
                    break;
            }

            return messageQueueEntries;
        }

        private List<int> GetItemIdsForThresholdExceededErrors(IEnumerable<BusinessErrorModel> businessErrors)
        {
            return businessErrors
                .Where(be => be.Code == BusinessErrorCodes.ThresholdExceededError)
                .Select(be => be.MainId) //MainId correlates to the MessageQueue's ItemId
                .ToList();
        }
    }
}
