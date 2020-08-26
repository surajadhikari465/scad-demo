using Dapper;
using Icon.Common.DataAccess;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using Icon.Common.Models;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class UpdateAttributeManagerHandlerTests
    {
        private UpdateAttributeManagerHandler managerHandler;
        private Mock<ICommandHandler<UpdateAttributeCommand>> mockUpdateAttributeCommand;
        private Mock<ICommandHandler<AddUpdateCharacterSetCommand>> mockAddUpdateCharacterSetCommand;
        private Mock<ICommandHandler<AddUpdatePickListDataCommand>> mockAddUpdatePickListDataCommand;
        private Mock<ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand>> mockAddMissingColumnsToItemColumnDisplayTableCommand;

        [TestInitialize]
        public void Initialize()
        {
            mockUpdateAttributeCommand = new Mock<ICommandHandler<UpdateAttributeCommand>>();
            mockAddUpdateCharacterSetCommand = new Mock<ICommandHandler<AddUpdateCharacterSetCommand>>();
            mockAddUpdatePickListDataCommand = new Mock<ICommandHandler<AddUpdatePickListDataCommand>>();
            mockAddMissingColumnsToItemColumnDisplayTableCommand = new Mock<ICommandHandler<AddMissingColumnsToItemColumnDisplayTableCommand>>();
        }


        private void BuildAttributeManagerHandler()
        {
            managerHandler = new UpdateAttributeManagerHandler(mockUpdateAttributeCommand.Object, 
                mockAddUpdateCharacterSetCommand.Object, 
                mockAddUpdatePickListDataCommand.Object, 
                mockAddMissingColumnsToItemColumnDisplayTableCommand.Object);
        }

        [TestMethod]
        public void UpdateAttribute_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            BuildAttributeManagerHandler();

            var manager = GetUpdateAttributeManager();

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockUpdateAttributeCommand.Verify(c => c.Execute(It.IsAny<UpdateAttributeCommand>()), Times.Once);
            mockAddUpdateCharacterSetCommand.Verify(c => c.Execute(It.IsAny<AddUpdateCharacterSetCommand>()), Times.Once);
            mockAddUpdatePickListDataCommand.Verify(c => c.Execute(It.IsAny<AddUpdatePickListDataCommand>()), Times.Once);
            mockAddMissingColumnsToItemColumnDisplayTableCommand.Verify(c => c.Execute(It.IsAny<AddMissingColumnsToItemColumnDisplayTableCommand>()), Times.Once);
        }

        private UpdateAttributeManager GetUpdateAttributeManager()
        {
            return new UpdateAttributeManager()
            {
                Attribute = new AttributeModel { AttributeId = 1, IsPickList = true},
                CharacterSetModelList = new List<CharacterSetModel>(),
                PickListModel = new List<PickListModel>()
            };
        }
    }
}