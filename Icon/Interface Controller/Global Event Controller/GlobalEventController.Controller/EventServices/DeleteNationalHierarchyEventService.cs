using GlobalEventController.Common;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller.EventServices
{
   public class DeleteNationalHierarchyEventService : EventServiceBase, IEventService
    {
        private ICommandHandler<DeleteNationalHierarchyCommand> deleteNationalHierarchyHandler;

        public DeleteNationalHierarchyEventService(IrmaContext irmaContext,
            ICommandHandler<DeleteNationalHierarchyCommand> deleteNationalHierarchyHandler)
                : base(irmaContext)
        {
            this.deleteNationalHierarchyHandler = deleteNationalHierarchyHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(DeleteNationalHierarchyEventService), ReferenceId, Message, Region);

            DeleteNationalHierarchyCommand deleteNationalHierarchy = new DeleteNationalHierarchyCommand();
            deleteNationalHierarchy.IconId = (int) ReferenceId;
            deleteNationalHierarchyHandler.Handle(deleteNationalHierarchy);
            irmaContext.SaveChanges();
        }
    }
}