using Icon.Common.DataAccess;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.Context;
using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ProcessFailedR10MessageResponseCommandHandler : ICommandHandler<ProcessFailedR10MessageResponseCommand>
    {
        private IRenewableContext<IconContext> context;
        private ICommandHandler<ResendMessageQueueEntriesCommand> resendMessageQueueEntriesCommandHandler;
        private ICommandHandler<ResendMessageCommand> resendMessageCommandHandler;

        public ProcessFailedR10MessageResponseCommandHandler(IRenewableContext<IconContext> context,
            ICommandHandler<ResendMessageQueueEntriesCommand> resendMessageQueueEntriesCommandHandler,
            ICommandHandler<ResendMessageCommand> resendMessageCommandHandler)
        {
            this.context = context;
            this.resendMessageQueueEntriesCommandHandler = resendMessageQueueEntriesCommandHandler;
            this.resendMessageCommandHandler = resendMessageCommandHandler;
        }

        public void Execute(ProcessFailedR10MessageResponseCommand data)
        {
            var messageHistory = context.Context.MessageHistory.FirstOrDefault(mh => mh.MessageHistoryId == data.MessageResponse.MessageHistoryId);
            if (messageHistory == null)
            {
                throw new ArgumentException(String.Format("No MessageHistory found with MessageHistoryId equal to {0}", data.MessageResponse.MessageHistoryId));
            }

            messageHistory.MessageStatusId = MessageStatusTypes.Failed;

            if (ShouldResendMessage(data.MessageResponse))
            {
                resendMessageCommandHandler.Execute(new ResendMessageCommand
                    {
                        MessageHistoryId = data.MessageResponse.MessageHistoryId
                    });
            }

            if (ShouldResendMessageQueueEntries(data.MessageResponse, messageHistory))
            {
                resendMessageQueueEntriesCommandHandler.Execute(new ResendMessageQueueEntriesCommand
                    {
                        MessageResponse = data.MessageResponse,
                        MessageHistory = messageHistory
                    });
            }
        }

        private bool ShouldResendMessage(R10MessageResponseModel messageResponse)
        {
            return messageResponse.SystemError && messageResponse.FailureReasonCode == FailureReasonCodes.SystemTimeOut;
        }

        private bool ShouldResendMessageQueueEntries(R10MessageResponseModel messageResponse, MessageHistory messageHistory)
        {
            if (!messageResponse.SystemError 
                && !String.IsNullOrWhiteSpace(messageResponse.FailureReasonCode) 
                && messageResponse.FailureReasonCode.Contains(Constants.BusinessErrorCodes.ThresholdExceededError))
            {
                if (messageHistory.MessageTypeId == MessageTypes.Product
                    || messageHistory.MessageTypeId == MessageTypes.Price
                    || messageHistory.MessageTypeId == MessageTypes.ItemLocale)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
