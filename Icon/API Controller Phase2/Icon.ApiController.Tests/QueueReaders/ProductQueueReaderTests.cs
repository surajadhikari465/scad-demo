using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.DataAccess.Commands;

using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Tests.QueueReaderTests
{
    [TestClass]
    public class ProductQueueReaderTests
    {
        private ProductQueueReader queueReader;

        private Mock<ILogger<ProductQueueReader>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>>> mockGetMessageQueueQuery;
        private Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>>> mockUpdateMessageQueueStatusCommandHandler;
        private Mock<IProductSelectionGroupsMapper> mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();
        private Mock<IUomMapper> mockUomMapper = new Mock<IUomMapper>();

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ProductQueueReader>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockGetMessageQueueQuery = new Mock<IQueryHandler<GetMessageQueueParameters<MessageQueueProduct>, List<MessageQueueProduct>>>();
            mockUpdateMessageQueueStatusCommandHandler = new Mock<ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>>>();
            mockProductSelectionGroupsMapper = new Mock<IProductSelectionGroupsMapper>();

            queueReader = new ProductQueueReader(
                mockLogger.Object,
                mockEmailClient.Object,
                mockGetMessageQueueQuery.Object,
                mockUpdateMessageQueueStatusCommandHandler.Object,
                mockProductSelectionGroupsMapper.Object,
                mockUomMapper.Object);
        }

        [TestMethod]
        public void GroupProductMessages_InvalidArgument_ShouldThrowException()
        {
            // Given.
            int exceptionCount = 0;

            // When.
            var messages = new List<MessageQueueProduct>();

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            messages = null;

            try { queueReader.GroupMessagesForMiniBulk(messages); }
            catch (Exception) { exceptionCount++; }

            // Then.
            Assert.AreEqual(2, exceptionCount);
        }

        [TestMethod]
        public void GroupProductMessages_OneMessage_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
        }

        [TestMethod]
        public void GroupProductMessages_TwoMessagesWithSameItemId_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
        }

        [TestMethod]
        public void GroupProductMessages_DistinctFirstItemIdWithDuplicateSecondAndThirdItemId_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(2, messages.Count);
        }

        [TestMethod]
        public void GroupProductMessages_TwoMessagesWithDifferentDepartmentSale_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
        }

        [TestMethod]
        public void GroupProductMessages_TwoMessagesWithDifferentItemIdAndSameDepartmentSale_ShouldReturnTwoMessagesForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(2, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
            Assert.AreEqual(2, messages[1].ItemId);
        }

        [TestMethod]
        public void GetProductMiniBulk_InvalidArguments_ExceptionShouldBeThrown()
        {
            // Given.
            var messages = new List<MessageQueueProduct>();

            int caughtExceptions = 0;

            // When.
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            messages = null;
            try { var miniBulk = queueReader.BuildMiniBulk(messages); }
            catch (ArgumentException) { caughtExceptions++; }

            // Then.
            int expectedExceptions = 2;
            Assert.AreEqual(expectedExceptions, caughtExceptions);
        }

        [TestMethod]
        public void GetProductMiniBulk_ThreeProductMessages_ShouldReturnMiniBulkWithThreeItemEntries()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "0", ItemTypeCodes.RetailSale)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(3, miniBulk.item.Length);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessage_StandardItemTraitsShouldBePresent()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;

            string productDescription = itemTraits.Single(it => it.code == TraitCodes.ProductDescription).type.value[0].value;
            string posDescription = itemTraits.Single(it => it.code == TraitCodes.PosDescription).type.value[0].value;

            Assert.AreEqual(fakeMessage.ProductDescription, productDescription);
            Assert.AreEqual(fakeMessage.PosDescription, posDescription);
        }

        [TestMethod]
        public void GetProductMiniBulk_PackageUnitExistsButNotRetailSizeOrUom_NoneShouldBePresentInTheMiniBulk()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.PackageUnit = "1";
            fakeMessage.RetailSize = null;
            fakeMessage.RetailUom = null;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;

            var packageUnit = itemTraits.SingleOrDefault(it => it.code == TraitCodes.PackageUnit);
            var retailSize = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailSize);
            var retailUom = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailUom);

            Assert.IsNull(packageUnit);
            Assert.IsNull(retailSize);
            Assert.IsNull(retailUom);
        }

        [TestMethod]
        public void GetProductMiniBulk_PackageUnitAndRetailSizeExistButNotRetailUom_NoneShouldBePresentInTheMiniBulk()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.PackageUnit = "1";
            fakeMessage.RetailSize = 1m.ToString();
            fakeMessage.RetailUom = null;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;

            var packageUnit = itemTraits.SingleOrDefault(it => it.code == TraitCodes.PackageUnit);
            var retailSize = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailSize);
            var retailUom = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailUom);

            Assert.IsNull(packageUnit);
            Assert.IsNull(retailSize);
            Assert.IsNull(retailUom);
        }

        [TestMethod]
        public void GetProductMiniBulk_PackageUnitAndRetailUomExistButNotRetailSize_NoneShouldBePresentInTheMiniBulk()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.PackageUnit = "1";
            fakeMessage.RetailSize = null;
            fakeMessage.RetailUom = "EA";

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;

            var packageUnit = itemTraits.SingleOrDefault(it => it.code == TraitCodes.PackageUnit);
            var retailSize = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailSize);
            var retailUom = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailUom);

            Assert.IsNull(packageUnit);
            Assert.IsNull(retailSize);
            Assert.IsNull(retailUom);
        }

        [TestMethod]
        public void GetProductMiniBulk_RetailUomAndRetailSizeExistButNotPackageUnit_NoneShouldBePresentInTheMiniBulk()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.PackageUnit = null;
            fakeMessage.RetailSize = 1m.ToString();
            fakeMessage.RetailUom = "EA";

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;

            var packageUnit = itemTraits.SingleOrDefault(it => it.code == TraitCodes.PackageUnit);
            var retailSize = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailSize);
            var retailUom = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailUom);

            Assert.IsNull(packageUnit);
            Assert.IsNull(retailSize);
            Assert.IsNull(retailUom);
        }

        [TestMethod]
        public void GetProductMiniBulk_PackageUnitAndRetailUomAndRetailSizeExist_AllThreeShouldBePresentInTheMiniBulk()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.PackageUnit = "1";
            fakeMessage.RetailSize = 1m.ToString();
            fakeMessage.RetailUom = "EA";

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;

            var packageUnit = itemTraits.SingleOrDefault(it => it.code == TraitCodes.PackageUnit);
            var retailSize = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailSize);
            var retailUom = itemTraits.SingleOrDefault(it => it.code == TraitCodes.RetailUom);

            Assert.IsNotNull(packageUnit);
            Assert.IsNotNull(retailSize);
            Assert.IsNotNull(retailUom);

            Assert.AreEqual(fakeMessage.PackageUnit, packageUnit.type.value[0].value);
            Assert.AreEqual(fakeMessage.RetailSize.ToString(), retailSize.type.value[0].value);
            Assert.AreEqual(fakeMessage.RetailUom, retailUom.type.value[0].value);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessage_ShouldNotContainPosScaleTareElement()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var scaleTare = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.SingleOrDefault(it => it.code == TraitCodes.PosScaleTare);
            Assert.IsNull(scaleTare);
        }

        [TestMethod]
        public void GetProductMiniBulk_ThreeDepartmentSaleMessages_ShouldReturnMiniBulkWithThreeItemEntries()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "1", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "1", ItemTypeCodes.RetailSale)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);
            bool eachEntryHasItemTraits = Array.TrueForAll(miniBulk.item, i => (i.locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.Length == 1);

            // Then.
            Assert.AreEqual(3, miniBulk.item.Length);
            Assert.IsTrue(eachEntryHasItemTraits);
        }

        [TestMethod]
        public void GetProductMiniBulk_DepartmentSaleMiniBulk_MessageShouldOnlyContainDepartmentSaleItemTrait()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.RetailSale);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemTraits = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits;
            var departmentSale = itemTraits.Single(it => it.code == TraitCodes.DepartmentSale);

            Assert.AreEqual(1, itemTraits.Count());
            Assert.AreEqual(fakeMessage.FinancialClassId, departmentSale.type.value[0].value);
        }

        [TestMethod]
        public void GetProductMiniBulk_DepartmentSaleMiniBulk_MessageShouldContainTaxHierarchy()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.RetailSale);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var itemHierarchies = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).hierarchies;
            var taxHierarchy = itemHierarchies.Single(ih => ih.name == HierarchyNames.Tax);

            Assert.AreEqual(2, itemHierarchies.Count());
            Assert.AreEqual(fakeMessage.TaxClassName.Split(' ')[0], taxHierarchy.@class[0].id);
        }

        [TestMethod]
        public void GetProductMiniBulk_DepartmentSaleMiniBulk_MiniBulkShouldNotContainGroupsElement()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.RetailSale);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var groups = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).groups;

            Assert.IsNull(groups);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessageTaxClassId_ShouldBeTheSevenDigitTaxCode()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var taxClass = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).hierarchies.Single(ih => ih.name == HierarchyNames.Tax).@class[0].id;
            Assert.AreEqual(fakeMessage.TaxClassName.Split(' ')[0], taxClass);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessageFinancialHierarchyInformation_ShouldBeSetCorrectlyForNullSubteam()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.FinancialClassId = "0000";
            fakeMessage.FinancialClassName = "na";

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var financial = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).hierarchies.Single(ih => ih.name == HierarchyNames.Financial);

            Assert.AreEqual("0000", financial.@class[0].id);
            Assert.AreEqual("na", financial.@class[0].name);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessage_ShouldHaveSelectionGroups()
        {
            // Given.
            mockProductSelectionGroupsMapper.Setup(m => m.GetProductSelectionGroups(It.IsAny<MessageQueueProduct>()))
                .Returns(new Contracts.SelectionGroupsType
                {
                    group = new Contracts.GroupTypeType[]
                    {
                        new Contracts.GroupTypeType
                        {
                            id = "Test",
                            name = "Test",
                            type = "Test"
                        }
                    }
                });

            var message = new TestProductMessageBuilder().Build();

            // When.
            var miniBulk = queueReader.BuildMiniBulk(new List<MessageQueueProduct> { message });
            var attributes = miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType;

            // Then.
            Assert.AreEqual(1, attributes.selectionGroups.group.Length);
            Assert.IsNotNull(attributes.selectionGroups.group.SingleOrDefault(sg => sg.id == "Test"));
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessageWithRetailUomTrait_TraitValueShouldBeSentAsEnum()
        {
            // Given.
            mockUomMapper.Setup(m => m.GetEsbUomCode(It.IsAny<string>())).Returns(Contracts.WfmUomCodeEnumType.EXP);
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var retailUomTrait = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.Single(t => t.code == TraitCodes.RetailUom);

            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EXP, retailUomTrait.type.value[0].uom.code);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessageWithUnknownRetailUomTrait_RetailUomShouldBeEach()
        {
            // Given.
            mockUomMapper.Setup(m => m.GetEsbUomCode(It.IsAny<string>())).Returns(Contracts.WfmUomCodeEnumType.EA);
            mockUomMapper.Setup(m => m.GetEsbUomDescription(It.IsAny<string>())).Returns(Contracts.WfmUomDescEnumType.EACH);

            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);
            fakeMessage.RetailUom = "ZZZ";

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var retailUomTrait = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.Single(t => t.code == TraitCodes.RetailUom);
            var uomCode = retailUomTrait.type.value[0].uom.code;
            var uomName = retailUomTrait.type.value[0].uom.name;

            Assert.AreEqual(Contracts.WfmUomCodeEnumType.EA, uomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.EACH, uomName);
        }

        [TestMethod]
        public void GetProductMiniBulk_ProductMessageWithRetailUomTraitContainingSpaces_RetailUomShouldBeMatchedToEnum()
        {
            // Given.
            mockUomMapper.Setup(m => m.GetEsbUomCode(It.IsAny<string>())).Returns(Contracts.WfmUomCodeEnumType.CT);
            mockUomMapper.Setup(m => m.GetEsbUomDescription(It.IsAny<string>())).Returns(Contracts.WfmUomDescEnumType.COUNT);
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.RetailSale);

            fakeMessage.RetailUom = "CT";

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var retailUomTrait = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.Single(t => t.code == TraitCodes.RetailUom);
            var uomCode = retailUomTrait.type.value[0].uom.code;
            var uomName = retailUomTrait.type.value[0].uom.name;

            Assert.AreEqual(Contracts.WfmUomCodeEnumType.CT, uomCode);
            Assert.AreEqual(Contracts.WfmUomDescEnumType.COUNT, uomName);
        }


        [TestMethod]
        public void GetProductMiniBulk_ThreeNonRetailProductMessages_ShouldReturnMiniBulkWithThreeItemEntries()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "0", ItemTypeCodes.NonRetail)
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(3, miniBulk.item.Length);
        }

        [TestMethod]
        public void GroupProductMessages_ThreeNonRetailMessages_ShouldReturn3MessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "0", ItemTypeCodes.NonRetail)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(3, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
        }

        [TestMethod]
        public void GroupProductMessages_ThreeRatailNonRetailMessagesOneDeptSale_ShouldReturnOneMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.RetailSale),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "0", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "1", ItemTypeCodes.NonRetail)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(1, messages.Count);
            Assert.AreEqual(1, messages[0].ItemId);
        }

        [TestMethod]
        public void GroupProductMessages_ThreeNonRetailMessagesThreeDeptSale_ShouldReturnThreeMessageForTheMiniBulk()
        {
            // Given.
            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 2, "1", ItemTypeCodes.NonRetail),
                TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 3, "1", ItemTypeCodes.NonRetail)
            };

            // When.
            var messages = queueReader.GroupMessagesForMiniBulk(fakeMessageQueueProducts);

            // Then.
            Assert.AreEqual(3, messages.Count);
        }

        [TestMethod]
        public void GetProductMiniBulk_NonRetailDepartmentSaleMiniBulk_MiniBulkShouldNotContainGroupsElement()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "1", ItemTypeCodes.NonRetail);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var groups = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).groups;

            Assert.IsNull(groups);
        }

        [TestMethod]
        public void GetProductMiniBulk_NonRetailWithOutNutritionMiniBulk_MiniBulkShouldNotContainConsumerInformation()
        {
            // Given.
            var fakeMessage = TestHelpers.GetFakeMessageQueueProduct(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.NonRetail);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var groups = (miniBulk.item[0].@base.consumerInformation as Contracts.ConsumerInformationType);

            Assert.IsNull(groups);
        }

        [TestMethod]
        public void GetProductMiniBulk_NonRetailWithNutritionMiniBulk_MiniBulkShouldContainConsumerInformation()
        {
            // Given.
            mockUomMapper.Setup(m => m.GetEsbUomCode(It.IsAny<string>())).Returns(Contracts.WfmUomCodeEnumType.EA);
            var fakeMessage = TestHelpers.GetFakeMessageQueueProductWithNutritionalData(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.NonRetail);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var groups = (miniBulk.item[0].@base.consumerInformation as Contracts.ConsumerInformationType);
            var hshTrait = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.Single(t => t.code == TraitCodes.Hsh);
            var servingSizeUom = groups.stockItemConsumerProductLabel.servingSizeUom;

            Assert.IsNotNull(groups);
            Assert.AreEqual("5", hshTrait.type.value[0].value);
        }

        [TestMethod]
        public void GetProductMiniBulk_NonRetailWithNutritionMiniBulk_ConsumerInformationContainsBlankHazardousMaterialTypeCode()
        {
            // Given.
            mockUomMapper.Setup(m => m.GetEsbUomCode(It.IsAny<string>())).Returns(Contracts.WfmUomCodeEnumType.EA);
            var fakeMessage = TestHelpers.GetFakeMessageQueueProductWithNutritionalData(MessageStatusTypes.Ready, 1, "0", ItemTypeCodes.NonRetail);
            fakeMessage.ProhibitDiscount = false;

            var fakeMessageQueueProducts = new List<MessageQueueProduct>
            {
                fakeMessage
            };

            // When.
            var miniBulk = queueReader.BuildMiniBulk(fakeMessageQueueProducts);

            // Then.
            var groups = (miniBulk.item[0].@base.consumerInformation as Contracts.ConsumerInformationType);
            var hshTrait = (miniBulk.item[0].locale[0].Item as Contracts.EnterpriseItemAttributesType).traits.Single(t => t.code == TraitCodes.Hsh);
            var hazardousMaterialTypeCode = groups.stockItemConsumerProductLabel.hazardousMaterialTypeCode;

            Assert.IsNotNull(groups);
            Assert.AreEqual("5", hshTrait.type.value[0].value);
            Assert.IsNull(hazardousMaterialTypeCode);
        }
    }
}
