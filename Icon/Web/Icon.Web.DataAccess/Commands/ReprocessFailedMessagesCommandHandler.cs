using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedMessagesCommandHandler : ICommandHandler<ReprocessFailedMessagesCommand>
    {
        private IconContext context;

        public ReprocessFailedMessagesCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedMessagesCommand data)
        {
            var messageHistories = context.MessageHistory.Where(mh => data.MessageHistoriesById.Contains(mh.MessageHistoryId));

            foreach (var messageHistory in messageHistories)
            {
                messageHistory.MessageStatusId = MessageStatusTypes.Ready;
            }

            context.SaveChanges();
        }
    }
}
