using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;

namespace Icon.Esb.EwicErrorResponseListener.DataAccess.Commands
{
    public class SaveToMessageResponseCommand : ICommandHandler<SaveToMessageResponseParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public SaveToMessageResponseCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(SaveToMessageResponseParameters data)
        {
            var messageResponse = new R10MessageResponse
            {
                MessageHistoryId = data.ErrorResponse.MessageHistoryId,
                ResponseText = data.ErrorResponse.ResponseText,
                FailureReasonCode = data.ErrorResponse.ResponseReason,
                SystemError = data.ErrorResponse.SystemError,
                RequestSuccess = data.ErrorResponse.RequestSuccess,
                InsertDate = DateTime.Now
            };

            globalContext.Context.R10MessageResponse.Add(messageResponse);
        }
    }
}
