using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class AddMessageHistoryCommand : ICommandHandler<AddMessageHistoryParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public AddMessageHistoryCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(AddMessageHistoryParameters data)
        {
            globalContext.Context.MessageHistory.Add(data.Message);
            globalContext.Context.SaveChanges();
        }
    }
}
