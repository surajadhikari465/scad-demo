using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class AddAgencyCommand : ICommandHandler<AddAgencyParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public AddAgencyCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(AddAgencyParameters data)
        {
            globalContext.Context.Agency.Add(data.Agency);
            globalContext.Context.SaveChanges();
        }
    }
}
