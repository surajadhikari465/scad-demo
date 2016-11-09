using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Linq;

namespace PushController.DataAccess.Commands
{
    public class MarkStagedRecordsAsInProcessForUdmCommandHandler : ICommandHandler<MarkStagedRecordsAsInProcessForUdmCommand>
    {
        private IRenewableContext<IconContext> context;

        public MarkStagedRecordsAsInProcessForUdmCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(MarkStagedRecordsAsInProcessForUdmCommand command)
        {
            bool controllerAlreadyHasRecordsInProcess = context.Context.IRMAPush.Any(push =>
                push.InProcessBy == command.Instance &&
                push.EsbReadyDate != null &&
                push.InUdmDate == null &&
                push.UdmFailedDate == null);

            if (!controllerAlreadyHasRecordsInProcess)
            {
                context.Context.MarkStagingTableEntriesAsInProcessForUdm(command.MaxRecordsToProcess, command.Instance);
            }
        }
    }
}
