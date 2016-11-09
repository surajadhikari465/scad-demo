using PushController.DataAccess.Interfaces;
using System.Linq;

namespace PushController.DataAccess.Commands
{
    public class MarkPublishedRecordsAsInProcessCommandHandler : ICommandHandler<MarkPublishedRecordsAsInProcessCommand>
    {
        public void Execute(MarkPublishedRecordsAsInProcessCommand command)
        {
            bool controllerAlreadyHasRecordsInProcess = command.Context.IConPOSPushPublish.Any(publish =>
                publish.InProcessBy == command.Instance &&
                publish.ProcessedDate == null &&
                publish.ProcessingFailedDate == null);

            if (!controllerAlreadyHasRecordsInProcess)
            {
                command.Context.MarkPublishTableEntriesAsInProcess(command.MaxRecordsToProcess, command.Instance);
            }
        }
    }
}
