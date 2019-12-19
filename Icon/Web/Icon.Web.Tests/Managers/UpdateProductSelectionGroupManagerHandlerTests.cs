using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Framework;
using Icon.Web.DataAccess.Managers;
using Moq;
using Icon.Web.DataAccess.Commands;
using Icon.Common.DataAccess;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass] [Ignore]
    public class UpdateProductSelectionGroupManagerHandlerTests
    {
        private IconContext context;
        private UpdateProductSelectionGroupManager manager;
        private UpdateProductSelectionGroupManagerHandler managerHandler;
        
        private Mock<ICommandHandler<UpdateProductSelectionGroupCommand>> mockUpdatePsgHandler;
        private Mock<ICommandHandler<AddProductSelectionGroupMessageCommand>> mockAddPsgMessageHandler;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IconContext();
            
            this.mockUpdatePsgHandler = new Mock<ICommandHandler<UpdateProductSelectionGroupCommand>>();
            this.mockAddPsgMessageHandler = new Mock<ICommandHandler<AddProductSelectionGroupMessageCommand>>();
            
            
            this.manager = new UpdateProductSelectionGroupManager();
            this.managerHandler = new UpdateProductSelectionGroupManagerHandler(this.context, this.mockUpdatePsgHandler.Object, this.mockAddPsgMessageHandler.Object);
        }

        [TestMethod]
        public void UpdateProductSelectionGroup_UpdatePsgManagerObject_UpdatePsgCommandHandlerCalledOneTime()
        {
            // When
            this.managerHandler.Execute(this.manager);

            // Then
            this.mockUpdatePsgHandler.Verify(u => u.Execute(It.IsAny<UpdateProductSelectionGroupCommand>()), Times.Once, "Update Psg Command Handler was not called one time.");
        }

        [TestMethod]
        public void UpdateProductSelectionGroup_PsgNameChanged_AddPsgMessageCommandHandlerCalledOnetime()
        {
            // Given
            this.mockUpdatePsgHandler.Setup(u => u.Execute(It.IsAny<UpdateProductSelectionGroupCommand>()))
                .Callback<UpdateProductSelectionGroupCommand>(u => u.ProductSelectionGroupNameChanged = true);

            // When
            this.managerHandler.Execute(this.manager);

            // Then
            this.mockAddPsgMessageHandler.Verify(u => u.Execute(It.IsAny<AddProductSelectionGroupMessageCommand>()), Times.Once,
                "Add Psg Message Command Handler was not called one time.");
        }

        [TestMethod]
        public void UpdateProductSelectionGroup_PsgNameDidNotChanged_AddPsgMessageCommandHandlerCalledZeroTimes()
        {
            // Given
            this.mockUpdatePsgHandler.Setup(u => u.Execute(It.IsAny<UpdateProductSelectionGroupCommand>()))
                .Callback<UpdateProductSelectionGroupCommand>(u => u.ProductSelectionGroupNameChanged = false);

            // When
            this.managerHandler.Execute(this.manager);

            // Then
            this.mockAddPsgMessageHandler.Verify(u => u.Execute(It.IsAny<AddProductSelectionGroupMessageCommand>()), Times.Never,
                "Add Psg Message Command Handler was called when it should not have been.");
        }

        [TestMethod]
        public void UpdateProductSelectionGroup_PsgTypeChanged_AddPsgMessageCommandHandlerCalledOneTime()
        {
            // Given
            this.mockUpdatePsgHandler.Setup(u => u.Execute(It.IsAny<UpdateProductSelectionGroupCommand>()))
                .Callback<UpdateProductSelectionGroupCommand>(u => u.ProductSelectionGroupTypeChanged = true);

            // When
            this.managerHandler.Execute(this.manager);

            // Then
            this.mockAddPsgMessageHandler.Verify(u => u.Execute(It.IsAny<AddProductSelectionGroupMessageCommand>()), Times.Once,
                "Add Psg Message Command Handler was not called one time.");
        }
    }
}
