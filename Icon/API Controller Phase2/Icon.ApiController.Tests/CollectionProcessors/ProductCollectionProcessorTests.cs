using Icon.ApiController.Controller.CollectionProcessors;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests.CollectionProcessors
{
    [TestClass]
    public class ProductCollectionProcessorTests
    {
        private IconContext context;
        private ProductCollectionProcessor subject;
        private Mock<IQueryHandler<GetItemsByIdParameters, List<Item>>> mockGetItemsQueryHandler;
        private Serializer<Contracts.items> serializer;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveXmlMessageCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private Mock<ILogger<ProductCollectionProcessor>> mockLogger;
        private Mock<IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass>> mockGetFinancialClassQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            mockGetItemsQueryHandler = new Mock<IQueryHandler<GetItemsByIdParameters, List<Item>>>();
            serializer = new Serializer<Contracts.items>(new Mock<ILogger<Serializer<Contracts.items>>>().Object, new Mock<IEmailClient>().Object);
            mockSaveXmlMessageCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            mockProducer = new Mock<IEsbProducer>();
            mockLogger = new Mock<ILogger<ProductCollectionProcessor>>();
            mockGetFinancialClassQuery = new Mock<IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass>>();

            subject = new ProductCollectionProcessor(
                mockLogger.Object,
                serializer,
                mockGetItemsQueryHandler.Object,
                mockGetFinancialClassQuery.Object,
                mockSaveXmlMessageCommandHandler.Object,
                mockUpdateMessageHistoryCommandHandler.Object,
                mockProducer.Object);
        }

        [TestMethod]
        public void ProductCollectionProcessor_GetItemsSearchReturnsNoResults_ReturnsWithoutCallingAnyDependencies()
        {
            // Given.
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsByIdParameters>())).Returns(new List<Item>());

            // When.
            subject.GenerateMessages(new List<int>());

            // Then.
            mockSaveXmlMessageCommandHandler.Verify(m => m.Execute(It.IsAny<SaveToMessageHistoryCommand<MessageHistory>>()), Times.Never);
            mockProducer.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()), Times.Never);
            mockLogger.Verify(m => m.Info(It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Warn(It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ProductCollectionProcessor_BuildProductMessage_MessageShouldContainItemSignAttributes()
        {
            // Given.
            HierarchyClass testBrand = new TestHierarchyClassBuilder();
            HierarchyClass testMerchandise = new TestHierarchyClassBuilder();
            HierarchyClass testTax = new TestHierarchyClassBuilder();

            int brandId = testBrand.hierarchyClassID;
            int subBrickId = testMerchandise.hierarchyClassID;
            int taxClassId = testTax.hierarchyClassID;

            Item testItem = new TestItemBuilder()
                .WithBrandAssociation(brandId)
                .WithSubBrickAssociation(subBrickId)
                .WithTaxClassAssociation(taxClassId);

            var itemTraits = new List<ItemTrait>
            {
                new ItemTrait { itemID = testItem.itemID, traitID = Traits.ProductDescription, traitValue = "ProductCollectionProcessor Product Description", localeID = 1 },
                new ItemTrait { itemID = testItem.itemID, traitID = Traits.PosDescription, traitValue = "ProductCollectionProcessor POS Description", localeID = 1 },
                new ItemTrait { itemID = testItem.itemID, traitID = Traits.PackageUnit, traitValue = "9", localeID = 1 },
                new ItemTrait { itemID = testItem.itemID, traitID = Traits.RetailSize, traitValue = "9", localeID = 1 },
                new ItemTrait { itemID = testItem.itemID, traitID = Traits.RetailUom, traitValue = "9", localeID = 1 },
                new ItemTrait { itemID = testItem.itemID, traitID = Traits.FoodStampEligible, traitValue = "1", localeID = 1 }
            };

            testItem.ItemType = context.ItemType.Single(it => it.itemTypeCode == ItemTypeCodes.RetailSale);
            testItem.ScanCode.Single().ScanCodeType = context.ScanCodeType.Single(sct => sct.scanCodeTypeID == ScanCodeTypes.Upc);
            
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsByIdParameters>())).Returns(new List<Item>
            {
                testItem
            });

            string message = string.Empty;

            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Callback<string, Dictionary<string, string>>((m, p) =>
                {
                    message = m;
                });

            // When.
            subject.GenerateMessages(new List<int>());

            // Then.
            Assert.AreEqual(File.ReadAllText(@"TestMessages\product_collection_sample_with_sign_attributes.xml"), message);
        }
    }
}
