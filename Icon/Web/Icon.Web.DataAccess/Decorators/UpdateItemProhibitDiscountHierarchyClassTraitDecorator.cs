using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Queries;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateItemProhibitDiscountHierarchyClassTraitDecorator : ICommandHandler<UpdateHierarchyClassTraitCommand>
    {
        private readonly ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler;
        private readonly ICommandHandler<UpdateItemProhibitDiscountByHierarchyClassCommand> updateItemsCommandHandler;
        private readonly IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQueryHandler;
        private ILogger logger;

        public UpdateItemProhibitDiscountHierarchyClassTraitDecorator(
            ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler,
            ICommandHandler<UpdateItemProhibitDiscountByHierarchyClassCommand> updateItemsCommandHandler,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQueryHandler,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.updateItemsCommandHandler = updateItemsCommandHandler;
            this.getHierarchyClassQueryHandler = getHierarchyClassQueryHandler;
            this.logger = logger;
        }

        /// <summary>
        /// Executes main command handler then checks if the Prohibit Discount changed.
        /// Gets the child sub-bricks and loops through those to execute a stored procedure
        /// that updates all the items associated to the Gs1Brick.
        /// </summary>
        /// <param name="data"></param>
        public void Execute(UpdateHierarchyClassTraitCommand data)
        {
            this.commandHandler.Execute(data);

            if (!data.ProhibitDiscountChanged
                || data.UpdatedHierarchyClass.hierarchyID != Hierarchies.Merchandise
                || data.UpdatedHierarchyClass.hierarchyLevel != HierarchyLevels.Gs1Brick)
            {
                return;
            }

            //get all sub-brick IDs that are children of the Gs1Brick
            List<int> subBrickIds = new List<int>();
            GetHierarchyClassByIdParameters getHierarchyClassParams = new GetHierarchyClassByIdParameters { HierarchyClassId = data.UpdatedHierarchyClass.hierarchyClassID };
            var childSubBricks = this.getHierarchyClassQueryHandler
                .Search(getHierarchyClassParams)
                .HierarchyClass1 // children of searched hierarchy class
                .Select(hc => hc.hierarchyClassID)
                .ToList();
            subBrickIds.AddRange(childSubBricks);

            var updateItemsCommand = new UpdateItemProhibitDiscountByHierarchyClassCommand
            {
                // Prohibit Discount will come in either as "true" or string.Empty
                ProhibitDiscount = string.IsNullOrEmpty(data.ProhibitDiscount) ? "false" : "true",
                UserName = data.UserName,
                ModifiedDateTimeUtc = data.ModifiedDateTimeUtc
            };

            // run update for each id so we update all items inside the Brick that was updated.
            foreach (var id in subBrickIds)
            {
                updateItemsCommand.HierarchyClassId = id;
                this.logger.Info($"Updating the ProhibitDiscount value for all items associated to Merchandise SubBrick with HierarchyClassID: {data.UpdatedHierarchyClass.hierarchyClassID}.");
                this.updateItemsCommandHandler.Execute(updateItemsCommand);
            }
        }
    }
}
