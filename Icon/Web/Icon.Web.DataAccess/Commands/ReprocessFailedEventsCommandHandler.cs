using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedEventsCommandHandler : ICommandHandler<ReprocessFailedEventsCommand>
    {
        private IconContext context;

        public ReprocessFailedEventsCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedEventsCommand data)
        {
            var failedEventQueues = context.EventQueue.Where(eq => data.EventQueueIds.Contains(eq.QueueId));
            foreach (var failedEvent in failedEventQueues)
            {
                failedEvent.ProcessFailedDate = null;
            }
            context.SaveChanges();
        }
    }
}
