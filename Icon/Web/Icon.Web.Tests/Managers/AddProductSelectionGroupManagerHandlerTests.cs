using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Moq;
using Icon.Framework;
using Icon.Common.DataAccess;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass] [Ignore]
    public class AddProductSelectionGroupManagerHandlerTests
    {
        private IconContext context;

        private AddProductSelectionGroupManager manager;
        private AddProductSelectionGroupManagerHandler managerHandler;
        private Mock<ICommandHandler<AddProductSelectionGroupCommand>> mockAddPsgHandler;
        private Mock<ICommandHandler<AddProductSelectionGroupMessageCommand>> mockAddPsgMessageHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IconContext();

            this.mockAddPsgHandler = new Mock<ICommandHandler<AddProductSelectionGroupCommand>>();
            this.mockAddPsgMessageHandler = new Mock<ICommandHandler<AddProductSelectionGroupMessageCommand>>();

            this.manager = new AddProductSelectionGroupManager();

            this.managerHandler = new AddProductSelectionGroupManagerHandler(this.context, this.mockAddPsgHandler.Object, this.mockAddPsgMessageHandler.Object);
        }

        [TestMethod]
        public void AddProductSelectionGroupManager_ManagerObject_AddProductSelectionGroupCommandHandlerCalled()
        {
            // When
            this.managerHandler.Execute(this.manager);

            // Then
            this.mockAddPsgHandler.Verify(p => p.Execute(It.IsAny<AddProductSelectionGroupCommand>()), Times.Once, "AddProductSelectionGroupCommandHandler was not called one time.");
        }

        [TestMethod]
        public void AddProductSelectionGroupManager_ManagerObject_AddProductSelectionGroupMessageCommandHandlerCalled()
        {
            // When
            this.managerHandler.Execute(this.manager);

            // Then
            this.mockAddPsgHandler.Verify(p => p.Execute(It.IsAny<AddProductSelectionGroupCommand>()), Times.Once, "AddProductSelectionGroupCommandHandler was not called one time.");
        }
    }
}
