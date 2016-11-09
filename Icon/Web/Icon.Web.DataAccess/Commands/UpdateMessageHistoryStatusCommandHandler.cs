using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommandHandler : ICommandHandler<UpdateMessageHistoryStatusCommand>
    {
        private readonly IconContext context;

        public UpdateMessageHistoryStatusCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateMessageHistoryStatusCommand data)
        {
            var messages = context.MessageHistory.Where(h => data.MessageHistoriesById.Contains(h.MessageHistoryId)).ToList();

            foreach (var message in messages)
            {
                message.MessageStatusId = data.MessageStatusId;
            }

            context.SaveChanges();
        }
    }
}
