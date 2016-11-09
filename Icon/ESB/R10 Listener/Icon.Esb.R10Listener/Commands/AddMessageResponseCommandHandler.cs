using Icon.Common.DataAccess;
using Icon.Esb.R10Listener.Context;
using Icon.Framework;
using System;

namespace Icon.Esb.R10Listener.Commands
{
    public class AddMessageResponseCommandHandler : ICommandHandler<AddMessageResponseCommand>
    {
        private IRenewableContext<IconContext> context;

        public AddMessageResponseCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(AddMessageResponseCommand data)
        {
            R10MessageResponse messageResponse = new R10MessageResponse
            {
                MessageHistoryId = data.MessageResponse.MessageHistoryId,
                ResponseText = data.MessageResponse.ResponseText,
                FailureReasonCode = data.MessageResponse.FailureReasonCode,
                SystemError = data.MessageResponse.SystemError,
                RequestSuccess = data.MessageResponse.RequestSuccess,
                InsertDate = DateTime.Now
            };

            context.Context.R10MessageResponse.Add(messageResponse);
        }
    }
}
