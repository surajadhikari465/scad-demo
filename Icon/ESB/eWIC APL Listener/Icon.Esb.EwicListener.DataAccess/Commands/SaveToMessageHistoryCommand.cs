using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class SaveToMessageHistoryCommand : ICommandHandler<SaveToMessageHistoryParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public SaveToMessageHistoryCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(SaveToMessageHistoryParameters data)
        {
            globalContext.Context.MessageHistory.AddRange(data.Messages);
            globalContext.Context.SaveChanges();
        }
    }
}
