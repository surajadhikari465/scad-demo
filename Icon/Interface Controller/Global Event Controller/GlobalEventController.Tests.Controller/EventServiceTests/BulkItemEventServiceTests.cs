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
using Icon.Framework;
using GlobalEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Queries;
using Icon.Logging;
using GlobalEventController.Testing.Common;

namespace GlobalEventController.Tests.Controller.EventServiceTests
{
    [TestClass]
    public class BulkItemEventServiceTests
    {
        private BulkItemEventService bulkService;
        private IrmaContext context;
        private Mock<ILogger<BulkItemEventService>> mockLogger;
        private Mock<ICommandHandler<BulkAddBrandCommand>> mockBulkAddBrandCommandHandler;
        private Mock<ICommandHandler<BulkAddUpdateLastChangeCommand>> mockBulkAddUpdateLastChangeCommandHandler;
        private Mock<ICommandHandler<BulkUpdateItemCommand>> mockBulkUpdateItemCommandHandler;
        private Mock<ICommandHandler<BulkAddValidatedScanCodeCommand>> mockBulkAddValidatedScanCodeCommandHandler;
        private Mock<IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>>> mockBulkBulkGetItemsWithTaxClassQueryHandler;
        private Mock<ICommandHandler<BulkUpdateNutriFactsCommand>> mockBulkUpdateNutriFactsCommandHandler;
        private Mock<ICommandHandler<BulkUpdateItemSignAttributesCommand>> mockUpdateItemSignAttributesCommandHandler;
        private Mock<IEmailUomChangeService> mockEmailUomChangeService;
        private Mock<IQueryHandler<BulkGetItemsWithNoNatlClassQuery, List<ValidatedItemModel>>> mockBulkGetValidatedItemsWithNoNatlClassQueryHandler;
        private Mock<IQueryHandler<BulkGetItemsWithNoRetailUomQuery, List<ValidatedItemModel>>> mockBulkGetValidatedItemsWithNoRetailUomQueryHandler;


        [TestInitialize]
        public void InitializeData()
        {
            this.context = new IrmaContext();
            this.mockLogger = new Mock<ILogger<BulkItemEventService>>();
            this.mockBulkAddBrandCommandHandler = new Mock<ICommandHandler<BulkAddBrandCommand>>();
            this.mockBulkAddUpdateLastChangeCommandHandler = new Mock<ICommandHandler<BulkAddUpdateLastChangeCommand>>();
            this.mockBulkUpdateItemCommandHandler = new Mock<ICommandHandler<BulkUpdateItemCommand>>();
            this.mockBulkAddValidatedScanCodeCommandHandler = new Mock<ICommandHandler<BulkAddValidatedScanCodeCommand>>();
            this.mockBulkBulkGetItemsWithTaxClassQueryHandler = new Mock<IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>>>();
            this.mockBulkUpdateNutriFactsCommandHandler = new Mock<ICommandHandler<BulkUpdateNutriFactsCommand>>();
            this.mockUpdateItemSignAttributesCommandHandler = new Mock<ICommandHandler<BulkUpdateItemSignAttributesCommand>>();
            this.mockEmailUomChangeService = new Mock<IEmailUomChangeService>();
            this.mockBulkGetValidatedItemsWithNoNatlClassQueryHandler = new Mock<IQueryHandler<BulkGetItemsWithNoNatlClassQuery, List<ValidatedItemModel>>>();
            this.mockBulkGetValidatedItemsWithNoRetailUomQueryHandler = new Mock<IQueryHandler<BulkGetItemsWithNoRetailUomQuery, List<ValidatedItemModel>>>();

            this.bulkService = new BulkItemEventService(this.context,
                this.mockLogger.Object,
                this.mockBulkAddBrandCommandHandler.Object,
                this.mockBulkAddUpdateLastChangeCommandHandler.Object,
                this.mockBulkUpdateItemCommandHandler.Object,
                this.mockBulkAddValidatedScanCodeCommandHandler.Object,
                this.mockBulkBulkGetItemsWithTaxClassQueryHandler.Object,
                this.mockBulkUpdateNutriFactsCommandHandler.Object,
                this.mockUpdateItemSignAttributesCommandHandler.Object,
                this.mockBulkGetValidatedItemsWithNoNatlClassQueryHandler.Object,
                this.mockBulkGetValidatedItemsWithNoRetailUomQueryHandler.Object);

            this.bulkService.RegionalItemMessage = new List<RegionalItemMessageModel>();
            this.bulkService.Region = "FL";
        }

