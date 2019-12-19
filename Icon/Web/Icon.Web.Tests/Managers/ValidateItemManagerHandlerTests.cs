using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Web.Tests.Unit.ManagerHandlers
{
    [TestClass] [Ignore]
    public class ValidateItemManagerHandlerTests
    {
        private ValidateItemManagerHandler managerHandler;
        private ValidateItemManager manager;
        private Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>> mockBulkImportCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockBulkImportCommandHandler = new Mock<ICommandHandler<BulkImportCommand<BulkImportItemModel>>>();
            managerHandler = new ValidateItemManagerHandler(mockBulkImportCommandHandler.Object);
            manager = new ValidateItemManager { ScanCode = "111" };
        }

        [TestMethod]
        public void UpdateItemManager_ValidManager_ShouldCallCommandHandler()
        {
            //When
            managerHandler.Execute(manager);

            //Then
            mockBulkImportCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }

        [TestMethod]
        public void UpdateItemManager_BulkImportThrowsException_ShouldThrowCommandException()
        {
            //Given
            mockBulkImportCommandHandler.Setup(c => c.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()))
                .Throws(new Exception("Test Exception"));

            //When
            Exception exception = null;
            try
            {
                managerHandler.Execute(manager);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Then
            Assert.IsNotNull(exception);
            Assert.AreEqual("There was an error validating scan code 111. Error: Test Exception", exception.Message);
            mockBulkImportCommandHandler.Verify(v => v.Execute(It.IsAny<BulkImportCommand<BulkImportItemModel>>()), Times.Once);
        }
    }
}
