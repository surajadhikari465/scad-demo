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
    public class BrandDeleteEventService : IEventService
    {
        private ICommandHandler<BrandDeleteCommand> brandDeleteHandler;
        private IrmaContext irmaContext;

        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        public List<ScanCode> ScanCodes { get; set; }
        public List<RegionalItemMessageModel> RegionalItemMessage { get; set; }

        public BrandDeleteEventService(IrmaContext irmaContext,
            ICommandHandler<BrandDeleteCommand> brandDeleteHandler)
        {
            this.irmaContext = irmaContext;
            this.brandDeleteHandler = brandDeleteHandler;
        }

        public void Run()
        {
            if ((ReferenceId == null || ReferenceId < 1) || String.IsNullOrEmpty(Message) || String.IsNullOrEmpty(Region))
            {
                string message = String.Format("BrandDeleteEventHandler was called with invalid arguments.  ReferenceId must be greater than 0.  Region and Message must not be null or empty." +
                    "  ReferenceId = {0}, Message = {1}, Region = {2}", ReferenceId, Message, Region);
                throw new ArgumentException(message);
            }

            BrandDeleteCommand deleteBrand = new BrandDeleteCommand();
            deleteBrand.IconBrandId = ReferenceId;
            deleteBrand.Region = Region;

            brandDeleteHandler.Handle(deleteBrand);

            irmaContext.SaveChanges();
        }
    }
}

