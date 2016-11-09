using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.ApiController.DataAccess.Commands
{
    public class ClearBusinessUnitInProcessCommandHandler : ICommandHandler<ClearBusinessUnitInProcessCommand>
    {
        IRenewableContext<IconContext> globalContext;

        public ClearBusinessUnitInProcessCommandHandler(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(ClearBusinessUnitInProcessCommand data)
        {
            globalContext.Context.MessageQueueDeleteControllerFromBusinessUnitInProcess(data.InstanceId, data.MessageTypeId);
        }
    }
}
