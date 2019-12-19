using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Commands;
using System;

namespace Icon.Web.DataAccess.Decorators
{
    public class UpdateItemSubTeamHierarchyClassTraitDecorator : ICommandHandler<UpdateHierarchyClassTraitCommand>
    {
        private readonly ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler;
        private readonly ICommandHandler<UpdateItemSubTeamByHierarchyClassCommand> updateItemsSubTeamCommandHandler;
        private ILogger logger;

        public UpdateItemSubTeamHierarchyClassTraitDecorator(
            ICommandHandler<UpdateHierarchyClassTraitCommand> commandHandler,
            ICommandHandler<UpdateItemSubTeamByHierarchyClassCommand> updateItemsSubTeamCommandHandler,
            ILogger logger)
        {
            this.commandHandler = commandHandler;
            this.updateItemsSubTeamCommandHandler = updateItemsSubTeamCommandHandler;
            this.logger = logger;
        }

        public void Execute(UpdateHierarchyClassTraitCommand data)
        {
            this.commandHandler.Execute(data);

            if (!data.SubteamChanged
                || data.UpdatedHierarchyClass.hierarchyID != Hierarchies.Merchandise
                || data.UpdatedHierarchyClass.hierarchyLevel != HierarchyLevels.SubBrick)
            {
                return;
            }

            if (data.SubTeamHierarchyClassId == 0)
            {
                throw new ArgumentException($"Cannot update all items associated to the sub-brick because the SubTeam Association is not set.");
            }

            var updateItemsCommand = new UpdateItemSubTeamByHierarchyClassCommand
            {
                HierarchyClassId = data.UpdatedHierarchyClass.hierarchyClassID,
                SubTeamHierarchyClassId = data.SubTeamHierarchyClassId,
                UserName = data.UserName,
                ModifiedDateTimeUtc = data.ModifiedDateTimeUtc
            };

            this.logger.Info($"Updating the Sub-Team association for all items associated to Merchandise SubBrick with HierarchyClassID: {data.UpdatedHierarchyClass.hierarchyClassID}.");
            this.updateItemsSubTeamCommandHandler.Execute(updateItemsCommand);
        }
    }
}
