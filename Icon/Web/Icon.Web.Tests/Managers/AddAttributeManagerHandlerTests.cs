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
    public class AddAttributeManagerHandlerTests
    {
        private AddAttributeManagerHandler managerHandler;
        private Mock<ICommandHandler<AddAttributeCommand>> mockAddAttributeCommand;
        private Mock<ICommandHandler<AddUpdateCharacterSetCommand>> mockAddUpdateCharacterSetCommand;
        private Mock<ICommandHandler<AddUpdatePickListDataCommand>> mockAddUpdatePickListDataCommand;
        private Mock<ICommandHandler<AddAttributeMessageCommand>> mockAddAttributeMessageCommand;
        private AttributeModel attribute;

        [TestInitialize]
        public void Initialize()
        {
            mockAddAttributeCommand = new Mock<ICommandHandler<AddAttributeCommand>>();
            mockAddUpdateCharacterSetCommand = new Mock<ICommandHandler<AddUpdateCharacterSetCommand>>();
            mockAddUpdatePickListDataCommand = new Mock<ICommandHandler<AddUpdatePickListDataCommand>>();
            mockAddAttributeMessageCommand = new Mock<ICommandHandler<AddAttributeMessageCommand>>();

            managerHandler = new AddAttributeManagerHandler(
                mockAddAttributeCommand.Object, 
                mockAddUpdateCharacterSetCommand.Object, 
                mockAddUpdatePickListDataCommand.Object,
                mockAddAttributeMessageCommand.Object);
            attribute = new AttributeModel();
        }

        [TestMethod]
        public void AddAttribute_SuccessfulExecution_AllCommandsShouldBeCalled()
        {
            // Given.
            var manager = new AddAttributeManager()
            {
                Attribute = attribute
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockAddAttributeCommand.Verify(c => c.Execute(It.IsAny<AddAttributeCommand>()), Times.Once);
            mockAddUpdateCharacterSetCommand.Verify(c => c.Execute(It.IsAny<AddUpdateCharacterSetCommand>()), Times.Once);
            mockAddUpdatePickListDataCommand.Verify(c => c.Execute(It.IsAny<AddUpdatePickListDataCommand>()), Times.Once);
            mockAddAttributeMessageCommand.Verify(c => c.Execute(It.IsAny<AddAttributeMessageCommand>()), Times.Once);
        }
    }
}