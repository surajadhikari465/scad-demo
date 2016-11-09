using AutoMapper;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;
using Icon.Web.DataAccess.Queries;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateSubTeamManagerHandler : IManagerHandler<UpdateSubTeamManager>
    {
        private IconContext context;
        private ICommandHandler<UpdateSubTeamCommand> updateSubTeamHandler;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private ICommandHandler<AddProductMessagesBySubTeamCommand> addProductMessagesForSubTeamsAssociatedItemsCommandHandler;
        private ICommandHandler<UpdateHierarchyClassTraitCommand> updateHierarchyClassTraitHandler;
        private ICommandHandler<AddSubTeamEventsCommand> addSubTeamEventsHandler;
        private IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>> getRegionalSettingsBySettingsKeyNameQuery;

        public UpdateSubTeamManagerHandler(IconContext context,
            ICommandHandler<UpdateSubTeamCommand> updateSubTeamHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            ICommandHandler<AddProductMessagesBySubTeamCommand> addProductMessagesForSubTeamsAssociatedItemsCommandHandler,
            ICommandHandler<UpdateHierarchyClassTraitCommand> hierarchyClassTraitHandler,
            ICommandHandler<AddSubTeamEventsCommand> addSubTeamEventsHandler,
            IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>> getRegionalSettingsBySettingsKeyNameQuery)
        {
            this.context = context;
            this.updateSubTeamHandler = updateSubTeamHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.addProductMessagesForSubTeamsAssociatedItemsCommandHandler = addProductMessagesForSubTeamsAssociatedItemsCommandHandler;
            this.updateHierarchyClassTraitHandler = hierarchyClassTraitHandler;
            this.addSubTeamEventsHandler = addSubTeamEventsHandler;
            this.getRegionalSettingsBySettingsKeyNameQuery = getRegionalSettingsBySettingsKeyNameQuery;
        }

        public void Execute(UpdateSubTeamManager data)
        {
            using(var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    UpdateSubTeamCommand updateSubTeamCommand = Mapper.Map<UpdateSubTeamCommand>(data);
                    updateSubTeamHandler.Execute(updateSubTeamCommand);

                    if (updateSubTeamCommand.PeopleSoftChanged)
                    {
                        AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = new AddHierarchyClassMessageCommand
                        {
                            HierarchyClass = updateSubTeamCommand.UpdatedHierarchyClass,
                            ClassNameChange = updateSubTeamCommand.PeopleSoftChanged
                        };
                        addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);

                        AddProductMessagesBySubTeamCommand addProductMessagesForSubTeamsAssociatedItemsCommand = 
                            new AddProductMessagesBySubTeamCommand
                            {
                                NewSubTeam = updateSubTeamCommand.UpdatedHierarchyClass.hierarchyClassName
                            };
                        addProductMessagesForSubTeamsAssociatedItemsCommandHandler.Execute(addProductMessagesForSubTeamsAssociatedItemsCommand);
                    }

                    // Find the HierarchyClass to be updated.
                    HierarchyClass hierarchyClass = context.HierarchyClass.SingleOrDefault(hc => hc.hierarchyClassID == data.HierarchyClassId);

                    // Update traits 
                    if (hierarchyClass != null)
                    {
                        UpdateHierarchyClassTraitCommand updateHierarchyClassTraitCommand = new UpdateHierarchyClassTraitCommand()
                        {
                            UpdatedHierarchyClass = hierarchyClass,
                            PosDeptNumber = data.PosDeptNumber,
                            TeamName = data.TeamName,
                            TeamNumber = data.TeamNumber,
                            NonAlignedSubteam = data.NonAlignedSubteam
                        };
                        updateHierarchyClassTraitHandler.Execute(updateHierarchyClassTraitCommand);

                        if (hierarchyClass.hierarchyID == Hierarchies.Financial)
                        {
                            List<RegionalSettingsModel> posSubTeamSettings = getRegionalSettingsBySettingsKeyNameQuery.Search(new GetRegionalSettingsBySettingsKeyNameParameters { SettingsKeyName = ConfigurationConstants.SendSubTeamUpdatesToIRMASettingsKey });
                            AddSubTeamEventsCommand addSubTeamEventsCommand = new AddSubTeamEventsCommand()
                            {
                                HierarchyClassId = hierarchyClass.hierarchyClassID,
                                PosSubTeamRegionalSettingsList = posSubTeamSettings
                            };
                            addSubTeamEventsHandler.Execute(addSubTeamEventsCommand);
                        }
                    }

                    transaction.Commit();
                }
                catch (HierarchyClassTraitUpdateException exception)
                {
                    transaction.Rollback();
                    throw new CommandException(exception.Message, exception);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("There was an error updating subteam {0} and Peoplesoft number {1}, POS Dept Number {2}, Team Number {3} and Team Name {4}.  Error: {5}",
                        data.SubTeamName, data.PeopleSoftNumber, data.PosDeptNumber, data.TeamNumber, data.TeamName, ex.Message), ex);
                }
            }
        }
    }
}
