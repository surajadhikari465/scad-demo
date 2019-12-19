using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Commands;
using System;

namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateItemTypeHierarchyClassTraitDecorator : ICommandHandler<UpdateHierarchyClassTraitCommand>
    {
        private readonly ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler;
        private readonly ICommandHandler<UpdateItemTypeByHierarchyClassCommand> updateItemsCommandHandler;
        private ILogger logger;

        public UpdateItemTypeHierarchyClassTraitDecorator(
            ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler,
            ICommandHandler<UpdateItemTypeByHierarchyClassCommand> updateItemsCommandHandler,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.updateItemsCommandHandler = updateItemsCommandHandler;
            this.logger = logger;
        }

        public void Execute(UpdateHierarchyClassTraitCommand data)
        {
            this.commandHandler.Execute(data);

            if (!data.NonMerchandiseTraitChanged
                || data.UpdatedHierarchyClass.hierarchyID != Hierarchies.Merchandise
                || data.UpdatedHierarchyClass.hierarchyLevel != HierarchyLevels.SubBrick)
            {
                return;
            }

            int itemTypeId = MerchToItemTypeCodeMapper.GetItemTypeId(data.NonMerchandiseTrait);

            if (itemTypeId <= 0)
            {
                throw new ArgumentException($"Cannot update all items associated to the sub-brick because the Non-Merchandise trait cannot be mapped to an Item Type.");
            }

            var updateItemsCommand = new UpdateItemTypeByHierarchyClassCommand
            {
                HierarchyClassId = data.UpdatedHierarchyClass.hierarchyClassID,
                ItemTypeId = itemTypeId,
                UserName = data.UserName,
                ModifiedDateTimeUtc = data.ModifiedDateTimeUtc
            };

            this.logger.Info($"Updating the ItemTypeId for all items associated to Merchandise SubBrick with HierarchyClassID: {data.UpdatedHierarchyClass.hierarchyClassID}.");
            this.updateItemsCommandHandler.Execute(updateItemsCommand);
        }
    }
}
