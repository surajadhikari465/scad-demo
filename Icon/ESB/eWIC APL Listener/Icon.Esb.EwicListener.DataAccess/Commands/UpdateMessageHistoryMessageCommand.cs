using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class UpdateMessageHistoryMessageCommand : ICommandHandler<UpdateMessageHistoryMessageParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public UpdateMessageHistoryMessageCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(UpdateMessageHistoryMessageParameters data)
        {
            var message = globalContext.Context.MessageHistory.Single(h => h.MessageHistoryId == data.MessageHistoryId);
            message.Message = data.Message;
            globalContext.Context.SaveChanges();
        }
    }
}
