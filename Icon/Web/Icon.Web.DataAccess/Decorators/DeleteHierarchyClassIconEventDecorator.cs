using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Icon.Logging;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Decorators
{
    public class DeleteHierarchyClassIconEventDecorator : ICommandHandler<DeleteHierarchyClassCommand>
    {
        private readonly ICommandHandler<DeleteHierarchyClassCommand> commandHandler;
        private IconContext context;
        private ILogger logger;

        public DeleteHierarchyClassIconEventDecorator(
            ICommandHandler<DeleteHierarchyClassCommand> commandHandler,
            IconContext context,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.context = context;
            this.logger = logger;
        }

        public void Execute(DeleteHierarchyClassCommand command)
        {
            // log that an icon hierarchy class is being deleted
            var deletedClass = command.DeletedHierarchyClass;
            this.commandHandler.Execute(command);
            logger.Info($"Deleting iCON Hierarchy Class. ID = {deletedClass.hierarchyClassID}, Name = '{deletedClass.hierarchyClassName}'");
        }
    }
}
