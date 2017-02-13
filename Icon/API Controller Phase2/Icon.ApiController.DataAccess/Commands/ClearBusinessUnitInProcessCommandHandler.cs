using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class ClearBusinessUnitInProcessCommandHandler : ICommandHandler<ClearBusinessUnitInProcessCommand>
    {
        IDbContextFactory<IconContext> iconContextFactory;

        public ClearBusinessUnitInProcessCommandHandler(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(ClearBusinessUnitInProcessCommand data)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                context.MessageQueueDeleteControllerFromBusinessUnitInProcess(data.InstanceId, data.MessageTypeId);
            }
        }
    }
}
