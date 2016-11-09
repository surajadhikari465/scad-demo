using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class AddMappingsCommand : ICommandHandler<AddMappingsParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public AddMappingsCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(AddMappingsParameters data)
        {
            globalContext.Context.Mapping.AddRange(data.Mappings);
            globalContext.Context.SaveChanges();
        }
    }
}
