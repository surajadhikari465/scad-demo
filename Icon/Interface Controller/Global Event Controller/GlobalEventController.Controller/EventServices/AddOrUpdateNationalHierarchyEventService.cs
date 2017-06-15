using GlobalEventController.Common;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller.EventServices
{
    public class AddOrUpdateNationalHierarchyEventService : EventServiceBase, IEventService
    {
        private IconContext iconContext;
        private ICommandHandler<AddOrUpdateNationalHierarchyCommand> addOrUpdateNationalHierarchyHandler;
        private IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassQueryHandler;

        public AddOrUpdateNationalHierarchyEventService(IrmaContext irmaContext, IconContext iconContext,
            ICommandHandler<AddOrUpdateNationalHierarchyCommand> addOrUpdateNationalHierarchyHandler,
            IQueryHandler<GetHierarchyClassQuery, HierarchyClass> getHierarchyClassQueryHandler)
            : base (irmaContext)
        {
            this.iconContext = iconContext;
            this.addOrUpdateNationalHierarchyHandler = addOrUpdateNationalHierarchyHandler;
            this.getHierarchyClassQueryHandler = getHierarchyClassQueryHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(AddOrUpdateNationalHierarchyEventService), ReferenceId, Message, Region);

            GetHierarchyClassQuery getHierarchyClass = new GetHierarchyClassQuery();
            getHierarchyClass.HierarchyClassId = ReferenceId.Value;
            HierarchyClass hierarchyClass = getHierarchyClassQueryHandler.Handle(getHierarchyClass);

            AddOrUpdateNationalHierarchyCommand addOrUpdateNationalHierarchy = new AddOrUpdateNationalHierarchyCommand();
            addOrUpdateNationalHierarchy.HierarchyClass = hierarchyClass;
            addOrUpdateNationalHierarchy.IconId = ReferenceId.Value;
            addOrUpdateNationalHierarchy.Name =Message;
            addOrUpdateNationalHierarchyHandler.Handle(addOrUpdateNationalHierarchy);
        }
    }
}
