using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Irma.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller.EventServices
{
    public class UpdateBrandEventService : EventServiceBase, IEventService
    {
        private ICommandHandler<AddOrUpdateBrandCommand> updateBrandHandler;
        private ICommandHandler<AddUpdateLastChangeByIdentifiersCommand> updateLastChangeHandler;
        private IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>> getItemIdentifiersHandler;

        public UpdateBrandEventService(
            ICommandHandler<AddOrUpdateBrandCommand> updateBrandHandler,
            ICommandHandler<AddUpdateLastChangeByIdentifiersCommand> updateLastChangeHandler,
            IQueryHandler<GetItemIdentifiersQuery, List<ItemIdentifier>> getItemIdentifiersHandler) 
        {
            this.updateBrandHandler = updateBrandHandler;
            this.updateLastChangeHandler = updateLastChangeHandler;
            this.getItemIdentifiersHandler = getItemIdentifiersHandler;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(UpdateBrandEventService), ReferenceId, Message, Region);

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
        }
    }
}
