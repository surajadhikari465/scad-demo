using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddPluRequestChangeHistoryCommandHandler : ICommandHandler<AddPluRequestChangeHistoryCommand>
    {
        private IconContext context;

        public AddPluRequestChangeHistoryCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddPluRequestChangeHistoryCommand data)
        {
            context.PLURequestChangeHistory.Add(data.PLURequestChangeHistory);
            context.SaveChanges();
        }
    }
}
