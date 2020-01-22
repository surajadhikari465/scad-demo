using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using Icon.Services.ItemPublisher.Services;
using Icon.Services.Newitem.Test.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb.Tests
{
    [TestClass()]
    public class EsbMessageBuilderTests
    {
        private TestDataFactory testDataFactory = new TestDataFactory();

        [TestMethod]
        public async Task BuildItem_NotDepartmentSale_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
             new Attributes()
             {
                 AttributeId = 1,
                 AttributeName = "ItemId",
                 Description = "ItemId",
                 TraitCode = "ItemId"
             }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = "test"
             }));

            var productSelectionGroupMock = new ConcurrentDictionary<int, ProductSelectionGroup>() { };
            productSelectionGroupMock[1] = new ProductSelectionGroup();
            productSelectionGroupMock[2] = new ProductSelectionGroup();
            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroupMock);

            // When.
            BuildMessageResult result = await builder.BuildItem(new List<MessageQueueItemModel>() { this.testDataFactory.MessageQueueItemModel }, false);

            // Then.
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Errors.Count == 0);
            Assert.IsNotNull(result.Contract.item.FirstOrDefault());
            Assert.AreEqual(ActionEnum.AddOrUpdate, result.Contract.item.FirstOrDefault().Action);
            Assert.IsTrue(result.Contract.item.FirstOrDefault().ActionSpecified);
            Assert.AreEqual(999999999, result.Contract.item.FirstOrDefault().id);
            Assert.IsFalse(result.Contract.item.FirstOrDefault().isAvailable);
            Assert.IsFalse(result.Contract.item.FirstOrDefault().isAvailableSpecified);
            Assert.IsTrue(result.Contract.item.FirstOrDefault().locale.Length > 0);
        }

        [TestMethod]
        public async Task BuildItem_IsDepartmentSale_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyClassIdForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns("999999998");
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());
            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
              new Attributes()
              {
                  AttributeId = 2,
                  AttributeName = ItemPublisherConstants.Attributes.DepartmentSale,
                  Description = "DepartmentSaleAttribute",
                  TraitCode = "DPT"
              }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = Framework.HierarchyNames.Financial
             }));

            var productSelectionGroupMock = new ConcurrentDictionary<int, ProductSelectionGroup>() { };
            productSelectionGroupMock[1] = new ProductSelectionGroup();
            productSelectionGroupMock[2] = new ProductSelectionGroup();
            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroupMock);

            // When.
            BuildMessageResult result = await builder.BuildItem(new List<MessageQueueItemModel>() { this.testDataFactory.MessageQueueItemModel }, true);

            // Then.
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Errors.Count == 0);
            Assert.IsNotNull(result.Contract.item.FirstOrDefault());
            Assert.AreEqual(ActionEnum.AddOrUpdate, result.Contract.item.FirstOrDefault().Action);
            Assert.IsTrue(result.Contract.item.FirstOrDefault().ActionSpecified);
            Assert.AreEqual(999999999, result.Contract.item.FirstOrDefault().id);
            Assert.IsFalse(result.Contract.item.FirstOrDefault().isAvailable);
            Assert.IsFalse(result.Contract.item.FirstOrDefault().isAvailableSpecified);
            Assert.IsTrue(result.Contract.item.FirstOrDefault().locale.Length > 0);
            Assert.AreEqual(Framework.TraitCodes.DepartmentSale, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).traits[0].code);
            Assert.AreEqual(Framework.TraitDescriptions.DepartmentSale, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).traits[0].type.description);
            Assert.AreEqual(2, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).hierarchies.Length);
            Assert.AreEqual(Framework.HierarchyNames.Tax, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).hierarchies[0].name);
            Assert.AreEqual(Framework.HierarchyNames.Merchandise, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).hierarchies[1].name);
            Assert.AreEqual(1, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).traits.Length);
            Assert.AreEqual(Framework.TraitCodes.DepartmentSale, (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).traits[0].code);
            Assert.AreEqual("999999998", (result.Contract.item[0].locale[0].Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType).traits[0].type.value[0].value);
        }

        [TestMethod]
        public async Task BuildItem_TraitMappingMissing_FailureResultShouldBeReturned()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(null));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
            new HierarchyCacheItem()
            {
                HierarchyId = 1,
                HierarchyName = "test"
            }));

            var productSelectionGroupMock = new ConcurrentDictionary<int, ProductSelectionGroup>() { };
            productSelectionGroupMock[1] = new ProductSelectionGroup();
            productSelectionGroupMock[2] = new ProductSelectionGroup();
            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroupMock);

            // When.
            BuildMessageResult result = await builder.BuildItem(new List<MessageQueueItemModel>() { this.testDataFactory.MessageQueueItemModel }, false);

            // Then.
            Assert.IsFalse(result.Success, "TraitType was null so there should have been an error thrown");
            Assert.IsTrue(result.Errors.Count > 0);
        }

        [TestMethod]
        public async Task BuildItem_TraitCodeNull_FailureResultShouldBeReturned()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
            new Attributes()
            {
                AttributeId = 1,
                AttributeName = "ItemId",
                Description = "ItemId",
                TraitCode = null
            }));
            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
            new HierarchyCacheItem()
            {
                HierarchyId = 1,
                HierarchyName = "test"
            }));

            var productSelectionGroupMock = new ConcurrentDictionary<int, ProductSelectionGroup>() { };
            productSelectionGroupMock[1] = new ProductSelectionGroup();
            productSelectionGroupMock[2] = new ProductSelectionGroup();
            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroupMock);

            // When.
            BuildMessageResult result = await builder.BuildItem(new List<MessageQueueItemModel>() { this.testDataFactory.MessageQueueItemModel }, false);

            // Then.
            Assert.IsFalse(result.Success, "We did not have a mapping for the ItemId trait so this should cause an error");
            Assert.IsTrue(result.Errors.Count > 0);
        }

        [TestMethod]
        public async Task BuildItemType_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();

            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
                new Attributes()
                {
                    AttributeId = 1,
                    AttributeName = "AttributeName",
                    Description = "Description",
                    TraitCode = "TraitCode"
                }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                new HierarchyCacheItem()
                {
                    HierarchyId = 1,
                    HierarchyName = "test"
                }));

            var productSelectionGroupMock = new ConcurrentDictionary<int, ProductSelectionGroup>() { };
            productSelectionGroupMock[1] = new ProductSelectionGroup();
            productSelectionGroupMock[2] = new ProductSelectionGroup();
            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroupMock);

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            ItemType result = await builder.BuildItemType(this.testDataFactory.MessageQueueItemModel, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, result.Action);
            Assert.IsTrue(result.ActionSpecified);
            Assert.IsTrue(result.id == 999999999);
            Assert.IsFalse(result.isAvailable);
            Assert.IsFalse(result.isAvailableSpecified);
            Assert.IsTrue(result.locale.Length > 0);

            Assert.IsTrue(result.@base.type.code == ItemPublisherConstants.RetailSaleTypeCode);
            Assert.IsTrue(result.@base.type.description == ItemPublisherConstants.RetailSaleTypeCodeDescription);

            var firstLocale = result.locale.First();
            Assert.AreEqual(ActionEnum.Add, firstLocale.Action);
            Assert.IsFalse(firstLocale.ActionSpecified);
            Assert.IsNull(firstLocale.addresses);
            Assert.AreEqual(DateTime.MinValue, firstLocale.closeDate);
            Assert.IsFalse(firstLocale.closeDateSpecified);
            Assert.IsNull(firstLocale.financialTransactionAccounts);
            Assert.AreEqual("1", firstLocale.id);
            Assert.IsNotNull(firstLocale.Item);
            Assert.AreEqual("Whole Foods Market", firstLocale.name);
            Assert.AreEqual(DateTime.MinValue, firstLocale.openDate);
            Assert.IsFalse(firstLocale.openDateSpecified);
            Assert.AreEqual(0, firstLocale.parentId);
            Assert.IsFalse(firstLocale.parentIdSpecified);
            Assert.IsNull(firstLocale.store);
            Assert.IsNull(firstLocale.traits);
            Assert.AreEqual(LocaleCodeType.CHN, firstLocale.type.code);
            Assert.AreEqual(LocaleDescType.Chain, firstLocale.type.description);

            var item = firstLocale.Item as Icon.Esb.Schemas.Wfm.Contracts.EnterpriseItemAttributesType;
            Assert.IsTrue(item.scanCodes.Length > 0);
            Assert.IsTrue(item.hierarchies.Length > 0);
            Assert.IsTrue(item.traits.Length > 0);
            Assert.IsTrue(item.isKitchenItem);
            Assert.IsTrue(item.isKitchenItemSpecified);
            Assert.IsTrue(item.isHospitalityItem);
            Assert.IsTrue(item.isHospitalityItemSpecified);
            Assert.IsNotNull(item.selectionGroups);
        }

        [TestMethod]
        public async Task BuildHierarchies_MerchandiseHierarchy_ReturnIsInCorrectFormat()
        {
            // Given.
            int expectedHierarchyClassId = 51214;
            string expectedHierarchyClassName = "Unit Test Merchandise SubBrick HierarchyClassName: Beer (3100)";
            int expectedHierarchyId = Icon.Framework.Hierarchies.Merchandise;
            string expectedHierarchyName = Icon.Framework.HierarchyNames.Merchandise;
            int expectedParentId = 54132654;
            string expectedParentName = "Unit Test Merch Parent Name";
            int expectedLevel = 5;

            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyClassIdForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassId.ToString());
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyNameForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassName);
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            this.testDataFactory.Hierarchy = new List<Hierarchy>
            {
                new Hierarchy()
                {
                    HierarchyClassId = expectedHierarchyClassId,
                    HierarchyId = expectedHierarchyId,
                    HierarchyName = expectedHierarchyName,
                    HierarchyClassName = expectedHierarchyClassName,
                    HierarchyLevel = expectedLevel,
                    ItemId = 493,
                    HierarchyClassParentId = expectedParentId,
                    HierarchyClassParentName = expectedParentName
                }
            };

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
             new Attributes()
             {
                 AttributeId = 1,
                 AttributeName = "AttributeName",
                 Description = "Description",
                 TraitCode = "TraitCode"
             }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                new HierarchyCacheItem()
                {
                    HierarchyId = 1,
                    HierarchyName = "test"
                }));

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            List<HierarchyType> result = await builder.BuildHierarchies(this.testDataFactory.Hierarchy, processLogger);

            // Then.
            Assert.IsTrue(result.Count == 1);

            Assert.AreEqual(ActionEnum.Add, result.First().Action);
            Assert.IsFalse(result.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyId, result.First().id);
            Assert.IsFalse(result.First().idSpecified);
            Assert.AreEqual(expectedHierarchyName, result.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().Action);
            Assert.IsFalse(result.First().@class.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyClassId.ToString(), result.First().@class.First().id);
            Assert.AreEqual(expectedLevel, result.First().@class.First().level);
            Assert.IsFalse(result.First().@class.First().levelSpecified);
            Assert.AreEqual(expectedHierarchyClassName, result.First().@class.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().parentId.Action);
            Assert.IsFalse(result.First().@class.First().parentId.ActionSpecified);
            Assert.AreEqual(expectedParentId, result.First().@class.First().parentId.Value);
            Assert.IsNull(result.First().@class.First().traits);
        }

        [TestMethod]
        public async Task BuildHierarchies_BrandHierarchy_ReturnIsInCorrectFormat()
        {
            // Given.
            int expectedHierarchyClassId = 51214;
            string expectedHierarchyClassName = "UnitTest BrandName";
            int expectedHierarchyId = Icon.Framework.Hierarchies.Brands;
            string expectedHierarchyName = Icon.Framework.HierarchyNames.Brands;
            int expectedLevel = 1;

            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyNameForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassName);
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyClassIdForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassId.ToString());
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            this.testDataFactory.Hierarchy = new List<Hierarchy>
            {
                new Hierarchy()
                {
                    HierarchyClassId = expectedHierarchyClassId,
                    HierarchyId = expectedHierarchyId,
                    HierarchyName = expectedHierarchyName,
                    HierarchyClassName = expectedHierarchyClassName,
                    HierarchyLevel = expectedLevel,
                    ItemId = 493,
                    HierarchyClassParentId = null,
                    HierarchyClassParentName = null
                }
            };

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
             new Attributes()
             {
                 AttributeId = 1,
                 AttributeName = "AttributeName",
                 Description = "Description",
                 TraitCode = "TraitCode"
             }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                new HierarchyCacheItem()
                {
                    HierarchyId = 1,
                    HierarchyName = "test"
                }));

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            List<HierarchyType> result = await builder.BuildHierarchies(this.testDataFactory.Hierarchy, processLogger);

            // Then.
            Assert.IsTrue(result.Count == 1);

            Assert.AreEqual(ActionEnum.Add, result.First().Action);
            Assert.IsFalse(result.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyId, result.First().id);
            Assert.IsFalse(result.First().idSpecified);
            Assert.AreEqual(expectedHierarchyName, result.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().Action);
            Assert.IsFalse(result.First().@class.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyClassId.ToString(), result.First().@class.First().id);
            Assert.AreEqual(expectedLevel, result.First().@class.First().level);
            Assert.IsFalse(result.First().@class.First().levelSpecified);
            Assert.AreEqual(expectedHierarchyClassName, result.First().@class.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().parentId.Action);
            Assert.IsFalse(result.First().@class.First().parentId.ActionSpecified);
            Assert.AreEqual(0, result.First().@class.First().parentId.Value);
            Assert.IsNull(result.First().@class.First().traits);
        }

        [TestMethod]
        public async Task BuildHierarchies_NationalHierarchy_ReturnIsInCorrectFormat()
        {
            // Given.
            int expectedHierarchyClassId = 51214;
            string expectedHierarchyClassName = "UnitTest National Class";
            int expectedHierarchyId = Icon.Framework.Hierarchies.National;
            string expectedHierarchyName = Icon.Framework.HierarchyNames.National;
            int expectedParentId = 456232;
            string expectedParentName = "Test National Parent Name";
            int expectedLevel = 4;

            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyClassIdForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassId.ToString());
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyNameForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassName);
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());
            this.testDataFactory.Hierarchy = new List<Hierarchy>
            {
                new Hierarchy()
                {
                    HierarchyClassId = expectedHierarchyClassId,
                    HierarchyId = expectedHierarchyId,
                    HierarchyName = expectedHierarchyName,
                    HierarchyClassName = expectedHierarchyClassName,
                    HierarchyLevel = expectedLevel,
                    ItemId = 493,
                    HierarchyClassParentId = expectedParentId,
                    HierarchyClassParentName = expectedParentName
                }
            };

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
             new Attributes()
             {
                 AttributeId = 1,
                 AttributeName = "AttributeName",
                 Description = "Description",
                 TraitCode = "TraitCode"
             }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                new HierarchyCacheItem()
                {
                    HierarchyId = 1,
                    HierarchyName = "test"
                }));

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            List<HierarchyType> result = await builder.BuildHierarchies(this.testDataFactory.Hierarchy, processLogger);

            // Then.
            Assert.IsTrue(result.Count == 1);

            Assert.AreEqual(ActionEnum.Add, result.First().Action);
            Assert.IsFalse(result.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyId, result.First().id);
            Assert.IsFalse(result.First().idSpecified);
            Assert.AreEqual(expectedHierarchyName, result.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().Action);
            Assert.IsFalse(result.First().@class.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyClassId.ToString(), result.First().@class.First().id);
            Assert.AreEqual(expectedLevel, result.First().@class.First().level);
            Assert.IsFalse(result.First().@class.First().levelSpecified);
            Assert.AreEqual(expectedHierarchyClassName, result.First().@class.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().parentId.Action);
            Assert.IsFalse(result.First().@class.First().parentId.ActionSpecified);
            Assert.AreEqual(expectedParentId, result.First().@class.First().parentId.Value);
            Assert.IsNull(result.First().@class.First().traits);
        }

        [TestMethod]
        public async Task BuildHierarchies_IncludeManufacturerHierarchyIsFalse_ShouldNotBuildManufacturerHierarchy()
        {
            // Given.
            int expectedHierarchyClassId = 51214;
            string expectedHierarchyClassName = "UnitTest Manufacturer Class";
            int expectedHierarchyId = Icon.Framework.Hierarchies.Manufacturer;
            string expectedHierarchyName = Icon.Framework.HierarchyNames.Manufacturer;
            int? expectedParentId = null;
            int expectedLevel = 1;

            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyClassIdForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassId.ToString());
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyNameForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassName);
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());
            this.testDataFactory.Hierarchy = new List<Hierarchy>
            {
                new Hierarchy()
                {
                    HierarchyClassId = expectedHierarchyClassId,
                    HierarchyId = expectedHierarchyId,
                    HierarchyName = expectedHierarchyName,
                    HierarchyClassName = expectedHierarchyClassName,
                    HierarchyLevel = expectedLevel,
                    ItemId = 493,
                    HierarchyClassParentId = expectedParentId,
                    HierarchyClassParentName = null
                }
            };

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
             new Attributes()
             {
                 AttributeId = 1,
                 AttributeName = "AttributeName",
                 Description = "Description",
                 TraitCode = "TraitCode"
             }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                new HierarchyCacheItem()
                {
                    HierarchyId = 1,
                    HierarchyName = "test"
                }));

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            List<HierarchyType> result = await builder.BuildHierarchies(this.testDataFactory.Hierarchy, processLogger);

            // Then.
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod]
        public async Task BuildHierarchies_TaxHierarchy_IdIsTaxCodeFromTaxClassName()
        {
            // Given.
            string expectedHierarchyClassId = "4392837";
            string expectedHierarchyClassName = "4392837 Tax Class For Test";
            int expectedHierarchyId = Icon.Framework.Hierarchies.Tax;
            string expectedHierarchyName = Icon.Framework.HierarchyNames.Tax;

            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyClassIdForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassId.ToString());
            hierarchyValuesParserMock.Setup(x => x.ParseHierarchyNameForContract(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(expectedHierarchyClassName);
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            this.testDataFactory.Hierarchy = new List<Hierarchy>
            {
                new Hierarchy()
                {
                    HierarchyClassId = 999999999,
                    HierarchyId = Icon.Framework.Hierarchies.Tax,
                    HierarchyName = Icon.Framework.HierarchyNames.Tax,
                    HierarchyClassName = expectedHierarchyClassName,
                    HierarchyLevel = 1,
                    ItemId = 493,
                    HierarchyClassParentId = null,
                    HierarchyClassParentName = null
                }
            };

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
             new Attributes()
             {
                 AttributeId = 1,
                 AttributeName = "AttributeName",
                 Description = "Description",
                 TraitCode = "TraitCode"
             }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                new HierarchyCacheItem()
                {
                    HierarchyId = 1,
                    HierarchyName = "test"
                }));

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            List<HierarchyType> result = await builder.BuildHierarchies(this.testDataFactory.Hierarchy, processLogger);

            // Then.
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(ActionEnum.Add, result.First().Action);
            Assert.IsFalse(result.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyId, result.First().id);
            Assert.IsFalse(result.First().idSpecified);
            Assert.AreEqual(expectedHierarchyName, result.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().Action);
            Assert.IsFalse(result.First().@class.First().ActionSpecified);
            Assert.AreEqual(expectedHierarchyClassId, result.First().@class.First().id);
            Assert.AreEqual(1, result.First().@class.First().level);
            Assert.IsFalse(result.First().@class.First().levelSpecified);
            Assert.AreEqual(expectedHierarchyClassName, result.First().@class.First().name);
            Assert.AreEqual(ActionEnum.Add, result.First().@class.First().parentId.Action);
            Assert.IsFalse(result.First().@class.First().parentId.ActionSpecified);
            Assert.AreEqual(0, result.First().@class.First().parentId.Value);
            Assert.IsNull(result.First().@class.First().traits);
        }

        /// <summary>
        /// Tests that a SelectionGroupsType is created and has groups
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroupRootNode_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "AttributeName",
                AttributeValue = "AttributeValue",
                MerchandiseHierarchyClassId = 2,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "AttributeName2",
                AttributeValue = "AttributeValue2",
                MerchandiseHierarchyClassId = 2,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName2",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            // When.
            SelectionGroupsType response = await builder.BuildProductSelectionGroupRootNode(this.testDataFactory.MessageQueueItemModel, processLogger);

            // Then.
            Assert.AreEqual(2, response.group.Length);
        }

        /// <summary>
        /// Tests that when we process an item the product selection groups are returned in a collection
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "AttributeName2",
                AttributeValue = "AttributeValue2",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName2",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            productSelectionGroups[3] = new ProductSelectionGroup
            {
                MerchandiseHierarchyClassId = 23456,
                ProductSelectionGroupId = 4,
                ProductSelectionGroupName = "SomeMerchSelectionGroup",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(this.testDataFactory.MessageQueueItemModel, processLogger);

            // Then.
            Assert.AreEqual(3, response.Count);
        }

        /// <summary>
        /// Tests that when we build product selection groups that if the FoodStampEligible column in Item matches tha attribute value of the PSG
        /// that the PSG message is sent with the AddOrUpdate Action
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_FoodStampEligibleMatchesPSGAttributeValue_CreateProductSelectionGroupElementShouldAddOrUpdate()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedFoodStampPsgName = ItemPublisherConstants.ProductSelectionGroups.FoodStamp;
            string expectedProhibitDiscountPsgName = "Prohibit_Discount_Name";

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedFoodStampPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "ProhibitDiscount",
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedProhibitDiscountPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
            new Item()
            {
                ItemAttributesJson = "{ 'FoodStampEligible':'true', 'ProhibitDiscount':'false', 'Inactive':'false'  }"
            },
            null,
            null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.First(x => x.id == expectedFoodStampPsgName).Action);
            Assert.AreEqual(ActionEnum.Delete, response.First(x => x.id == expectedProhibitDiscountPsgName).Action);
        }

        /// <summary>
        /// Tests that when we build product selection groups that if the FoodStampEligible column in Item does not match tha attribute value of the PSG
        /// that the PSG message is sent with the Delete Action
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_FoodStampEligibleDoesNotMatchPSGAttributeValue_CreateProductSelectionGroupElementShouldDelete()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedFoodStampPsgName = ItemPublisherConstants.ProductSelectionGroups.FoodStamp;
            string expectedProhibitDiscountPsgName = "Prohibit_Discount_Name";

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "1",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedFoodStampPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "ProhibitDiscount",
                AttributeValue = "1",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedProhibitDiscountPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
                new Item()
                {
                    ItemAttributesJson = "{ 'FoodStampEligible':0, 'ProhibitDiscount':0 , 'IsActive':1 }"
                },
                null,
                null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.Delete, response.First(x => x.id == expectedFoodStampPsgName).Action);
        }

        /// <summary>
        /// Tests that when we build product selection groups that if the ProhibitDiscount column in Item matches tha attribute value of the PSG
        /// that the PSG message is sent with the AddOrUpdate Action
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_ProhibitDiscountMatchesPSGAttributeValue_CreateProductSelectionGroupElementShouldBeAddOrUpdate()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedFoodStampPsgName = ItemPublisherConstants.ProductSelectionGroups.FoodStamp;
            string expectedProhibitDiscountPsgName = "Prohibit_Discount_Name";

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedFoodStampPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "ProhibitDiscount",
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedProhibitDiscountPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
             new Item()
             {
                 ItemAttributesJson = "{'FoodStampEligible':'false', 'ProhibitDiscount':'true', 'Inactive':'0'  }"
             },
             null,
             null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.First(x => x.id == expectedProhibitDiscountPsgName).Action);
        }

        /// <summary>
        /// Tests that when we build product selection groups that if the ProhibitDiscount column in Item matches tha attribute value of the PSG
        /// that the PSG message is sent with the AddOrUpdate Action
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_ProhibitDiscountDoesNotMatchPSGAttributeValue_CreateProductSelectionGroupElementShouldBeDelete()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedFoodStampPsgName = ItemPublisherConstants.ProductSelectionGroups.FoodStamp;
            string expectedProhibitDiscountPsgName = "Prohibit_Discount_Name";

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedFoodStampPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "ProhibitDiscount",
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedProhibitDiscountPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
             new Item()
             {
                 ItemAttributesJson = "{ 'FoodStampEligible':'false', 'ProhibitDiscount':'false', 'Inactive':'false'  }"
             },
             null,
             null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.Delete, response.First(x => x.id == expectedProhibitDiscountPsgName).Action);
        }

        /// <summary>
        /// Tests that when we build product selection groups that if the MerchandiseHierarchyClassId in Item matches
        /// the Merchandise HierarchyClassId value of the PSG that the PSG message is sent with the AddOrUpdate Action
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_ItemMerchAssociationMatchesPsgMerchHierarchyId_PsgCreateWithAddOrUpdateAction()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedFoodStampPsgName = ItemPublisherConstants.ProductSelectionGroups.FoodStamp;
            string expectedProhibitDiscountPsgName = "Prohibit_Discount_Name";
            string expectedMerchPsg = "Unit_Test_MerchId_PSG";
            int? expectedHierarchyClassId = 345234;

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedFoodStampPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "ProhibitDiscount",
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedProhibitDiscountPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            productSelectionGroups[3] = new ProductSelectionGroup()
            {
                AttributeId = null,
                AttributeName = null,
                AttributeValue = null,
                MerchandiseHierarchyClassId = expectedHierarchyClassId,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedMerchPsg,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = null,
                TraitValue = null
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
             new Item()
             {
                 ItemId = 11,
                 ItemAttributesJson = "{ 'FoodStampEligible':'false', 'ProhibitDiscount':'false', 'Inactive':'false'  }"
             },
             new List<Hierarchy>
             {
                 new Hierarchy
                 {
                     HierarchyClassId = expectedHierarchyClassId.Value,
                     HierarchyClassName = "Unit Test Merch SubBrick 1",
                     HierarchyClassParentId = 123456,
                     HierarchyId = Icon.Framework.Hierarchies.Merchandise,
                     HierarchyLevel = 5,
                     HierarchyName = Icon.Framework.HierarchyNames.Merchandise,
                     ItemId = 11
                 }
             },
             null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.First(x => x.id == expectedMerchPsg).Action);
        }

        /// <summary>
        /// Tests that when we build product selection groups that if the MerchandiseHierarchyClassId in Item matches
        /// the Merchandise HierarchyClassId value of the PSG that the PSG message is sent with the AddOrUpdate Action
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_ItemMerchAssociationDoesNotMatchesPsgMerchHierarchyId_PsgCreateWithDeleteAction()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedFoodStampPsgName = ItemPublisherConstants.ProductSelectionGroups.FoodStamp;
            string expectedProhibitDiscountPsgName = "Prohibit_Discount_Name";
            string expectedMerchPsg = "Unit_Test_MerchId_PSG";
            int? expectedHierarchyClassId = 345234;

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();
            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = ItemPublisherConstants.Attributes.FoodStampEligible,
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedFoodStampPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };

            productSelectionGroups[2] = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "ProhibitDiscount",
                AttributeValue = "true",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedProhibitDiscountPsgName,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName2",
                TraitId = 5,
                TraitValue = "TraitValue2"
            };

            productSelectionGroups[3] = new ProductSelectionGroup()
            {
                AttributeId = null,
                AttributeName = null,
                AttributeValue = null,
                MerchandiseHierarchyClassId = expectedHierarchyClassId,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedMerchPsg,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = null,
                TraitValue = null
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
             new Item()
             {
                 ItemId = 11,
                 ItemAttributesJson = "{ 'FoodStampEligible':'false', 'ProhibitDiscount':'false', 'Inactive':'false'  }"
             },
             new List<Hierarchy>
             {
                 new Hierarchy
                 {
                     HierarchyClassId = 98, // different from psg merch hierarchy class id
                     HierarchyClassName = "Unit Test Merch SubBrick 1",
                     HierarchyClassParentId = 123456,
                     HierarchyId = Icon.Framework.Hierarchies.Merchandise,
                     HierarchyLevel = 5,
                     HierarchyName = Icon.Framework.HierarchyNames.Merchandise,
                     ItemId = 11
                 }
             },
             null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.Delete, response.First(x => x.id == expectedMerchPsg).Action);
        }

        /// <summary>
        /// Test what happens when there are two PSGs with the same name but have different MerchandiseHierarchyClassIds to check against
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_TwoPsgWithSameNameOneItemMerchMatchesHierarchyClassId_PsgCreateWithAddOrUpdateAction()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedMerchPsg = "Unit_Test_MerchId_PSG";
            int? expectedHierarchyClassId = 345234;

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();

            productSelectionGroups[0] = new ProductSelectionGroup()
            {
                AttributeId = null,
                AttributeName = null,
                AttributeValue = null,
                MerchandiseHierarchyClassId = expectedHierarchyClassId,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedMerchPsg,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = null,
                TraitValue = null
            };

            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = null,
                AttributeName = null,
                AttributeValue = null,
                MerchandiseHierarchyClassId = 956458,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedMerchPsg,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = null,
                TraitValue = null
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
             new Item()
             {
                 ItemId = 11,
                 ItemAttributesJson = "{ 'FoodStampEligible':'false', 'ProhibitDiscount':'false', 'Inactive':'false'  }"
             },
             new List<Hierarchy>
             {
                 new Hierarchy
                 {
                     HierarchyClassId = expectedHierarchyClassId.Value,
                     HierarchyClassName = "Unit Test Merch SubBrick 1",
                     HierarchyClassParentId = 123456,
                     HierarchyId = Icon.Framework.Hierarchies.Merchandise,
                     HierarchyLevel = 5,
                     HierarchyName = Icon.Framework.HierarchyNames.Merchandise,
                     ItemId = 11
                 }
             },
             null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.First(x => x.id == expectedMerchPsg).Action);
        }

        /// <summary>
        /// Test what happens when there are two PSGs with the same name but have different MerchandiseHierarchyClassIds to check against
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildProductSelectionGroups_TwoPsgWithSameNameOneItemMerchMatchesHierarchyClassId_NoDuplicatesCreated()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            string expectedMerchPsg = "Unit_Test_MerchId_PSG";
            int? expectedHierarchyClassId = 345234;

            var productSelectionGroups = new ConcurrentDictionary<int, ProductSelectionGroup>();

            productSelectionGroups[0] = new ProductSelectionGroup()
            {
                AttributeId = null,
                AttributeName = null,
                AttributeValue = null,
                MerchandiseHierarchyClassId = expectedHierarchyClassId,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedMerchPsg,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = null,
                TraitValue = null
            };

            productSelectionGroups[1] = new ProductSelectionGroup()
            {
                AttributeId = null,
                AttributeName = null,
                AttributeValue = null,
                MerchandiseHierarchyClassId = 956458,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = expectedMerchPsg,
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "Consumable",
                TraitId = null,
                TraitValue = null
            };

            serviceCacheMock.Setup(x => x.ProductSelectionGroupCache).Returns(productSelectionGroups);

            Action<string> processLogger = (string message) =>
            {
            };

            MessageQueueItemModelBuilder messageQueueItemModelBuilder = new MessageQueueItemModelBuilder(new ItemMapper());
            MessageQueueItemModel model = messageQueueItemModelBuilder.Build(
             new Item()
             {
                 ItemId = 11,
                 ItemAttributesJson = "{ 'FoodStampEligible':'false', 'ProhibitDiscount':'false', 'Inactive':'false'  }"
             },
             new List<Hierarchy>
             {
                 new Hierarchy
                 {
                     HierarchyClassId = expectedHierarchyClassId.Value,
                     HierarchyClassName = "Unit Test Merch SubBrick 1",
                     HierarchyClassParentId = 123456,
                     HierarchyId = Icon.Framework.Hierarchies.Merchandise,
                     HierarchyLevel = 5,
                     HierarchyName = Icon.Framework.HierarchyNames.Merchandise,
                     ItemId = 11
                 }
             },
             null);

            // When.
            List<GroupTypeType> response = await builder.BuildProductSelectionGroups(model, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.Single(x => x.id == expectedMerchPsg).Action);
        }

        /// <summary>
        /// Tests that when we create a CreateProductSelectionGroupElement that the return is in the correct format
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateProductSelectionGroupElement_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            var productSelectionGroup = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "AttributeName",
                AttributeValue = "AttributeValue",
                MerchandiseHierarchyClassId = null,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };
            Action<string> processLogger = (string message) =>
            {
            };

            // When.
            GroupTypeType response = await builder.CreateProductSelectionGroupElement(productSelectionGroup, true, processLogger);

            //Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.Action);
            Assert.AreEqual(true, response.ActionSpecified);
            Assert.AreEqual("ProductSelectionGroupName", response.name);
            Assert.AreEqual("ProductSelectionGroupTypeName", response.type);
        }

        /// <summary>
        /// Tests that when we create a CreateProductSelectionGroupElement and the entry is an AddOrUpdate that
        /// the Action property is set to AddOrUpdate in the response
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateProductSelectionGroupElement_AddOrUpdateIsTrue_ActionIsSetToAddOrUpdate()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            var productSelectionGroup = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "AttributeName",
                AttributeValue = "AttributeValue",
                MerchandiseHierarchyClassId = 2,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };
            Action<string> processLogger = (string message) =>
            {
            };

            // When.
            GroupTypeType response = await builder.CreateProductSelectionGroupElement(productSelectionGroup, true, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, response.Action);
        }

        /// <summary>
        /// Tests that when we create a CreateProductSelectionGroupElement and the entry is not AddOrUpdate that
        /// the Action property is set to Delete in the response
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateProductSelectionGroupElement_AddOrUpdateIsFalse_ActionIsSetToDelete()
        {
            //Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            var productSelectionGroup = new ProductSelectionGroup()
            {
                AttributeId = 1,
                AttributeName = "AttributeName",
                AttributeValue = "AttributeValue",
                MerchandiseHierarchyClassId = 2,
                ProductSelectionGroupId = 3,
                ProductSelectionGroupName = "ProductSelectionGroupName",
                ProductSelectionGroupTypeId = 4,
                ProductSelectionGroupTypeName = "ProductSelectionGroupTypeName",
                TraitId = 5,
                TraitValue = "TraitValue"
            };
            Action<string> processLogger = (string message) =>
            {
            };
            // When.
            GroupTypeType response = await builder.CreateProductSelectionGroupElement(productSelectionGroup, false, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.Delete, response.Action);
        }

        [TestMethod]
        public async Task BuildConsumerInformation_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            ConsumerInformationType result = await builder.BuildConsumerInformation(this.testDataFactory.Nutrition, processLogger);

            // Then.
            Assert.AreEqual(ActionEnum.AddOrUpdate, result.stockItemConsumerProductLabel.Action);
            Assert.IsTrue(result.stockItemConsumerProductLabel.ActionSpecified);
            Assert.AreEqual(10m, result.stockItemConsumerProductLabel.calciumDailyMinimumPercent);
            Assert.AreEqual(80m, result.stockItemConsumerProductLabel.caloriesCount);
            Assert.AreEqual(20m, result.stockItemConsumerProductLabel.caloriesFromFatCount);
            Assert.AreEqual(6.0m, result.stockItemConsumerProductLabel.cholesterolMilligramsCount);
            Assert.AreEqual(23m, result.stockItemConsumerProductLabel.cholesterolPercent);
            Assert.IsNull(result.stockItemConsumerProductLabel.consumerLabelTypeCode);
            Assert.AreEqual(2.0m, result.stockItemConsumerProductLabel.dietaryFiberGramsCount);
            Assert.IsNull(result.stockItemConsumerProductLabel.hazardousMaterialTypeCode);
            Assert.AreEqual(5m, result.stockItemConsumerProductLabel.ironDailyMinimumPercent);
            Assert.IsFalse(result.stockItemConsumerProductLabel.isHazardousMaterial);
            Assert.IsNull(result.stockItemConsumerProductLabel.maxCalories);
            Assert.IsNull(result.stockItemConsumerProductLabel.minCalories);
            Assert.IsNull(result.stockItemConsumerProductLabel.nutritionalDescriptionText);
            Assert.AreEqual(1.0m, result.stockItemConsumerProductLabel.proteinGramsCount);
            Assert.AreEqual(6.0m, result.stockItemConsumerProductLabel.saturatedFatGramsAmount);
            Assert.AreEqual(10m, result.stockItemConsumerProductLabel.saturatedFatPercent);
            Assert.IsNull(result.stockItemConsumerProductLabel.servingsInRetailSaleUnitCount);
            Assert.IsNull(result.stockItemConsumerProductLabel.servingSizeUom);
            Assert.AreEqual(0, result.stockItemConsumerProductLabel.servingSizeUomCount);
            Assert.AreEqual(130m, result.stockItemConsumerProductLabel.sodiumMilligramsCount);
            Assert.AreEqual(5m, result.stockItemConsumerProductLabel.sodiumPercent);
            Assert.AreEqual(14m, result.stockItemConsumerProductLabel.sugarsGramsCount);
            Assert.AreEqual(16m, result.stockItemConsumerProductLabel.totalCarbohydrateMilligramsCount);
            Assert.AreEqual(5m, result.stockItemConsumerProductLabel.totalCarbohydratePercent);
            Assert.AreEqual(3m, result.stockItemConsumerProductLabel.totalFatDailyIntakePercent);
            Assert.AreEqual(2m, result.stockItemConsumerProductLabel.totalFatGramsAmount);
            Assert.AreEqual(6m, result.stockItemConsumerProductLabel.vitaminADailyMinimumPercent);
            Assert.AreEqual(0m, result.stockItemConsumerProductLabel.vitaminBDailyMinimumPercent);
            Assert.AreEqual(3m, result.stockItemConsumerProductLabel.vitaminCDailyMinimumPercent);
            Assert.AreEqual(7, result.stockItemConsumerProductLabel.addedSugarDailyPercent);
            Assert.AreEqual(8m, result.stockItemConsumerProductLabel.addedSugarsGramsCount);
            Assert.AreEqual(9m, result.stockItemConsumerProductLabel.calciumMilligramsCount);
            Assert.AreEqual(10m, result.stockItemConsumerProductLabel.ironMilligramsCount);
            Assert.AreEqual(11m, result.stockItemConsumerProductLabel.vitaminDMicrogramsCount);

            Assert.IsTrue(result.stockItemConsumerProductLabel.calciumDailyMinimumPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.caloriesCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.caloriesFromFatCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.cholesterolMilligramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.cholesterolPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.dietaryFiberGramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.ironDailyMinimumPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.proteinGramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.saturatedFatGramsAmountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.saturatedFatPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.servingSizeUomCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.sodiumMilligramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.sodiumPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.sugarsGramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.totalCarbohydrateMilligramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.totalCarbohydratePercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.totalFatDailyIntakePercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.totalFatGramsAmountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.vitaminADailyMinimumPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.vitaminBDailyMinimumPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.vitaminCDailyMinimumPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.addedSugarDailyPercentSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.addedSugarsGramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.calciumMilligramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.ironMilligramsCountSpecified);
            Assert.IsTrue(result.stockItemConsumerProductLabel.vitaminDMicrogramsCountSpecified);
        }

        /// <summary>
        /// Tests that if the "new" nutrition elements AddedSugarsPercent, AddedSugarsWeight, CalciumWeight, IronWeight,VitaminDWeight
        /// are null that the specified property is false in the ESB message.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildConsumerInformation_NewNutritionElementsAreNull_ElementsShouldNotBeSpecified()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            Action<string> processLogger = (message) =>
            {
            };
            Nutrition nutrition = this.testDataFactory.Nutrition;

            // When.
            nutrition.AddedSugarsPercent = null;
            nutrition.AddedSugarsWeight = null;
            nutrition.CalciumWeight = null;
            nutrition.IronWeight = null;
            nutrition.VitaminDWeight = null;

            ConsumerInformationType result = await builder.BuildConsumerInformation(nutrition, processLogger);

            // Then.
            Assert.IsFalse(result.stockItemConsumerProductLabel.addedSugarDailyPercentSpecified);
            Assert.IsFalse(result.stockItemConsumerProductLabel.addedSugarsGramsCountSpecified);
            Assert.IsFalse(result.stockItemConsumerProductLabel.calciumMilligramsCountSpecified);
            Assert.IsFalse(result.stockItemConsumerProductLabel.ironMilligramsCountSpecified);
            Assert.IsFalse(result.stockItemConsumerProductLabel.vitaminDMicrogramsCountSpecified);
        }

        [TestMethod]
        public void IsNutritionRemoved_NutritionIsRemoved_ReturnsTrue()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            Nutrition nutrition = new Nutrition()
            {
                RecipeName = "DELETED"
            };

            // When.
            bool result = builder.IsNutritionRemoved(nutrition);

            // When.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNutritionRemoved_NullNutrition_ReturnsTrue()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            // When.
            bool result = builder.IsNutritionRemoved(null);

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNutritionRemoved_NutritionIsRemoved_ReturnsFalse()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            Nutrition nutrition = new Nutrition()
            {
                RecipeName = "Test"
            };

            // When.
            bool result = builder.IsNutritionRemoved(nutrition);

            //Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task BuildScanCodeType_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<ITraitMessageBuilder> traitBuilderMock = new Mock<ITraitMessageBuilder>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, traitBuilderMock.Object, hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            Action<string> processLogger = (message) =>
            {
            };

            // When.
            ScanCodeType result = await builder.BuildScanCodeType(this.testDataFactory.MessageQueueItemModel, processLogger);

            // Then.
            Assert.AreEqual("1568", result.code);
            Assert.AreEqual(95, result.id);
            Assert.IsFalse(result.idSpecified);
            Assert.AreEqual("POS PLU", result.typeDescription);
            Assert.AreEqual(2, result.typeId);
            Assert.IsTrue(result.typeIdSpecified);
        }

        [TestMethod]
        public async Task BuildTraits_ReturnIsInCorrectFormat()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            valueFormatterMock.Setup(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.IsAny<string>())).Returns("B");
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, new TraitMessageBuilder(), hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
               new Attributes()
               {
                   AttributeId = 1,
                   AttributeName = "AttributeName",
                   AttributeDisplayName = "AttributeDisplayName",
                   Description = "Description",
                   TraitCode = "TraitCode"
               }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = "test"
             }));

            Action<string> processLogger = (message) =>
            {
            };

            var nutrition = new Nutrition()
            {
            };
            // When.
            List<TraitType> result = await builder.BuildTraits(new Dictionary<string, string>() { { "test", "B" } }, nutrition, processLogger);

            // Then.
            Assert.AreEqual(50, result.Count);
            Assert.AreEqual(ActionEnum.Add, result.First().Action);
            Assert.IsFalse(result.First().ActionSpecified);
            Assert.AreEqual("TraitCode", result.First().code);
            Assert.IsNull(result.First().group);
            Assert.IsNull(result.First().pattern);
            Assert.IsNull(result.First().type.value.First().uom);
        }

        [TestMethod()]
        public async Task BuildTraits_NutritionTraitsArePopulated()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, new TraitMessageBuilder(), hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
               new Attributes()
               {
                   AttributeId = 1,
                   AttributeName = "AttributeName",
                   AttributeDisplayName = "AttributeDisplayName",
                   Description = "Description",
                   TraitCode = "TraitCode"
               }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = "test"
             }));

            Action<string> processLogger = (message) =>
            {
            };

            var nutrition = new Nutrition()
            {
                Allergens = "Allergens",
                Betacarotene = 1,
                Biotin = 2,
                Calcium = 3,
                Calories = 4,
                CaloriesFat = 5,
                CaloriesFromTransfat = 6,
                CaloriesSaturatedFat = 7,
                Chloride = 8,
                CholesterolPercent = 9,
                CholesterolWeight = 10,
                Chromium = 11,
                Copper = 12,
                DietaryFiberPercent = 13,
                DietaryFiberWeight = 14,
                Folate = 15,
                HshRating = 16,
                Ingredients = "Ingredients",
                InsertDate = DateTime.Parse("2000-01-01"),
                InsolubleFiber = 18,
                Iodine = 19,
                Iron = 20,
                Magnesium = 21,
                Manganese = 22,
                ModifiedDate = DateTime.Parse("2000-01-01"),
                Molybdenum = 23,
                MonounsaturatedFat = 24,
                Niacin = 25,
                Om3Fatty = 26,
                Om6Fatty = 27,
                OtherCarbohydrates = 28,
                PantothenicAcid = 29,
                Phosphorous = 30,
                Plu = "PLU",
                PolyunsaturatedFat = 31,
                PotassiumPercent = 32,
                PotassiumWeight = 33,
                ProteinPercent = 34,
                ProteinWeight = 35,
                RecipeName = "RecipeName",
                Riboflavin = 36,
                SaturatedFatPercent = 37,
                SaturatedFatWeight = 38,
                Selenium = 39,
                ServingPerContainer = "ServingPerContainer",
                ServingSizeDesc = "ServingSizeDesc",
                ServingUnits = 42,
                ServingsPerPortion = 43,
                SizeWeight = 44,
                SodiumPercent = 45,
                SodiumWeight = 46,
                SolubleFiber = 47,
                Starch = 48,
                Sugar = 49,
                SugarAlcohol = 50,
                Thiamin = 52,
                TotalCarbohydratePercent = 53,
                TotalCarbohydrateWeight = 54,
                TotalFatPercentage = 55,
                TotalFatWeight = 56,
                Transfat = 57,
                TransfatWeight = 58,
                VitaminA = 59,
                VitaminB12 = 60,
                VitaminB6 = 61,
                VitaminC = 62,
                VitaminD = 63,
                VitaminE = 64,
                VitaminK = 65,
                Zinc = 66
            };

            // When.
            List<TraitType> result = await builder.BuildTraits(new Dictionary<string, string>() { { "test", "B" } }, nutrition, processLogger);

            // Then.
            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.RecipeName
            && x.type.description == Icon.Framework.TraitDescriptions.RecipeName
            && x.type.value.First().value == "RecipeName"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Allergens
            && x.type.description == Icon.Framework.TraitDescriptions.Allergens
            && x.type.value.First().value == "Allergens"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Ingredients
            && x.type.description == Icon.Framework.TraitDescriptions.Ingredients
            && x.type.value.First().value == "Ingredients"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Hsh
            && x.type.description == Icon.Framework.TraitDescriptions.Hsh
            && x.type.value.First().value == "16"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.PolyunsaturatedFat
            && x.type.description == Icon.Framework.TraitDescriptions.PolyunsaturatedFat
            && x.type.value.First().value == "31"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.MonounsaturatedFat
            && x.type.description == Icon.Framework.TraitDescriptions.MonounsaturatedFat
            && x.type.value.First().value == "24"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.PotassiumWeight
            && x.type.description == Icon.Framework.TraitDescriptions.PotassiumWeight
            && x.type.value.First().value == "33"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.PotassiumPercent
            && x.type.description == Icon.Framework.TraitDescriptions.PotassiumPercent
            && x.type.value.First().value == "32"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.DietaryFiberPercent
            && x.type.description == Icon.Framework.TraitDescriptions.DietaryFiberPercent
            && x.type.value.First().value == "13"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.SolubleFiber
            && x.type.description == Icon.Framework.TraitDescriptions.SolubleFiber
            && x.type.value.First().value == "47"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.InsolubleFiber
            && x.type.description == Icon.Framework.TraitDescriptions.InsolubleFiber
            && x.type.value.First().value == "18"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.SugarAlcohol
            && x.type.description == Icon.Framework.TraitDescriptions.SugarAlcohol
            && x.type.value.First().value == "50"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.OtherCarbohydrates
            && x.type.description == Icon.Framework.TraitDescriptions.OtherCarbohydrates
            && x.type.value.First().value == "28"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.ProteinPercent
            && x.type.description == Icon.Framework.TraitDescriptions.ProteinPercent
            && x.type.value.First().value == "34"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Betacarotene
            && x.type.description == Icon.Framework.TraitDescriptions.Betacarotene
            && x.type.value.First().value == "1"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.VitaminD
            && x.type.description == Icon.Framework.TraitDescriptions.VitaminD
            && x.type.value.First().value == "63"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.VitaminE
            && x.type.description == Icon.Framework.TraitDescriptions.VitaminE
            && x.type.value.First().value == "64"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Thiamin
            && x.type.description == Icon.Framework.TraitDescriptions.Thiamin
            && x.type.value.First().value == "52"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Riboflavin
            && x.type.description == Icon.Framework.TraitDescriptions.Riboflavin
            && x.type.value.First().value == "36"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Niacin
            && x.type.description == Icon.Framework.TraitDescriptions.Niacin
            && x.type.value.First().value == "25"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.VitaminB6
            && x.type.description == Icon.Framework.TraitDescriptions.VitaminB6
            && x.type.value.First().value == "61"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Folate
            && x.type.description == Icon.Framework.TraitDescriptions.Folate
            && x.type.value.First().value == "15"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Biotin
            && x.type.description == Icon.Framework.TraitDescriptions.Biotin
            && x.type.value.First().value == "2"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.PantothenicAcid
            && x.type.description == Icon.Framework.TraitDescriptions.PantothenicAcid
            && x.type.value.First().value == "29"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Phosphorous
            && x.type.description == Icon.Framework.TraitDescriptions.Phosphorous
            && x.type.value.First().value == "30"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Iodine
            && x.type.description == Icon.Framework.TraitDescriptions.Iodine
            && x.type.value.First().value == "19"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Magnesium
            && x.type.description == Icon.Framework.TraitDescriptions.Magnesium
            && x.type.value.First().value == "21"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Zinc
            && x.type.description == Icon.Framework.TraitDescriptions.Zinc
            && x.type.value.First().value == "66"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Copper
            && x.type.description == Icon.Framework.TraitDescriptions.Copper
            && x.type.value.First().value == "12"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Transfat
            && x.type.description == Icon.Framework.TraitDescriptions.Transfat
            && x.type.value.First().value == "57"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Om6Fatty
            && x.type.description == Icon.Framework.TraitDescriptions.Om6Fatty
            && x.type.value.First().value == "27"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Om3Fatty
            && x.type.description == Icon.Framework.TraitDescriptions.Om3Fatty
            && x.type.value.First().value == "26"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Starch
            && x.type.description == Icon.Framework.TraitDescriptions.Starch
            && x.type.value.First().value == "48"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Chloride
            && x.type.description == Icon.Framework.TraitDescriptions.Chloride
            && x.type.value.First().value == "8"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Chromium
            && x.type.description == Icon.Framework.TraitDescriptions.Chromium
            && x.type.value.First().value == "11"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.VitaminK
            && x.type.description == Icon.Framework.TraitDescriptions.VitaminK
            && x.type.value.First().value == "65"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Manganese
            && x.type.description == Icon.Framework.TraitDescriptions.Manganese
            && x.type.value.First().value == "22"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.Molybdenum
            && x.type.description == Icon.Framework.TraitDescriptions.Molybdenum
            && x.type.value.First().value == "23"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.TransfatWeight
            && x.type.description == Icon.Framework.TraitDescriptions.TransfatWeight
            && x.type.value.First().value == "58"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.CaloriesFromTransFat
            && x.type.description == Icon.Framework.TraitDescriptions.CaloriesFromTransFat
            && x.type.value.First().value == "6"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.CaloriesSaturatedFat
            && x.type.description == Icon.Framework.TraitDescriptions.CaloriesSaturatedFat
            && x.type.value.First().value == "7"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.ServingPerContainer
            && x.type.description == Icon.Framework.TraitDescriptions.ServingPerContainer
            && x.type.value.First().value == "ServingPerContainer"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.ServingSizeDesc
            && x.type.description == Icon.Framework.TraitDescriptions.ServingSizeDesc
            && x.type.value.First().value == "ServingSizeDesc"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.ServingsPerPortion
            && x.type.description == Icon.Framework.TraitDescriptions.ServingsPerPortion
            && x.type.value.First().value == "43"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.ServingUnits
            && x.type.description == Icon.Framework.TraitDescriptions.ServingUnits
            && x.type.value.First().value == "42"));

            Assert.IsTrue(result.Any(x => x.code == Icon.Framework.TraitCodes.SizeWeight
            && x.type.description == Icon.Framework.TraitDescriptions.SizeWeight
            && x.type.value.First().value == "44"));
        }

        [TestMethod()]
        public async Task BuildTraitsFromAttributes_AttributeIsAdded()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            valueFormatterMock.Setup(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.IsAny<string>())).Returns("B");
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, new TraitMessageBuilder(), hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
               new Attributes()
               {
                   AttributeId = 1,
                   AttributeName = "AttributeName",
                   AttributeDisplayName = "AttributeDisplayName",
                   Description = "Description",
                   TraitCode = "TraitCode",
                   XmlTraitDescription = "XmlTraitDescription"
               }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = "test"
             }));

            Action<string> processLogger = (message) =>
            {
            };

            var nutrition = new Nutrition()
            {
            };
            // When.
            List<TraitType> result = await builder.BuildTraits(new Dictionary<string, string>() { { "test", "B" } }, nutrition, processLogger);

            // Then.
            Assert.AreEqual("B", result.First().type.value.First().value);
            Assert.AreEqual("XmlTraitDescription", result.First().type.description);
        }

        [TestMethod()]
        public async Task BuildTraitsFromAttributes_AttributeExists_FormatsAttributeValueIsCalled()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();

            valueFormatterMock.Setup(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.IsAny<string>())).Returns("true");
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, new TraitMessageBuilder(), hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
                    new Attributes()
                    {
                        AttributeId = 1,
                        AttributeName = "AttributeName",
                        AttributeDisplayName = "AttributeDisplayName",
                        Description = "Description",
                        TraitCode = "TraitCode",
                        XmlTraitDescription = "XmlTraitDescription1",
                        DataTypeName = "Boolean"
                    }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = "test"
             }));

            Action<string> processLogger = (message) =>
            {
            };

            var nutrition = new Nutrition()
            {
            };

            // When.
            List<TraitType> result = await builder.BuildTraitsFromAttributes(
                      new Dictionary<string, string>()
                {
                    { "testTrue", "true" },
                    { "testFalse", "false" }
                },
                processLogger);

            // Then.
            valueFormatterMock.Verify(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.Is<string>(v => v == "true")), Times.Once);
        }

        [TestMethod()]
        public async Task BuildTraitsFromAttributes_AttributeValueIsBlank_TraitIsNotAddedToMessage()
        {
            // Given.
            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            valueFormatterMock.Setup(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.IsAny<string>())).Returns(string.Empty);
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, new TraitMessageBuilder(), hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
                    new Attributes()
                    {
                        AttributeId = 1,
                        AttributeName = ItemPublisherConstants.Attributes.Kosher,
                        AttributeDisplayName = ItemPublisherConstants.Attributes.Kosher,
                        Description = "Description",
                        TraitCode = "TraitCode",
                        XmlTraitDescription = "XmlTraitDescription1",
                        DataTypeName = "Boolean"
                    }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
             new HierarchyCacheItem()
             {
                 HierarchyId = 1,
                 HierarchyName = "test"
             }));

            Action<string> processLogger = (message) =>
            {
            };

            var nutrition = new Nutrition()
            {
            };

            // When.
            List<TraitType> result = await builder.BuildTraitsFromAttributes(
             new Dictionary<string, string>()
                {
                    { ItemPublisherConstants.Attributes.KitchenDescription, "" }
                },
                processLogger);

            // Then.
            Assert.AreEqual(0, result.Count);
            valueFormatterMock.Verify(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task BuildTraitFromAttribute_TraitCodeIsRUM_UomCodeAndNameBuilt()
        {
            // Given.
            string expectedValue = "OZ";
            string expectedUomCode = "OZ";
            string expectedUomName = "OUNCES";

            Mock<ILogger<EsbMessageBuilder>> loggerMock = new Mock<ILogger<EsbMessageBuilder>>();
            Mock<IEsbServiceCache> serviceCacheMock = new Mock<IEsbServiceCache>();
            Mock<IHierarchyValueParser> hierarchyValuesParserMock = new Mock<IHierarchyValueParser>();
            Mock<IValueFormatter> valueFormatterMock = new Mock<IValueFormatter>();
            valueFormatterMock.Setup(x => x.FormatValueForMessage(It.IsAny<Attributes>(), It.IsAny<string>())).Returns(expectedValue);
            Mock<IUomMapper> uomMapperMock = new Mock<IUomMapper>();
            EsbMessageBuilder builder = new EsbMessageBuilder(loggerMock.Object, serviceCacheMock.Object, new TraitMessageBuilder(), hierarchyValuesParserMock.Object, valueFormatterMock.Object, uomMapperMock.Object, new ServiceSettings());

            serviceCacheMock.Setup(x => x.AttributeFromCache(It.IsAny<string>())).Returns(Task.FromResult<Attributes>(
               new Attributes()
               {
                   AttributeId = 1,
                   AttributeName = "UOM",
                   AttributeDisplayName = "UOM",
                   Description = "Description",
                   TraitCode = "RUM",
                   XmlTraitDescription = "Retail UOM"
               }));

            serviceCacheMock.Setup(x => x.HierarchyFromCache(It.IsAny<string>())).Returns(Task.FromResult<HierarchyCacheItem>(
                 new HierarchyCacheItem()
                 {
                     HierarchyId = 1,
                     HierarchyName = "test"
                 }));

            uomMapperMock.Setup(u => u.GetEsbUomCode(It.Is<string>(s => s.Equals(expectedUomCode)))).Returns(WfmUomCodeEnumType.OZ);

            Action<string> processLogger = (message) =>
            {
            };

            var nutrition = new Nutrition()
            {
            };
            // When.
            List<TraitType> result = await builder.BuildTraits(new Dictionary<string, string>() { { "UOM", "OZ" } }, nutrition, processLogger);

            // Then.
            Assert.AreEqual(expectedValue, result.First().type.value.First().value);
            Assert.AreEqual("Retail UOM", result.First().type.description);
            Assert.IsNotNull(result.First().type.value.FirstOrDefault()?.uom.code);
            Assert.IsNotNull(result.First().type.value.FirstOrDefault()?.uom.codeSpecified);
            Assert.IsNotNull(result.First().type.value.FirstOrDefault()?.uom.name);
            Assert.IsNotNull(result.First().type.value.FirstOrDefault()?.uom.nameSpecified);
        }
    }
}