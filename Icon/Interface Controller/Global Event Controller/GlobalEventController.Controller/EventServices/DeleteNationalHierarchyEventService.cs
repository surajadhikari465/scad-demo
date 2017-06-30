using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;

namespace GlobalEventController.Controller.EventServices
{
    public class DeleteNationalHierarchyEventService : EventServiceBase, IEventService
    {
        private ICommandHandler<DeleteNationalHierarchyCommand> deleteNationalHierarchyHandler;

        public DeleteNationalHierarchyEventService(ICommandHandler<DeleteNationalHierarchyCommand> deleteNationalHierarchyHandler)
        {
            this.deleteNationalHierarchyHandler = deleteNationalHierarchyHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(DeleteNationalHierarchyEventService), ReferenceId, Message, Region);

            DeleteNationalHierarchyCommand deleteNationalHierarchy = new DeleteNationalHierarchyCommand();
            deleteNationalHierarchy.IconId = (int) ReferenceId;
            deleteNationalHierarchyHandler.Handle(deleteNationalHierarchy);
        }
    }
}