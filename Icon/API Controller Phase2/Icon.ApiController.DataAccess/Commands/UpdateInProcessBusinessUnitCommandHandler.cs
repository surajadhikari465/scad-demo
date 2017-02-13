using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateInProcessBusinessUnitCommandHandler : ICommandHandler<UpdateInProcessBusinessUnitCommand>
    {
        IDbContextFactory<IconContext> iconContextFactory;

        public UpdateInProcessBusinessUnitCommandHandler(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(UpdateInProcessBusinessUnitCommand data)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                context.MessageQueueUpdateControllerBusinessUnitInProcess(data.InstanceId, data.BusinessUnitId, data.MessageTypeId);
            }
        }
    }
}
