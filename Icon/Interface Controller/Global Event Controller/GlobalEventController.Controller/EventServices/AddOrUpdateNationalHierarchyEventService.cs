using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;

namespace GlobalEventController.Controller.EventServices
{
    public class AddOrUpdateNationalHierarchyEventService : EventServiceBase, IEventService
    {
        private ICommandHandler<AddOrUpdateNationalHierarchyCommand> addOrUpdateNationalHierarchyHandler;
        private IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassQueryHandler;

        public AddOrUpdateNationalHierarchyEventService(
            ICommandHandler<AddOrUpdateNationalHierarchyCommand> addOrUpdateNationalHierarchyHandler,
            IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassQueryHandler)
        {
            this.addOrUpdateNationalHierarchyHandler = addOrUpdateNationalHierarchyHandler;
            this.getHierarchyClassQueryHandler = getHierarchyClassQueryHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(AddOrUpdateNationalHierarchyEventService), ReferenceId, Message, Region);

            HierarchyClass hierarchyClass = getHierarchyClassQueryHandler.Handle(new GetHierarchyClassQuery { HierarchyClassId = (int)ReferenceId });
            HierarchyClass parentHierarchyClass = null;
            if(hierarchyClass.hierarchyParentClassID.HasValue)
            {
                parentHierarchyClass = getHierarchyClassQueryHandler.Handle(new GetHierarchyClassQuery { HierarchyClassId = hierarchyClass.hierarchyParentClassID.Value });
            }

            addOrUpdateNationalHierarchyHandler.Handle(new AddOrUpdateNationalHierarchyCommand
            {
                HierarchyClass = hierarchyClass,
                ParentHierarchyClass = parentHierarchyClass
            });
        }
    }
}
