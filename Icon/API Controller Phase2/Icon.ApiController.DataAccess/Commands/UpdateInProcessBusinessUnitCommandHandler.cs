using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateInProcessBusinessUnitCommandHandler : ICommandHandler<UpdateInProcessBusinessUnitCommand>
    {
        IRenewableContext<IconContext> globalContext;

        public UpdateInProcessBusinessUnitCommandHandler(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(UpdateInProcessBusinessUnitCommand data)
        {
            globalContext.Context.MessageQueueUpdateControllerBusinessUnitInProcess(data.InstanceId, data.BusinessUnitId, data.MessageTypeId);
        }
    }
}
