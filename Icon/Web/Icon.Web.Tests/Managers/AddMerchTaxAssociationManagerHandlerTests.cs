using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Managers;
using Icon.Web.Mvc.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Web.Tests.Unit.Managers
{
    [TestClass]
    public class AddMerchTaxAssociationManagerHandlerTests
    {
        private AddMerchTaxAssociationManagerHandler managerHandler;
        private Mock<ICommandHandler<ApplyMerchTaxAssociationToItemsCommand>> mockApplyMerchTaxAssociationToItemsCommandHandler;
        private Mock<ICommandHandler<AddMerchTaxMappingCommand>> mockAddMerchTaxMappingCommandHandler;
        int testMerchandiseClassId;
        int testTaxClassId;

        [TestInitialize]
        public void Initialize()
        {
            testMerchandiseClassId = 1;
            testTaxClassId = 2;

            mockAddMerchTaxMappingCommandHandler = new Mock<ICommandHandler<AddMerchTaxMappingCommand>>();
            mockApplyMerchTaxAssociationToItemsCommandHandler = new Mock<ICommandHandler<ApplyMerchTaxAssociationToItemsCommand>>();

            managerHandler = new AddMerchTaxAssociationManagerHandler(
                mockAddMerchTaxMappingCommandHandler.Object,
                mockApplyMerchTaxAssociationToItemsCommandHandler.Object);

            AutoMapperWebConfiguration.Configure();
        }

        [TestMethod]
        public void AddMerchTaxAssociationManager_NewTaxClassIsAppliedToItemsSuccessfully_MappingShouldBeUpdated()
        {
            // Given.
            var manager = new AddMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockApplyMerchTaxAssociationToItemsCommandHandler.Verify(h => h.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>()), Times.Once);
            mockAddMerchTaxMappingCommandHandler.Verify(h => h.Execute(It.IsAny<AddMerchTaxMappingCommand>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddMerchTaxAssociationManager_AutoApplyMappingFails_MappingShouldNotBeCreated()
        {
            // Given.
            var manager = new AddMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            mockApplyMerchTaxAssociationToItemsCommandHandler.Setup(c => c.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>())).Throws(new Exception());

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockApplyMerchTaxAssociationToItemsCommandHandler.Verify(h => h.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>()), Times.Once);
            mockAddMerchTaxMappingCommandHandler.Verify(h => h.Execute(It.IsAny<AddMerchTaxMappingCommand>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void AddMerchTaxAssociationManager_AddMappingFails_ExceptionShouldBeThrown()
        {
            // Given.
            var manager = new AddMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            mockAddMerchTaxMappingCommandHandler.Setup(c => c.Execute(It.IsAny<AddMerchTaxMappingCommand>())).Throws(new Exception());

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockApplyMerchTaxAssociationToItemsCommandHandler.Verify(h => h.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>()), Times.Once);
            mockAddMerchTaxMappingCommandHandler.Verify(h => h.Execute(It.IsAny<AddMerchTaxMappingCommand>()), Times.Once);
        }
    }
}
