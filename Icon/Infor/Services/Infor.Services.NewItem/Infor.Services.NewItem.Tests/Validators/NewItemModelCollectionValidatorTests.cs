using Icon.Common.DataAccess;
using Infor.Services.NewItem.Queries;
using Infor.Services.NewItem.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Cache;
using Infor.Services.NewItem.Constants;

namespace Infor.Services.NewItem.Tests.Validators
{
    [TestClass]
    public class NewItemModelCollectionValidatorTests
    {
        private const string testTaxClassCode = "0000000";
        private const int testIconBrandId = 1234;
        private const string testNationalClassCode = "12345";

        private NewItemModelCollectionValidator validator;
        private Mock<IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>> mockGetItemIdsQueryHandler;
        private Mock<IIconCache> mockCache;
        private Dictionary<string, int> itemIdDictionary;
        private List<NewItemModel> testItems;

        [TestInitialize]
        public void Initialize()
        {
            itemIdDictionary = new Dictionary<string, int>();
            mockGetItemIdsQueryHandler = new Mock<IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>>();
            mockGetItemIdsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemIdsQuery>())).Returns(itemIdDictionary);
            mockCache = new Mock<IIconCache>();

            validator = new NewItemModelCollectionValidator(mockGetItemIdsQueryHandler.Object, mockCache.Object);

