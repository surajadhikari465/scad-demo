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
    public class UpdateBrandEventService : IEventService
    {
        private ICommandHandler<AddOrUpdateBrandCommand> updateBrandHandler;
        private ICommandHandler<AddUpdateLastChangeByIdentifiersCommand> updateLastChangeHandler;
        private IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>> getItemIdentifiersHandler;
        private IrmaContext irmaContext;

        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        public List<ScanCode> ScanCodes { get; set; }
        public List<RegionalItemMessageModel> RegionalItemMessage { get; set; }

        public UpdateBrandEventService(IrmaContext irmaContext,
            ICommandHandler<AddOrUpdateBrandCommand> updateBrandHandler,
            ICommandHandler<AddUpdateLastChangeByIdentifiersCommand> updateLastChangeHandler,
            IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>> getItemIdentifiersHandler)
        {
            this.irmaContext = irmaContext;
            this.updateBrandHandler = updateBrandHandler;
            this.updateLastChangeHandler = updateLastChangeHandler;
            this.getItemIdentifiersHandler = getItemIdentifiersHandler;
        }

        public void Run()
        {
            if ((ReferenceId == null || ReferenceId < 1) || String.IsNullOrEmpty(Message) || String.IsNullOrEmpty(Region))
            {
                string message = String.Format("BrandUpdateEventHandler was called with invalid arguments.  ReferenceId must be greater than 0.  Region and Message must not be null or empty." +
                    "  ReferenceId = {0}, Message = {1}, Region = {2}", ReferenceId, Message, Region);
                throw new ArgumentException(message);
            }

            AddOrUpdateBrandCommand updateBrand = new AddOrUpdateBrandCommand();
            updateBrand.IconBrandId = ReferenceId;
            updateBrand.BrandName = Message.Length > 25 ? Message.Substring(0, 25) : Message;
            updateBrand.Region = Region;

            updateBrandHandler.Handle(updateBrand);

            GetItemIdentifiersQuery getItemQuery = new GetItemIdentifiersQuery();
            getItemQuery.Predicate = (items => items.Item.Brand_ID == updateBrand.BrandId);
            List<ItemIdentifier> associatedItems = getItemIdentifiersHandler.Handle(getItemQuery);

            AddUpdateLastChangeByIdentifiersCommand updateIconItem = new AddUpdateLastChangeByIdentifiersCommand();
            updateIconItem.Identifiers = associatedItems
                .Where(ai => ai.Deleted_Identifier == 0 && ai.Default_Identifier == 1 && ai.Remove_Identifier == 0)
                .ToList();
            updateLastChangeHandler.Handle(updateIconItem);

            irmaContext.SaveChanges();
        }
    }
}
