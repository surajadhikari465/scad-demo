using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Esb.EwicErrorResponseListener.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommand : ICommandHandler<UpdateMessageHistoryStatusParameters>
    {
        IRenewableContext<IconContext> globalContext;

        public UpdateMessageHistoryStatusCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageHistoryStatusParameters data)
        {
            var message = globalContext.Context.MessageHistory.Single(h => data.MessageHistoryId == h.MessageHistoryId);

            message.MessageStatusId = data.MessageStatusId;

            globalContext.Context.SaveChanges();
        }
    }
}
