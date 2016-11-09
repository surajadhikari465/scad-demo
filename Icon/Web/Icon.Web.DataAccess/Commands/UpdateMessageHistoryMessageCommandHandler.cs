using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateMessageHistoryMessageCommandHandler : ICommandHandler<UpdateMessageHistoryMessageCommand>
    {
        private readonly IconContext context;

        public UpdateMessageHistoryMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateMessageHistoryMessageCommand data)
        {
            var message = context.MessageHistory.Single(h => h.MessageHistoryId == data.MessageHistoryId);
            message.Message = data.Message;
            context.SaveChanges();
        }
    }
}
