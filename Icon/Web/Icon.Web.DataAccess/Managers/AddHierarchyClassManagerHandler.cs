using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Common.Validators;

namespace Icon.Web.DataAccess.Managers
{
    public class AddHierarchyClassManagerHandler : IManagerHandler<AddHierarchyClassManager>
    {
        private IconContext context;
        private ICommandHandler<AddHierarchyClassCommand> addHierarchyClassHandler;
        private ICommandHandler<AddSubTeamEventsCommand> addSubTeamEventsHandler;
        private ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler;
        private IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>> getRegionalSettingsBySettingsKeyNameQuery;
        private IMapper mapper;

        public AddHierarchyClassManagerHandler(
            IconContext context,
            ICommandHandler<AddHierarchyClassCommand> hierarchyClassHandler,
            ICommandHandler<AddHierarchyClassMessageCommand> addHierarchyClassMessageHandler,
            ICommandHandler<AddSubTeamEventsCommand> addSubTeamEventsHandler,
            IQueryHandler<GetRegionalSettingsBySettingsKeyNameParameters, List<RegionalSettingsModel>> getRegionalSettingsBySettingsKeyNameQuery,
            IMapper mapper)
        {
            this.context = context;
            this.addHierarchyClassHandler = hierarchyClassHandler;
            this.addHierarchyClassMessageHandler = addHierarchyClassMessageHandler;
            this.addSubTeamEventsHandler = addSubTeamEventsHandler;
            this.getRegionalSettingsBySettingsKeyNameQuery = getRegionalSettingsBySettingsKeyNameQuery;
            this.mapper = mapper;
        }

        public void Execute(AddHierarchyClassManager data)
        {
            ValidateHierarchyClass(data);
            using (var transaction = this.context.Database.BeginTransaction())
            {
                AddHierarchyClassCommand addHierarchyClassCommand = mapper.Map<AddHierarchyClassCommand>(data);

                try
                {
                    addHierarchyClassHandler.Execute(addHierarchyClassCommand);

                    if (addHierarchyClassCommand.NewHierarchyClass.hierarchyID == Hierarchies.Financial)
                    {
                        List<RegionalSettingsModel> posSubTeamSettings = getRegionalSettingsBySettingsKeyNameQuery.Search(new GetRegionalSettingsBySettingsKeyNameParameters { SettingsKeyName = ConfigurationConstants.SendSubTeamUpdatesToIRMASettingsKey });
                        addSubTeamEventsHandler.Execute(new AddSubTeamEventsCommand
                        {
                            HierarchyClassId = addHierarchyClassCommand.NewHierarchyClass.hierarchyClassID,
                            PosSubTeamRegionalSettingsList = posSubTeamSettings
                        });
                    }

                    AddHierarchyClassMessageCommand addHierarchyClassMessageCommand = new AddHierarchyClassMessageCommand
                    {
                        ClassNameChange = true,
                        HierarchyClass = addHierarchyClassCommand.NewHierarchyClass
                    };
                    addHierarchyClassMessageHandler.Execute(addHierarchyClassMessageCommand);

                    transaction.Commit();
                }
                catch (DuplicateValueException exception)
                {
                    transaction.Rollback();
                    throw new CommandException(exception.Message, exception);
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("Error adding HierarchyClassName {0}, hierarchyID {1}, parentId {2}.",
                        data.NewHierarchyClass.hierarchyClassName, data.NewHierarchyClass.hierarchyID, data.NewHierarchyClass.hierarchyParentClassID), exception);
                }
            }
        }

        private void ValidateHierarchyClass(AddHierarchyClassManager data)
        {
            if (data.NewHierarchyClass.hierarchyID == Hierarchies.Merchandise && data.NewHierarchyClass.hierarchyLevel == HierarchyLevels.SubBrick)
            {
                var validationResult = new SubBrickCodeValidator().Validate(data.SubBrickCode);
                if (!validationResult.IsValid)
                {
                    throw new ArgumentException(validationResult.Error);
                }
                else if (context.HierarchyClassTrait.Any(hct => hct.traitID == Traits.SubBrickCode && hct.traitValue == data.SubBrickCode))
                {
                    throw new ArgumentException(string.Format("Sub-Brick Code {0} already exists. Sub-Brick Codes must be unique.", data.SubBrickCode));
                }
            }
        }
    }
}