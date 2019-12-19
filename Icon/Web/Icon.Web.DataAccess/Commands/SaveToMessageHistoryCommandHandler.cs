using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class SaveToMessageHistoryCommandHandler : ICommandHandler<SaveToMessageHistoryCommand>
    {
        private readonly IconContext context;

        public SaveToMessageHistoryCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(SaveToMessageHistoryCommand data)
        {
            context.MessageHistory.AddRange(data.Messages);
            context.SaveChanges();
        }
    }
}
