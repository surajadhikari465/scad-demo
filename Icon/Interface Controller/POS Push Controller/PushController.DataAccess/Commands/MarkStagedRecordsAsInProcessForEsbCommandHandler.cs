using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Linq;

namespace PushController.DataAccess.Commands
{
    public class MarkStagedRecordsAsInProcessForEsbCommandHandler : ICommandHandler<MarkStagedRecordsAsInProcessForEsbCommand>
    {
        private IRenewableContext<IconContext> context;

        public MarkStagedRecordsAsInProcessForEsbCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(MarkStagedRecordsAsInProcessForEsbCommand command)
        {
            bool controllerAlreadyHasRecordsInProcess = context.Context.IRMAPush.Any(push =>
                push.InProcessBy == command.Instance &&
                push.EsbReadyDate == null &&
                push.EsbReadyFailedDate == null);

            if (!controllerAlreadyHasRecordsInProcess)
            {
                context.Context.MarkStagingTableEntriesAsInProcessForEsb(command.MaxRecordsToProcess, command.Instance);
            }
        }
    }
}