        [TestMethod]
        public void BulkItemEventService_ValidatedItemList_EachCommandHandlerCalledOneTime()
        {
            // Given
            this.bulkService.ValidatedItemList = GetValidatedItemList();
            this.bulkService.ScanCodesWithNoTaxList = GetValidatedItemList().Select(vi => vi.ScanCode).ToList();
            this.mockBulkBulkGetItemsWithTaxClassQueryHandler.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithTaxClassQuery>())).Returns(GetValidatedItemList());
            this.mockBulkUpdateNutriFactsCommandHandler.Setup(bn => bn.Handle(It.IsAny<BulkUpdateNutriFactsCommand>()));
            this.bulkService.ItemNutriFacts = new List<NutriFactsModel>() { new NutriFactsModel() { Plu = "Test PLU" } };
            this.mockBulkGetValidatedItemsWithNoNatlClassQueryHandler.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithNoNatlClassQuery>())).Returns(GetValidatedItemList());
            this.mockBulkGetValidatedItemsWithNoRetailUomQueryHandler.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithNoRetailUomQuery>())).Returns(GetValidatedItemList());

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[0].ScanCode });
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[1].ScanCode });
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[2].ScanCode });
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[3].ScanCode });

            // When
            this.bulkService.Run();

            // Then
            this.mockBulkAddBrandCommandHandler.Verify(ab => ab.Handle(It.IsAny<BulkAddBrandCommand>()), Times.Once,
                "The BulkAddBrandCommandHandler was not called exactly one time.");
            this.mockBulkBulkGetItemsWithTaxClassQueryHandler.Verify(ab => ab.Handle(It.IsAny<BulkGetItemsWithTaxClassQuery>()), Times.Once,
                "The BulkAddTaxCommandHandler was not called exactly one time.");
            this.mockBulkAddUpdateLastChangeCommandHandler.Verify(ab => ab.Handle(It.IsAny<BulkAddUpdateLastChangeCommand>()), Times.Once,
                "The BulkAddUpdateLastChangeCommandHandler was not called exactly one time.");
            this.mockBulkUpdateItemCommandHandler.Verify(ab => ab.Handle(It.IsAny<BulkUpdateItemCommand>()), Times.Once,
                "The BulkUpdateItemCommandHandler was not called exactly one time.");
            this.mockBulkAddValidatedScanCodeCommandHandler.Verify(ab => ab.Handle(It.IsAny<BulkAddValidatedScanCodeCommand>()), Times.Once,
                "The BulkAddValidatedScanCodeCommandHandler was not called exactly one time.");
            this.mockBulkUpdateNutriFactsCommandHandler.Verify(bn => bn.Handle(It.IsAny<BulkUpdateNutriFactsCommand>()), Times.Once, 
                "The BulkUpdateNutriFactsCommandHandler was not called exactly one time");
            this.mockUpdateItemSignAttributesCommandHandler.Verify(m => m.Handle(
                It.Is<BulkUpdateItemSignAttributesCommand>(c => c.ValidatedItems.All(i => i.HasItemSignAttributes))), 
                Times.Once);
            this.mockBulkGetValidatedItemsWithNoNatlClassQueryHandler.Verify(ab => ab.Handle(It.IsAny<BulkGetItemsWithNoNatlClassQuery>()), Times.Once,
                "The BulkGetValidatedItemsWithNoNatlClassQueryHandler was not called exactly one time.");
            this.mockBulkGetValidatedItemsWithNoRetailUomQueryHandler.Verify(ab => ab.Handle(It.IsAny<BulkGetItemsWithNoRetailUomQuery>()), Times.Once,
                "The BulkGetValidatedItemsWithNoRetailUomQueryHandler was not called exactly one time.");
        }

        [TestMethod]
        public void BulkItemEventService_ValidatedItemTaxNationalClassRetailUomNotFound_ItemProcessedAndMessagePopulated()
        {
            // Given
            this.bulkService.ValidatedItemList = GetValidatedItemList();
            this.bulkService.ScanCodesWithNoTaxList = GetValidatedItemList().Select(vi => vi.ScanCode).ToList();
            this.mockBulkBulkGetItemsWithTaxClassQueryHandler.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithTaxClassQuery>())).Returns(GetEmptyValidatedItemsWithTaxList());
            this.mockBulkUpdateNutriFactsCommandHandler.Setup(bn => bn.Handle(It.IsAny<BulkUpdateNutriFactsCommand>()));
            this.bulkService.ItemNutriFacts = new List<NutriFactsModel>() { new NutriFactsModel() { Plu = "Test PLU" } };
            this.mockBulkGetValidatedItemsWithNoNatlClassQueryHandler.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithNoNatlClassQuery>())).Returns(GetValidatedItemList());
            this.mockBulkGetValidatedItemsWithNoRetailUomQueryHandler.Setup(bv => bv.Handle(It.IsAny<BulkGetItemsWithNoRetailUomQuery>())).Returns(GetValidatedItemList());

            List<IrmaItemModel> irmaItems = new List<IrmaItemModel>();
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[0].ScanCode });
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[1].ScanCode });
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[2].ScanCode });
            irmaItems.Add(new IrmaItemModel { Identifier = this.bulkService.ValidatedItemList[3].ScanCode });

            // When
            this.bulkService.Run();

            // Then
            Assert.IsTrue(this.bulkService.RegionalItemMessage[0].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[0].Identifier.Equals(this.bulkService.ValidatedItemList[0].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[0].Message.StartsWith("Tax Class doesn’t exist with tax name - " + this.bulkService.ValidatedItemList[0].TaxClassName));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[1].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[1].Identifier.Equals(this.bulkService.ValidatedItemList[1].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[1].Message.StartsWith("Tax Class doesn’t exist with tax name - " + this.bulkService.ValidatedItemList[1].TaxClassName));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[2].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[2].Identifier.Equals(this.bulkService.ValidatedItemList[2].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[2].Message.StartsWith("Tax Class doesn’t exist with tax name - " + this.bulkService.ValidatedItemList[2].TaxClassName));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[3].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[3].Identifier.Equals(this.bulkService.ValidatedItemList[3].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[3].Message.StartsWith("Tax Class doesn’t exist with tax name - " + this.bulkService.ValidatedItemList[3].TaxClassName));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[4].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[4].Identifier.Equals(this.bulkService.ValidatedItemList[0].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[4].Message.StartsWith("National Class doesn't exist with ClassId - " + this.bulkService.ValidatedItemList[0].NationalClassCode));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[5].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[5].Identifier.Equals(this.bulkService.ValidatedItemList[1].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[5].Message.StartsWith("National Class doesn't exist with ClassId - " + this.bulkService.ValidatedItemList[1].NationalClassCode));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[6].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[6].Identifier.Equals(this.bulkService.ValidatedItemList[2].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[6].Message.StartsWith("National Class doesn't exist with ClassId - " + this.bulkService.ValidatedItemList[2].NationalClassCode));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[7].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[7].Identifier.Equals(this.bulkService.ValidatedItemList[3].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[7].Message.StartsWith("National Class doesn't exist with ClassId - " + this.bulkService.ValidatedItemList[3].NationalClassCode));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[8].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[8].Identifier.Equals(this.bulkService.ValidatedItemList[0].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[8].Message.StartsWith("Retail UOM doesn't exist with UOM abbreviation - " + this.bulkService.ValidatedItemList[0].RetailUom));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[9].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[9].Identifier.Equals(this.bulkService.ValidatedItemList[1].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[9].Message.StartsWith("Retail UOM doesn't exist with UOM abbreviation - " + this.bulkService.ValidatedItemList[1].RetailUom));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[10].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[10].Identifier.Equals(this.bulkService.ValidatedItemList[2].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[10].Message.StartsWith("Retail UOM doesn't exist with UOM abbreviation - " + this.bulkService.ValidatedItemList[2].RetailUom));

            Assert.IsTrue(this.bulkService.RegionalItemMessage[11].RegionCode.Equals(this.bulkService.Region));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[11].Identifier.Equals(this.bulkService.ValidatedItemList[3].ScanCode));
            Assert.IsTrue(this.bulkService.RegionalItemMessage[11].Message.StartsWith("Retail UOM doesn't exist with UOM abbreviation - " + this.bulkService.ValidatedItemList[3].RetailUom));

        }
        private List<ValidatedItemModel> GetValidatedItemList()
        {
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(1).WithScanCode("12344").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(2).WithScanCode("12345").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(3).WithScanCode("12346").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());
            validatedItems.Add(new TestValidatedItemModelBuilder().WithItemId(4).WithScanCode("12347").WithEventTypeId(EventTypes.ItemValidation).WithHasItemSignAttributes(true).Build());

            return validatedItems;
        }

        private List<ValidatedItemModel> GetEmptyValidatedItemsWithTaxList()
        {
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();

            return validatedItems;
        }
    }
}
