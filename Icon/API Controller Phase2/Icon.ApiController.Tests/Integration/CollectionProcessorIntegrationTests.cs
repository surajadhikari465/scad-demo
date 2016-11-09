using Icon.ApiController.Controller.CollectionProcessors;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests.Integration
{
    [TestClass]
    public class CollectionProcessorIntegrationTests
    {
        private ProductCollectionProcessor productCollectionProcessor;
        private GlobalIconContext globalContext;
        private IconContext context;
        private Mock<IQueryHandler<GetItemsByIdParameters, List<Item>>> mockGetItemsQueryHandler;
        private Mock<ISerializer<Contracts.items>> mockSerializer;
        private Serializer<Contracts.items> serializer;
        private Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>> mockSaveXmlMessageCommandHandler;
        private SaveToMessageHistoryCommandHandler saveXmlMessageCommandHandler;
        private Mock<IEsbProducer> mockProducer;
        private Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>> mockUpdateMessageHistoryCommandHandler;
        private UpdateMessageHistoryStatusCommandHandler updateMessageHistoryCommandHandler;
        private Mock<ILogger<ProductCollectionProcessor>> mockLogger;
        private Mock<IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass>> mockGetFinancialClassQuery;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);
            mockGetItemsQueryHandler = new Mock<IQueryHandler<GetItemsByIdParameters, List<Item>>>();
            mockSerializer = new Mock<ISerializer<Contracts.items>>();
            serializer = new Serializer<Contracts.items>(new Mock<ILogger<Serializer<Contracts.items>>>().Object, new Mock<IEmailClient>().Object);
            mockSaveXmlMessageCommandHandler = new Mock<ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>>>();
            saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(new Mock<ILogger<SaveToMessageHistoryCommandHandler>>().Object, globalContext);
            mockUpdateMessageHistoryCommandHandler = new Mock<ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>>>();
            updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new Mock<ILogger<UpdateMessageHistoryStatusCommandHandler>>().Object, globalContext);
            mockProducer = new Mock<IEsbProducer>();
            mockLogger = new Mock<ILogger<ProductCollectionProcessor>>();
            mockGetFinancialClassQuery = new Mock<IQueryHandler<GetFinancialClassByMerchandiseClassParameters, HierarchyClass>>();

            productCollectionProcessor = new ProductCollectionProcessor(
                mockLogger.Object,
                serializer,
                mockGetItemsQueryHandler.Object,
                mockGetFinancialClassQuery.Object,
                saveXmlMessageCommandHandler,
                updateMessageHistoryCommandHandler,
                mockProducer.Object);
        }

        [TestMethod]
        public void ProductCollectionProcessor_GetItemsQueryReturnsThreeItems_ThreeItemsShouldBeSentToEsb()
        {
            // Given.
            mockProducer.Setup(p => p.Send(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsByIdParameters>())).Returns(CreateFakeItems());
            mockGetFinancialClassQuery.Setup(m => m.Search(It.IsAny<GetFinancialClassByMerchandiseClassParameters>())).Returns(new HierarchyClass
            {
                hierarchyClassID = 1,
                hierarchyClassName = "FinancialHierarchy (1111)",
                hierarchyLevel = 1,
                hierarchyParentClassID = null
            });

            // When.
            productCollectionProcessor.GenerateMessages(new List<int> { 1, 2, 3 });

            // Then.
            var newMessage = context.MessageHistory
                .Where(mh => mh.MessageTypeId == MessageTypes.Product)
                .OrderByDescending(mh => mh.MessageHistoryId)
                .Take(1)
                .Single();

            Assert.AreEqual(MessageStatusTypes.Sent, newMessage.MessageStatusId);
            Assert.IsNull(newMessage.InProcessBy);
            Assert.AreEqual(DateTime.Now.Date, newMessage.ProcessedDate.Value.Date);
        }

        private List<Item> CreateFakeItems()
        {
            return new List<Item>
            {
                new Item
                {
                    itemID = 1,
                    ItemType = new ItemType
                    {
                        itemTypeCode = ItemTypeCodes.Deposit,
                        itemTypeDesc = "Deposit",
                        itemTypeID = 2
                    },
                    ScanCode = new ScanCode[]
                    {
                        new ScanCode
                        {
                            scanCode = "1112223334441",
                            scanCodeID = 1,
                            scanCodeTypeID = 1,
                            itemID = 1,
                            ScanCodeType = new ScanCodeType
                            {
                                scanCodeTypeDesc = "PosPlu",
                                scanCodeTypeID = ScanCodeTypes.PosPlu
                            }
                        }
                    },
                    ItemTrait = new ItemTrait[]
                    {
                        CreateItemTrait(Traits.ProductDescription, "ProductDescription1"),
                        CreateItemTrait(Traits.PosDescription, "PosDescription1"),
                        CreateItemTrait(Traits.PackageUnit, "PackageUnit1"),
                        CreateItemTrait(Traits.FoodStampEligible, "FoodStampEligible1")
                    },
                    ItemHierarchyClass = new ItemHierarchyClass[]
                    {
                        CreateHierarchyClass(Hierarchies.Merchandise, 1, "MerchHierarchy"),
                        CreateHierarchyClass(Hierarchies.Brands, 1, "BrandHierarchy1"),
                        CreateHierarchyClass(Hierarchies.Tax, 1, "TaxHierarchy1")
                    }
                },
                new Item
                {
                    itemID = 2,
                    ItemType = new ItemType
                    {
                        itemTypeCode = ItemTypeCodes.Deposit,
                        itemTypeDesc = "Deposit",
                        itemTypeID = 2
                    },
                    ScanCode = new ScanCode[]
                    {
                        new ScanCode
                        {
                            scanCode = "1112223334442",
                            scanCodeID = 2,
                            scanCodeTypeID = 1,
                            itemID = 2,
                            ScanCodeType = new ScanCodeType
                            {
                                scanCodeTypeDesc = "PosPlu",
                                scanCodeTypeID = ScanCodeTypes.PosPlu
                            }
                        }
                    },
                    ItemTrait = new ItemTrait[]
                    {
                        CreateItemTrait(Traits.ProductDescription, "ProductDescription2"),
                        CreateItemTrait(Traits.PosDescription, "PosDescription2"),
                        CreateItemTrait(Traits.PackageUnit, "PackageUnit2"),
                        CreateItemTrait(Traits.FoodStampEligible, "FoodStampEligible2")
                    },
                    ItemHierarchyClass = new ItemHierarchyClass[]
                    {
                        CreateHierarchyClass(Hierarchies.Merchandise, 1, "MerchHierarchy"),
                        CreateHierarchyClass(Hierarchies.Brands, 2, "BrandHierarchy2"),
                        CreateHierarchyClass(Hierarchies.Tax, 2, "TaxHierarchy2")
                    }
                },
                new Item
                {
                    itemID = 3,
                    ItemType = new ItemType
                    {
                        itemTypeCode = ItemTypeCodes.Deposit,
                        itemTypeDesc = "Deposit",
                        itemTypeID = 2
                    },
                    ScanCode = new ScanCode[]
                    {
                        new ScanCode
                        {
                            scanCode = "1112223334443",
                            scanCodeID = 3,
                            scanCodeTypeID = 1,
                            itemID = 2,
                            ScanCodeType = new ScanCodeType
                            {
                                scanCodeTypeDesc = "PosPlu",
                                scanCodeTypeID = ScanCodeTypes.PosPlu
                            }
                        }
                    },
                    ItemTrait = new ItemTrait[]
                    {
                        CreateItemTrait(Traits.ProductDescription, "ProductDescription3"),
                        CreateItemTrait(Traits.PosDescription, "PosDescription3"),
                        CreateItemTrait(Traits.PackageUnit, "PackageUnit3"),
                        CreateItemTrait(Traits.RetailSize, null),
                        CreateItemTrait(Traits.RetailUom, null),
                        CreateItemTrait(Traits.FoodStampEligible, "FoodStampEligible3")
                    },
                    ItemHierarchyClass = new ItemHierarchyClass[]
                    {
                        CreateHierarchyClass(Hierarchies.Merchandise, 1, "MerchHierarchy"),
                        CreateHierarchyClass(Hierarchies.Brands, 3, "BrandHierarchy3"),
                        CreateHierarchyClass(Hierarchies.Tax, 3, "TaxHierarchy3")
                    }
                }
            };
        }

        private ItemHierarchyClass CreateHierarchyClass(int hierarchyId, int hierarchyClassId, string hierarchyClassName)
        {
            return new ItemHierarchyClass
            {
                HierarchyClass = new HierarchyClass
                {
                    hierarchyID = hierarchyId,
                    hierarchyClassName = hierarchyClassName,
                    hierarchyClassID = hierarchyClassId,
                    Hierarchy = context.Hierarchy.Single(h => h.hierarchyID == hierarchyId)
                }
            };
        }

        private ItemTrait CreateItemTrait(int traitId, string traitValue)
        {
            return new ItemTrait
            {
                traitID = traitId,
                traitValue = traitValue,
                Trait = new Trait
                {
                    traitCode = context.Trait.Single(t => t.traitID == traitId).traitCode
                }
            };
        }
    }
}
