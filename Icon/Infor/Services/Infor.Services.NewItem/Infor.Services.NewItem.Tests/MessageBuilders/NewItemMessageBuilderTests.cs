using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Esb.Core.Serializer;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Esb.Core.Mappers;
using Services.NewItem.Cache;
using Services.NewItem.MessageBuilders;
using Services.NewItem.Models;
using System.Collections.Generic;
using System.IO;
using Icon.Common.DataAccess;
using Services.NewItem.Queries;
using System.Linq;
using System.Xml.Linq;
using Icon.Framework;

namespace Services.NewItem.Tests.MessageBuilders
{
    [TestClass]
    public class NewItemMessageBuilderTests
    {
        private Mock<IUomMapper> mockUomMapper;
        private Serializer<Contracts.items> serializer;
        private Mock<IIconCache> mockIconCache;
        private NewItemMessageBuilder newItemMessageBuilder;
        private Mock<IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>> mockGetItemIdsQueryHandler;
        private NewItemApplicationSettings settings;

        [TestInitialize]
        public void InitializeTests()
        {
            settings = new NewItemApplicationSettings { SendOrganic = false };
            this.mockUomMapper = new Mock<IUomMapper>();
            serializer = new Serializer<Contracts.items>();
            this.mockIconCache = new Mock<IIconCache>();
            this.mockGetItemIdsQueryHandler = new Mock<IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>>();

            this.newItemMessageBuilder = new NewItemMessageBuilder(this.mockUomMapper.Object, serializer, this.mockIconCache.Object, this.mockGetItemIdsQueryHandler.Object, settings);

            this.mockIconCache.SetupGet(m => m.NationalClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "12345", 22 } });
            this.mockIconCache.SetupGet(m => m.TaxClassCodesToIdDictionary)
                .Returns(new Dictionary<string, int> { { "1111111", 23 } });
            this.mockIconCache.SetupGet(m => m.BrandIdToAbbreviationDictionary)
                .Returns(new Dictionary<int, string> { { 5, "Test Brand Abbr" } });
            this.mockIconCache.SetupGet(m => m.NationalClassModels)
                .Returns(new Dictionary<string, NationalClassModel> { { "12345", new NationalClassModel { Name = "Test National", HierarchyParentClassId = 1 } } });
            this.mockIconCache.SetupGet(m => m.TaxDictionary)
                .Returns(new Dictionary<string, TaxClassModel> { { "1111111", new TaxClassModel { Name = "Test Tax" } } });
            this.mockIconCache.SetupGet(m => m.BrandDictionary)
                .Returns(new Dictionary<int, BrandModel> { { 5, new BrandModel { Name = "Test Brand Abbr" } } });
            this.mockIconCache.SetupGet(m => m.SubTeamModels)
                .Returns(new Dictionary<string, SubTeamModel> { { "1234", new SubTeamModel { FinancialHierarchyCode = "1234", HierarchyClassName = "Test Sub Team (1234)" } } });
            this.mockUomMapper.Setup(m => m.GetEsbUomCode("EA"))
                .Returns(Contracts.WfmUomCodeEnumType.EA);
            this.mockUomMapper.Setup(m => m.GetEsbUomDescription("EA"))
                .Returns(Contracts.WfmUomDescEnumType.EACH);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeNewItems_ShouldReturnItemXmlStringForThreeItems()
        {
            // Given
            List<NewItemModel> newItemModel = BuildNewItemModel(numberOfItems: 3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(new Dictionary<string, int>
                {
                    { "test1", 111 },
                    { "test2", 112},
                    { "test3", 113 }
                });

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString(
                @"TestMessages\ThreeNewItems.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeNewItemsWithNoMatchingBrandTaxNationalClass_ShouldReturnItemXmlStringForThreeItems()
        {
            // Given   
            List<NewItemModel> newItemModel = BuildNewItemBlankHierarchyClassesModel(3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(new Dictionary<string, int>
                {
                    { "test1", 111 },
                    { "test2", 112},
                    { "test3", 113 }
                });

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString
                (@"TestMessages\ThreeNewItemsWithNoMatchingBrandTaxNationalCode.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeItemsThatExistInIcon_ShouldReturnItemXmlStringForThreeItemsWithIdsSetToIconIds()
        {
            // Given
            List<NewItemModel> newItemModel = BuildNewItemModel(numberOfItems: 3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => int.Parse(m.ScanCode)));

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString
                (@"TestMessages\ThreeExistingItems.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeItemsThatExistInIconWithCFDs_ShouldReturnItemXmlStringWithCFDs()
        {
            // Given
            settings.IncludeCustomerFacingDescription = true;
            List<NewItemModel> newItemModel = BuildNewItemModel(numberOfItems: 3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => int.Parse(m.ScanCode)));

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString
                (@"TestMessages\ThreeExistingItemsWithCustomerFacingDescriptions.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenItemsSubTeamNumberDoesntExistInIconCache_ShouldReturnItemMessageWithDefaultNoSubTeam()
        {
            // Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].SubTeamNumber = "9876";

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => int.Parse(m.ScanCode)));

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString(
                @"TestMessages\OneExistingItemWithNoSubTeam.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenAUpc_ShouldBuildMessageWithUpcScanCodeType()
        {
            //Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].ScanCode = "12345678";

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => int.Parse(m.ScanCode)));

            //When
            var actualXml = this.newItemMessageBuilder.BuildMessage(newItemModel);

            //Then
            var actualScanCodeType = XDocument.Parse(actualXml)
                .Descendants()
                .Where(e => e.Name.LocalName == "typeDescription")
                .First()
                .Value;
            Assert.AreEqual("UPC", actualScanCodeType);
        }

        [TestMethod]
        public void BuildMessage_GivenAPosPlu_ShouldBuildMessageWithPosPluScanCodeType()
        {
            //Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].ScanCode = "123456";

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => int.Parse(m.ScanCode)));

            //When
            var actualXml = this.newItemMessageBuilder.BuildMessage(newItemModel);

            //Then
            var actualScanCodeType = XDocument.Parse(actualXml)
                .Descendants()
                .Where(e => e.Name.LocalName == "typeDescription")
                .First()
                .Value;
            Assert.AreEqual("POS PLU", actualScanCodeType);
        }

        [TestMethod]
        public void BuildMessage_GivenAScalePlu_ShouldBuildMessageWithScalePluType()
        {
            //Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].ScanCode = "27735300000";

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => 1));

            //When
            var actualXml = this.newItemMessageBuilder.BuildMessage(newItemModel);

            //Then
            var actualScanCodeType = XDocument.Parse(actualXml)
                .Descendants()
                .Where(e => e.Name.LocalName == "typeDescription")
                .First()
                .Value;
            Assert.AreEqual("Scale PLU", actualScanCodeType);
        }

        [TestMethod]
        public void BuildMessage_GivenA46IngredientPlu_ShouldBuildMessageWithScalePluType()
        {
            //Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].ScanCode = "46000000001";

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => 1));

            //When
            var actualXml = this.newItemMessageBuilder.BuildMessage(newItemModel);

            //Then
            var actualScanCodeType = XDocument.Parse(actualXml)
                .Descendants()
                .Where(e => e.Name.LocalName == "typeDescription")
                .First()
                .Value;
            Assert.AreEqual("Scale PLU", actualScanCodeType);
        }

