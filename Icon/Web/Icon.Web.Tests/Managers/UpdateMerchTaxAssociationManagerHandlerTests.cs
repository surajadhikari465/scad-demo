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
    public class UpdateMerchTaxAssociationManagerHandlerTests
    {
        private UpdateMerchTaxAssociationManagerHandler managerHandler;
        private Mock<ICommandHandler<ApplyMerchTaxAssociationToItemsCommand>> mockApplyMerchTaxAssociationToItemsCommandHandler;
        private Mock<ICommandHandler<UpdateMerchTaxMappingCommand>> mockUpdateMerchTaxMappingCommandHandler;
        int testMerchandiseClassId;
        int testTaxClassId;

        [TestInitialize]
        public void Initialize()
        {
            testMerchandiseClassId = 1;
            testTaxClassId = 2;

            mockUpdateMerchTaxMappingCommandHandler = new Mock<ICommandHandler<UpdateMerchTaxMappingCommand>>();
            mockApplyMerchTaxAssociationToItemsCommandHandler = new Mock<ICommandHandler<ApplyMerchTaxAssociationToItemsCommand>>();

            managerHandler = new UpdateMerchTaxAssociationManagerHandler(
                mockUpdateMerchTaxMappingCommandHandler.Object,
                mockApplyMerchTaxAssociationToItemsCommandHandler.Object);

            AutoMapperWebConfiguration.Configure();
        }

        [TestMethod]
        public void UpdateMerchTaxAssociationManager_NewTaxClassIsAppliedToItemsSuccessfully_MappingShouldBeUpdated()
        {
            // Given.
            var manager = new UpdateMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockApplyMerchTaxAssociationToItemsCommandHandler.Verify(h => h.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>()), Times.Once);
            mockUpdateMerchTaxMappingCommandHandler.Verify(h => h.Execute(It.IsAny<UpdateMerchTaxMappingCommand>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void UpdateMerchTaxAssociationManager_AutoApplyMappingFails_MappingShouldNotBeCreated()
        {
            // Given.
            var manager = new UpdateMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            mockApplyMerchTaxAssociationToItemsCommandHandler.Setup(c => c.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>())).Throws(new Exception());

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockApplyMerchTaxAssociationToItemsCommandHandler.Verify(h => h.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>()), Times.Once);
            mockUpdateMerchTaxMappingCommandHandler.Verify(h => h.Execute(It.IsAny<UpdateMerchTaxMappingCommand>()), Times.Never);
        }

        [TestMethod]
        [ExpectedException(typeof(CommandException))]
        public void UpdateMerchTaxAssociationManager_AddMappingFails_ExceptionShouldBeThrown()
        {
            // Given.
            var manager = new UpdateMerchTaxAssociationManager
            {
                MerchandiseHierarchyClassId = testMerchandiseClassId,
                TaxHierarchyClassId = testTaxClassId
            };

            mockUpdateMerchTaxMappingCommandHandler.Setup(c => c.Execute(It.IsAny<UpdateMerchTaxMappingCommand>())).Throws(new Exception());

            // When.
            managerHandler.Execute(manager);

            // Then.
            mockApplyMerchTaxAssociationToItemsCommandHandler.Verify(h => h.Execute(It.IsAny<ApplyMerchTaxAssociationToItemsCommand>()), Times.Once);
            mockUpdateMerchTaxMappingCommandHandler.Verify(h => h.Execute(It.IsAny<UpdateMerchTaxMappingCommand>()), Times.Once);
        }
    }
}
