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
    public class AddOrUpdateNationalHierarchyEventService : IEventService
    {
        private ICommandHandler<AddOrUpdateNationalHierarchyCommand> addOrUpdateNationalHierarchyHandler;
        private IrmaContext irmaContext;
        private IconContext iconContext;
        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        public List<ScanCode> ScanCodes { get; set; }
        public List<RegionalItemMessageModel> RegionalItemMessage { get; set; }
        public AddOrUpdateNationalHierarchyEventService(IrmaContext irmaContext,
                                                        IconContext iconContext,
                                                        ICommandHandler<AddOrUpdateNationalHierarchyCommand> addOrUpdateNationalHierarchyHandler)
        {
            this.irmaContext = irmaContext;
            this.iconContext = iconContext;
            this.addOrUpdateNationalHierarchyHandler = addOrUpdateNationalHierarchyHandler;
        }
        public void Run()
        {
            if ((ReferenceId == null || ReferenceId < 1) || String.IsNullOrEmpty(Message) || String.IsNullOrEmpty(Region))
            {
                string message = String.Format("AddOrUpdateNationalHierarchyHandler was called with invalid arguments.  ReferenceId must be greater than 0.  Region and Message must not be null or empty." +
                    "  ReferenceId = {0}, Message = {1}, Region = {2}", ReferenceId, Message, Region);
                throw new ArgumentException(message);
            }

            GetHierarchyClassQueryHandler getHierarchyClassQueryHandler = new GetHierarchyClassQueryHandler(iconContext);
            GetHierarchyClassQuery getHierarchyClass = new GetHierarchyClassQuery();
            getHierarchyClass.HierarchyClassId = (int)ReferenceId;
            HierarchyClass hierarchyClass = getHierarchyClassQueryHandler.Handle(getHierarchyClass);

            AddOrUpdateNationalHierarchyCommand addOrUpdateNationalHierarchy = new AddOrUpdateNationalHierarchyCommand();
            addOrUpdateNationalHierarchy.hierarchyClass = hierarchyClass;
            addOrUpdateNationalHierarchy.IconId = (int)ReferenceId;
            addOrUpdateNationalHierarchy.Name =Message;
            addOrUpdateNationalHierarchyHandler.Handle(addOrUpdateNationalHierarchy);
        }
    }
}
