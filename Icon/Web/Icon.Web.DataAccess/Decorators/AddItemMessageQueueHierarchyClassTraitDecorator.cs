using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Queries;
using System.Linq;

namespace Icon.Web.DataAccess.Decorators
{
    public class AddItemMessageQueueHierarchyClassTraitDecorator : ICommandHandler<UpdateHierarchyClassTraitCommand>
    {
        private readonly ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler;
        private readonly ICommandHandler<AddMessageQueueItemByHierarchyClassIdCommand> addMessageQueueItemCommandHandler;
        private readonly IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery;
        private ILogger logger;

        public AddItemMessageQueueHierarchyClassTraitDecorator(
            ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler,
            ICommandHandler<AddMessageQueueItemByHierarchyClassIdCommand> addMessageQueueItemCommandHandler,
            IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass> getHierarchyClassQuery,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.addMessageQueueItemCommandHandler = addMessageQueueItemCommandHandler;
            this.getHierarchyClassQuery = getHierarchyClassQuery;
            this.logger = logger;
        }

        /// <summary>
        /// Executes UpdateHierarchyClassTraitCommandHandler first and then 
        /// adds records to the esb.MessageQueueItem table for associated items.
        /// This code used to live in the UpdateHierarchyClassTraitCommandHandler.
        /// </summary>
        /// <param name="data">UpdateHierarchyClassTraitCommand object</param>
        public void Execute(UpdateHierarchyClassTraitCommand data)
        {
            this.commandHandler.Execute(data);

            // Don't do anything if the HierarchyClass is not a brick or sub-brick.
            // This means that traits at the item level have not changed
            if (data.UpdatedHierarchyClass.hierarchyID != Hierarchies.Merchandise
                && (data.UpdatedHierarchyClass.hierarchyLevel != HierarchyLevels.Gs1Brick
                    || data.UpdatedHierarchyClass.hierarchyLevel != HierarchyLevels.SubBrick))
            {
                return;
            }

            // Don't do anything if neither the non-merchandise trait nor the sub-team changed.
            if (!(data.NonMerchandiseTraitChanged || data.SubteamChanged || data.ProhibitDiscountChanged))
            {
                return;
            }

            var addMessageQueueItemParameters = new AddMessageQueueItemByHierarchyClassIdCommand();

            if (data.UpdatedHierarchyClass.hierarchyLevel == HierarchyLevels.Gs1Brick)
            {
                GetHierarchyClassByIdParameters getHierarchyClassParams = new GetHierarchyClassByIdParameters { HierarchyClassId = data.UpdatedHierarchyClass.hierarchyClassID };
                var childSubBricks = this.getHierarchyClassQuery
                    .Search(getHierarchyClassParams)
                    .HierarchyClass1 // children of searched hierarchy class
                    .Select(hc => hc.hierarchyClassID);

                foreach (var subBrickId in childSubBricks)
                {
                    this.logger.Info($"Inserting esb.MessageQueueItem records for items associated to Merchandise HierarchyClassID {subBrickId}.");
                    addMessageQueueItemParameters.HierarchyClassId = subBrickId;
                    addMessageQueueItemCommandHandler.Execute(addMessageQueueItemParameters);
                }
            }
            else
            {
                this.logger.Info($"Inserting esb.MessageQueueItem records for items associated to Merchandise HierarchyClassID {data.UpdatedHierarchyClass.hierarchyClassID}.");
                addMessageQueueItemParameters.HierarchyClassId = data.UpdatedHierarchyClass.hierarchyClassID;
                addMessageQueueItemCommandHandler.Execute(addMessageQueueItemParameters);
            }
        }
    }
}