        [TestMethod]
        public void BuildMessage_GivenA48IngredientPlu_ShouldBuildMessageWithScalePluType()
        {
            //Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].ScanCode = "48000000001";

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => 1));

            //When
            var actualXml = this.newItemMessageBuilder.BuildMessage(newItemModel);

            //Then
            var actualScanCodeType = XDocument.Parse(actualXml)
                .Descendants()
                .Where(e => e.Name.LocalName == "typeDescription")
                .First()
                .Value;
            Assert.AreEqual("Scale PLU", actualScanCodeType);
        }

        [TestMethod]
        public void BuildMessage_GivenABrandAbbreviationAndPosDescriptionWithMoreThan25CharactersCombined_ShouldTruncatePosDescriptionTo25Characters()
        {
            //Given
            List<NewItemModel> newItemModel = BuildNewItemModel(1);
            newItemModel[0].PosDescription = new string('a', 26);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(newItemModel.ToDictionary(
                    m => m.ScanCode,
                    m => int.Parse(m.ScanCode)));

            //When
            var actualXml = this.newItemMessageBuilder.BuildMessage(newItemModel);

            //Then
            var posDescription = XDocument.Parse(actualXml)
                .Descendants()
                .Single(e => e.Name.LocalName == "code" && e.Value == Traits.Codes.PosDescription)
                .Parent
                .Descendants(XName.Get("value", "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitValue/V2"))
                .Single()
                .Value;
            Assert.AreEqual(25, posDescription.Length);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeNewItemsWithOrganicSet_ShouldReturnItemXmlStringForThreeItemsWithOrganicTraits()
        {
            // Given
            settings.SendOrganic = true;

            List<NewItemModel> newItemModel = BuildNewItemModel(3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(new Dictionary<string, int>
                {
                    { "test1", 111 },
                    { "test2", 112 },
                    { "test3", 113 }
                });

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString(
                @"TestMessages\ThreeNewItemsWithOrganicSet.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeNewItemsWithOrganicSetToFalse_ShouldReturnItemXmlStringForThreeItemsWithOrganicTraitsSetToZero()
        {
            // Given
            settings.SendOrganic = true;

            List<NewItemModel> newItemModel = BuildNewItemModel(3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(new Dictionary<string, int>
                {
                    { "test1", 111 },
                    { "test2", 112 },
                    { "test3", 113 }
                });

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            var expectedXml = File.ReadAllText(@"TestMessages\ThreeNewItemsWithOrganicSetToFalse.xml");
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeNewItemsWithCFD_ShouldReturnItemXmlStringForThreeItemsWithCFDTraits()
        {
            // Given
            settings.IncludeCustomerFacingDescription = true;

            List<NewItemModel> newItemModel = BuildNewItemModel(3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(new Dictionary<string, int>
                {
                    { "test1", 111 },
                    { "test2", 112 },
                    { "test3", 113 }
                });

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString(
                @"TestMessages\ThreeNewItemsWithCustomerFacingDescriptionsSet.xml", actualXmlString);
        }

        [TestMethod]
        public void BuildMessage_GivenAListOfThreeNewItemsWithoutCFD_ShouldReturnItemXmlStringForThreeItemsWithoutCFD()
        {
            // Given
            settings.IncludeCustomerFacingDescription = false;

            List<NewItemModel> newItemModel = BuildNewItemModel(3);

            this.mockGetItemIdsQueryHandler.Setup(qh => qh.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(new Dictionary<string, int>
                {
                    { "test1", 111 },
                    { "test2", 112 },
                    { "test3", 113 }
                });

            // When
            var actualXmlString = this.newItemMessageBuilder.BuildMessage(newItemModel);

            // Then
            Helpers.XmlDeepEquals.AssertEquivalentFileToLoadAndXmlString(
                @"TestMessages\ThreeNewItemsWithCustomerFacingDescriptionsAbsent.xml", actualXmlString);
        }

        private List<NewItemModel> BuildNewItemModel(int numberOfItems)
        {
            List<NewItemModel> newItemModel = new List<NewItemModel>();
            for (int i = 0; i < numberOfItems; i++)
            {
                NewItemModel newItem = new NewItemModel
                {
                    Region = "FL",
                    ScanCode = "123456789" + i.ToString(),
                    IsDefaultIdentifier = true,
                    IsRetailSale = true,
                    IconBrandId = 5,
                    BrandName = "Test Brand",
                    ItemDescription = "Test Item Description " + i.ToString(),
                    PosDescription = "TestPOS " + i.ToString(),
                    PackageUnit = 1,
                    RetailSize = 1.1m,
                    RetailUom = "EA",
                    FoodStampEligible = true,
                    TaxClassCode = "1111111",
                    SubTeamName = "Test Sub Team",
                    SubTeamNumber = "1234",
                    NationalClassCode = "12345"
				};
                if (settings.SendOrganic)
                {
                    newItem.Organic = true;
                }
                if (settings.IncludeCustomerFacingDescription)
                {
                    newItem.CustomerFriendlyDescription = "Test Customer Friendly Description";
                }
                newItemModel.Add(newItem);
            }
            return newItemModel;
        }

        private List<NewItemModel> BuildNewItemBlankHierarchyClassesModel(int numberOfItems)
        {
            List<NewItemModel> newItemModel = new List<NewItemModel>();
            for (int i = 0; i < numberOfItems; i++)
            {
                NewItemModel newItem = new NewItemModel
                {
                    Region = "FL",
                    ScanCode = "123456789" + i.ToString(),
                    IsDefaultIdentifier = true,
                    IsRetailSale = true,
                    IconBrandId = 0,
                    BrandName = "New Brand",
                    ItemDescription = "Test Item Description " + i.ToString(),
                    PosDescription = "TestPOS " + i.ToString(),
                    PackageUnit = 1,
                    RetailSize = 1.1m,
                    RetailUom = "EA",
                    FoodStampEligible = true,
                    TaxClassCode = "9999999",
                    SubTeamName = "Test Sub Team",
                    SubTeamNumber = "1234",
                    NationalClassCode = "56789"
                };
                if (settings.SendOrganic)
                {
                    newItem.Organic = true;
                }
                if (settings.IncludeCustomerFacingDescription)
                {
                    newItem.CustomerFriendlyDescription = "Test Customer Friendly Description";
                }
                newItemModel.Add(newItem);
            }
            return newItemModel;
        }

    }
}
