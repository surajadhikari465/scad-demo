using Icon.Common.DataAccess;
using Icon.Esb.R10Listener.Context;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Commands
{
    public class ResendMessageCommandHandler : ICommandHandler<ResendMessageCommand>
    {
        private IRenewableContext<IconContext> context;
        private R10ListenerApplicationSettings listenerSettings;

        public ResendMessageCommandHandler(
            IRenewableContext<IconContext> context,
            R10ListenerApplicationSettings listenerSettings)
        {
            this.context = context;
            this.listenerSettings = listenerSettings;
        }

        public void Execute(ResendMessageCommand data)
        {
            if (listenerSettings.ResendMessageCount < 1)
            {
                return;
            }

            var messageHistory = context.Context.MessageHistory.FirstOrDefault(mh => mh.MessageHistoryId == data.MessageHistoryId);
            if (messageHistory == null || messageHistory.MessageStatusId == MessageStatusTypes.Ready)
            {
                return;
            }

            var resendStatus = context.Context.MessageResendStatus.SingleOrDefault(mrs => mrs.MessageHistoryId == data.MessageHistoryId);
            if (resendStatus == null)
            {
                resendStatus = new MessageResendStatus
                {
                    MessageHistoryId = data.MessageHistoryId,
                    NumberOfResends = 0,
                    InsertDate = DateTime.Now
                };

                context.Context.MessageResendStatus.Add(resendStatus);
            }

            if (resendStatus.NumberOfResends < listenerSettings.ResendMessageCount)
            {
                messageHistory.MessageStatusId = MessageStatusTypes.Ready;
                resendStatus.NumberOfResends++;
            }
        }
    }
}
