using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.Common;
using System.Collections.Generic;
using GlobalEventController.Controller.EventServices;
using Irma.Framework;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.BulkCommands;
using Moq;
using System.Linq;
using GlobalEventController.Testing.Common;

namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class BulkItemNutriFactsServiceTests
    {
        private BulkItemNutriFactsService bulkService;
        private Mock<ICommandHandler<BulkAddUpdateLastChangeCommand>> mockLastChangeHandler; 
        private Mock<IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>>> bulkGetValidatedItemsWithTax;
        private Mock<ICommandHandler<BulkUpdateNutriFactsCommand>> mockBulkUpdateNutriFactsHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLastChangeHandler = new Mock<ICommandHandler<BulkAddUpdateLastChangeCommand>>();
            bulkGetValidatedItemsWithTax = new Mock<IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>>>();
            this.mockBulkUpdateNutriFactsHandler = new Mock<ICommandHandler<BulkUpdateNutriFactsCommand>>();

            this.bulkService = new BulkItemNutriFactsService(
                this.mockBulkUpdateNutriFactsHandler.Object,
                this.mockLastChangeHandler.Object,
                this.bulkGetValidatedItemsWithTax.Object);
        }

        [TestMethod]
        public void BulkItemEventService_ValidatedItemList_EachCommandHandlerCalledOneTime()
        {
            // Given
            this.bulkService.ValidatedItemList = GetValidatedItemList();
            this.bulkService.ScanCodesWithNoTaxList = GetValidatedItemList().Select(vi => vi.ScanCode).ToList();
            this.bulkGetValidatedItemsWithTax.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithTaxClassQuery>())).Returns(GetValidatedItemList());
            this.mockBulkUpdateNutriFactsHandler.Setup(bn => bn.Handle(It.IsAny<BulkUpdateNutriFactsCommand>()));
            
            // When
            this.bulkService.Run();

            // Then            
            this.bulkGetValidatedItemsWithTax.Verify(ab => ab.Handle(It.IsAny<BulkGetItemsWithTaxClassQuery>()), Times.Once,
                "The BulkAddTaxCommandHandler was not called exactly one time.");
            this.mockLastChangeHandler.Verify(ab => ab.Handle(It.IsAny<BulkAddUpdateLastChangeCommand>()), Times.Once,
                "The BulkAddUpdateLastChangeCommandHandler was not called exactly one time.");
            this.mockBulkUpdateNutriFactsHandler.Verify(bn => bn.Handle(It.IsAny<BulkUpdateNutriFactsCommand>()), Times.Once, "The BulkUpdateNutriFactsCommandHandler was not called exactly one time");
        }

        private List<ValidatedItemModel> GetValidatedItemList()
        {
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(1).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(2).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(3).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(4).Build());

            return validatedItems;
        }
    }
}
