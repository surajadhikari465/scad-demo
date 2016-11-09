using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Commands
{
    public class AddPluRequestCommandHandler : ICommandHandler<AddPluRequestCommand>
    {
        private IconContext context;

        public AddPluRequestCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddPluRequestCommand data)
        {
            context.PLURequest.Add(data.PLURequest);
            context.SaveChanges();
        }
    }
}