            testItems = new List<NewItemModel>();
            mockCache.SetupGet(m => m.BrandDictionary)
                .Returns(new Dictionary<int, BrandModel> { { testIconBrandId, new BrandModel() } });
            mockCache.SetupGet(m => m.TaxDictionary)
                .Returns(new Dictionary<string, TaxClassModel> { { testTaxClassCode, new TaxClassModel() } });
            mockCache.SetupGet(m => m.NationalClassModels)
                .Returns(new Dictionary<string, NationalClassModel> { { testNationalClassCode, new NationalClassModel() } });
        }

        [TestMethod]
        public void Validate_NoPlus_ShouldHaveOnlyValidItems()
        {
            //Given
            testItems.Add(CreateTestItem("1234567", true));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(0, result.InvalidEntities.Count());
            Assert.AreEqual(1, result.ValidEntities.Count());
        }

        [TestMethod]
        public void Validate_AllPlus_ShouldHaveOnlyInvalidItems()
        {
            //Given
            testItems.Add(CreateTestItem("123456", true));
            testItems.Add(CreateTestItem("1", true));
            testItems.Add(CreateTestItem("20000000000", true));
            testItems.Add(CreateTestItem("22879000000", true));
            testItems.Add(CreateTestItem("27894000000", true));
            testItems.Add(CreateTestItem("21234500000", true));
            testItems.Add(CreateTestItem("46123456787"));
            testItems.Add(CreateTestItem("46000000000"));
            testItems.Add(CreateTestItem("48452879654"));
            testItems.Add(CreateTestItem("48000000000"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(testItems.Count(), result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
        }

        [TestMethod]
        public void Validate_PlusThatExistInIcon_ShouldHaveOnlyInvalidItems()
        {
            //Given
            testItems.Add(CreateTestItem("123456", true));
            testItems.Add(CreateTestItem("1", true));
            testItems.Add(CreateTestItem("20000000000", true));
            testItems.Add(CreateTestItem("22879000000", true));
            testItems.Add(CreateTestItem("27894000000", true));
            testItems.Add(CreateTestItem("21234500000", true));
            testItems.Add(CreateTestItem("46123456787"));
            testItems.Add(CreateTestItem("46000000000"));
            testItems.Add(CreateTestItem("48452879654"));
            testItems.Add(CreateTestItem("48000000000"));

            mockGetItemIdsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemIdsQuery>()))
                .Returns(testItems.ToDictionary(i => i.ScanCode, i => 0));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(0, result.InvalidEntities.Count());
            Assert.AreEqual(testItems.Count(), result.ValidEntities.Count());
        }

        [TestMethod]
        public void Validate_NonPlusAndPlusThatDontExistInIcon_ShouldHaveInvalidAndValidItems()
        {
            //Given
            testItems.Add(CreateTestItem("12345678", true));
            testItems.Add(CreateTestItem("123456789"));
            testItems.Add(CreateTestItem("123456", true));
            testItems.Add(CreateTestItem("1", true));
            testItems.Add(CreateTestItem("20000000000", true));
            testItems.Add(CreateTestItem("22879000000", true));
            testItems.Add(CreateTestItem("27894000000", true));
            testItems.Add(CreateTestItem("21234500000", true));
            testItems.Add(CreateTestItem("46123456787"));
            testItems.Add(CreateTestItem("46000000000"));
            testItems.Add(CreateTestItem("48452879654"));
            testItems.Add(CreateTestItem("48000000000"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(10, result.InvalidEntities.Count());
            Assert.AreEqual(2, result.ValidEntities.Count());
        }

        [TestMethod]
        public void Validate_BrandIsInvalid_InvalidBrandError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", iconBrandId: 1));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidBrand, item.ErrorCode);
            Assert.AreEqual(
                "The item's Brand '' does not exist in Infor. Please choose a different Brand which is managed by Infor and then refresh the item.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_TaxClassCodeIsInvalid_InvalidTaxClassCodeError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", taxClassCode: "1111111"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidTaxClassCode, item.ErrorCode);
            Assert.AreEqual(
                "The item's Tax Class has a Tax Class Code '1111111' which does not exist in Infor. Please choose a different Tax Class which is managed by Infor and then refresh the item.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_NationalClassCodeIsInvalid_InvalidNationalClassCodeError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", nationalClassCode: "987654321"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidNationalClassCode, item.ErrorCode);
            Assert.AreEqual(
                "The item's National Class has a National Class Code '987654321' which does not exist in Infor. Please choose a different National Class which is managed by Infor and then refresh the item.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_ProductDescriptionIsInvalid_InvalidProductDescriptionError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", itemDescription: "bad value +"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidProductDescription, item.ErrorCode);
            Assert.AreEqual(
                @"Product Description has invalid value 'bad value +'. Product Description is required and must be less than 60 characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @. Please remove the invalid characters and refresh the item.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_PosDescriptionIsInvalid_InvalidPosDescriptionError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", posDescription: "bad value +"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidPosDescription, item.ErrorCode);
            Assert.AreEqual(
                @"POS Description has invalid value 'bad value +'. POS Description is requried and must be less than 25 characters and can contain whitespaces, !, #, $, %, &, ', (, ), *, commas, -, ., /, :, ;, <, =, >, ?, or @. Please remove the invalid characters and refresh the item.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_RetailUomIsInvalid_InvalidRetailUomError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", retailUom: "Bad value"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidRetailUom, item.ErrorCode);
            Assert.AreEqual(
                @"Retail UOM has invalid value 'Bad value'. Retail UOM is required and must be one of the following: EA,LB,CT,OZ,CS,PK,LT,PT,KG,ML,GL,GR,CG,FT,YD,QT,SQFT,MT,FZ. Please remove the invalid UOM and refresh the item.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_CustomerFriendlyDescriptionIsInvalid_CustomerFriendlyDescriptionError()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", customerFriendlyDescription: "An Excessively Long Customer Friendly Description over 60 characters"));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(1, result.InvalidEntities.Count());
            Assert.AreEqual(0, result.ValidEntities.Count());
            var item = result.InvalidEntities.Single();
            Assert.AreEqual(ApplicationErrors.Codes.InvalidCustomerFriendlyDescription, item.ErrorCode);
            Assert.AreEqual(
                @"Customer Friendly Description has invalid value 'An Excessively Long Customer Friendly Description over 60 characters'. Maximum length is 60 characters.",
                item.ErrorDetails);
        }

        [TestMethod]
        public void Validate_CustomerFriendlyDescriptionIsValid_NullValueIsAcceptable()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", customerFriendlyDescription: null));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(0, result.InvalidEntities.Count());
            Assert.AreEqual(1, result.ValidEntities.Count());
            var item = result.ValidEntities.Single();
            Assert.IsNull(item.CustomerFriendlyDescription);
        }

        [TestMethod]
        public void Validate_CustomerFriendlyDescriptionIsValid_EmptyValueIsAcceptable()
        {
            //Given
            testItems.Add(CreateTestItem("123456789", customerFriendlyDescription: String.Empty));

            //When
            var result = validator.ValidateCollection(testItems);

            //Then
            Assert.AreEqual(0, result.InvalidEntities.Count());
            Assert.AreEqual(1, result.ValidEntities.Count());
            var item = result.ValidEntities.Single();
            Assert.AreEqual(String.Empty, item.CustomerFriendlyDescription);
        }

        private NewItemModel CreateTestItem(
            string scanCode, 
            bool isRetailSale = false, 
            string itemDescription = "ItemDescription",
            string posDescription = "POSDescription",
            string retailUom = "EA",
            string taxClassCode = testTaxClassCode,
            int iconBrandId = testIconBrandId,
            string nationalClassCode = testNationalClassCode,
            string customerFriendlyDescription = null)
        {
            return new NewItemModel
            {
                ScanCode = scanCode,
                IsRetailSale = isRetailSale,
                ItemDescription = itemDescription,
                PosDescription = posDescription,
                RetailUom = retailUom,
                TaxClassCode = taxClassCode,
                IconBrandId = iconBrandId,
                NationalClassCode = nationalClassCode,
                CustomerFriendlyDescription = customerFriendlyDescription
            };
        }
    }
}
