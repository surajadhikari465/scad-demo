using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Constants.ItemValidation;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Queries;
using Icon.Infor.Listeners.Item.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Infor.Listeners.Item.Tests.Validators
{
    [TestClass]
    public class ItemModelValidatorTests
    {
        private ItemModelValidator validator;
        private ItemListenerSettings settings;
        private Mock<IQueryHandler<GetItemValidationPropertiesParameters, List<GetItemValidationPropertiesResultModel>>> mockValidateItemsCommandHandler;
        private List<ItemModel> testItems;
        private ItemModel testItem;
        private GetItemValidationPropertiesResultModel testItemValidationPropertiesResultModel;

        [TestInitialize]
        public void Initialize()
        {
            testItem = new ItemModel
            {
                ItemId = 1,
                ItemTypeCode = ItemTypes.Codes.RetailSale,
                ProductDescription = "ProductDescription",
                AirChilled = string.Empty,
                AlcoholByVolume = string.Empty,
                AnimalWelfareRating = string.Empty,
                BrandsHierarchyClassId = "1",
                Biodynamic = string.Empty,
                CaseinFree = string.Empty,
                CheeseMilkType = string.Empty,
                CheeseRaw = string.Empty,
                ContainesDuplicateMerchandiseClass = false,
                ContainesDuplicateNationalClass = false,
                DeliverySystem = string.Empty,
                DrainedWeight = string.Empty,
                DrainedWeightUom = string.Empty,
                DryAged = string.Empty,
                EcoScaleRating = string.Empty,
                ErrorCode = null,
                ErrorDetails = null,
                FairTradeCertified = string.Empty,
                FinancialHierarchyClassId = "2000",
                FoodStampEligible = "1",
                FreeRange = string.Empty,
                FreshOrFrozen = string.Empty,
                GlutenFree = string.Empty,
                GrassFed = string.Empty,
                Hemp = string.Empty,
                HiddenItem = string.Empty,
                InsertDate = string.Empty,
                InforMessageId = Guid.NewGuid(),
                Kosher = string.Empty,
                LocalLoanProducer = string.Empty,
                MadeInHouse = string.Empty,
                MainProductName = string.Empty,
                MerchandiseHierarchyClassId = "1",
                ModifiedDate = "2017-04-08T12:04:22.287Z",
                ModifiedUser = string.Empty,
                Msc = string.Empty,
                NationalHierarchyClassId = "3",
                NonGmo = string.Empty,
                Notes = string.Empty,
                NutritionRequired = string.Empty,
                Organic = string.Empty,
                OrganicPersonalCare = string.Empty,
                PackageUnit = "1",
                Paleo = string.Empty,
                PastureRaised = string.Empty,
                PosDescription = "PosDescription",
                PosScaleTare = "1",
                PremiumBodyCare = string.Empty,
                ProductFlavorType = string.Empty,
                ProhibitDiscount = "1",
                RetailSize = "1",
                RetailUom = UomCodes.Each,
                ScanCode = "1234567890",
                ScanCodeType = ScanCodeTypes.Descriptions.Upc,
                SeafoodCatchType = string.Empty,
                SequenceId = 10,
                TaxHierarchyClassId = "0123456",
                Vegan = string.Empty,
                Vegetarian = string.Empty,
                WholeTrade = string.Empty,
                CustomerFriendlyDescription = "Test Customer Friendly Description"
            };

            testItems = new List<ItemModel>
            {
                testItem
            };

            testItemValidationPropertiesResultModel = new GetItemValidationPropertiesResultModel
            {
                BrandId = 1,
                ItemId = testItem.ItemId,
                ModifiedDate = DateTime.Parse(testItem.ModifiedDate).AddMinutes(-1).ToString(),
                NationalClassId = 1,
                SubBrickId = 1,
                SubTeamId = 1,
                TaxClassId = 1,
                SequenceId = 10
            };

            settings = new ItemListenerSettings { ValidateSequenceId = true };
            mockValidateItemsCommandHandler = new Mock<IQueryHandler<GetItemValidationPropertiesParameters, List<GetItemValidationPropertiesResultModel>>>();
            mockValidateItemsCommandHandler.Setup(m => m.Search(It.IsAny<GetItemValidationPropertiesParameters>()))
                .Returns(new List<GetItemValidationPropertiesResultModel>
                {
                    testItemValidationPropertiesResultModel
                });

            validator = new ItemModelValidator(settings, mockValidateItemsCommandHandler.Object);
        }

        [TestMethod]
        public void ValidateCollection_AllPropertiesAreValid_ItemShouldBeValid()
        {
            //When
            validator.ValidateCollection(testItems);

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AirChilledIsValid_NoError()
        {
            // Given
            testItem.AirChilled = "1";
            testItems.Add(CopyTestItem(i => i.AirChilled = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AirChilledIsInvalid_NoError()
        {
            // Given
            testItem.AirChilled = " ";
            testItems.Add(CopyTestItem(i => i.AirChilled = "asdf"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidAirChilled,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.AirChilled),
                (i) => i.AirChilled);
        }

        [TestMethod]
        public void ValidateCollection_AirChilledIsEmpty_NoError()
        {
            // Given
            testItem.AirChilled = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AlcoholByVolumeIsValid_NoError()
        {
            // Given
            testItem.AlcoholByVolume = "0";
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "99.99"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "10"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "99.0"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "99.01"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AlcoholByVolumeIsInvalid_ItemInvalidAlcoholByVolumeError()
        {
            // Given
            testItem.AlcoholByVolume = "120.23";
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "100.00"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "99.999"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "99."));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "0.789"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = " "));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "-1"));
            testItems.Add(CopyTestItem(i => i.AlcoholByVolume = "abc"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidAlcoholByVolume,
                ValidationErrorMessages.InvalidAlcoholByVolume,
                nameof(testItem.AlcoholByVolume),
                (i) => i.AlcoholByVolume);
        }

        [TestMethod]
        public void ValidateCollection_AlcoholByVolumeIsEmpty_NoError()
        {
            // Given
            testItem.AlcoholByVolume = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AnimalWelfareRatingIsValid_NoError()
        {
            //Given
            foreach (var rating in AnimalWelfareRatings.Descriptions.AsArray)
            {
                testItems.Add(CopyTestItem((i) => i.AnimalWelfareRating = rating));
            }
            testItem.AnimalWelfareRating = AnimalWelfareRatings.Descriptions.NoStep;

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AnimalWelfareRatingAStringUpTo50Characters_NoError()
        {
            //Given
            testItem.AnimalWelfareRating = new string('a', 50);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
            //AssertExpectedErrorsWithExpectedMessages(
            //    ValidationErrorCodes.InvalidAnimalWelfareRating,
            //    ValidationErrorMessages.InvalidAnimalWelfareRating,
            //    nameof(testItem.AnimalWelfareRating),
            //    (i) => i.AnimalWelfareRating);
        }

        [TestMethod]
        public void ValidateCollection_AnimalWelfareRatingIsEmpty_NoError()
        {
            //Given
            testItem.AnimalWelfareRating = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AnimalWelfareRatingIsNull_NoError()
        {
            //Given
            testItem.AnimalWelfareRating = null;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_BiodynamicIsValid_NoError()
        {
            //Given
            testItem.Biodynamic = "1";
            testItems.Add(CopyTestItem((i) => i.Biodynamic = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_BiodynamicIsInvalid_InvalidBiodynamicError()
        {
            //Given
            testItem.Biodynamic = "Invalid value";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.InvalidBiodynamic,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne.GetFormattedValidationMessage(nameof(testItem.Biodynamic), testItem.Biodynamic));
        }

        [TestMethod]
        public void ValidateCollection_BiodynamicIsEmpty_NoError()
        {
            //Given
            testItem.Biodynamic = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                null,
                null);
        }

        [TestMethod]
        public void ValidateCollection_BrandIsValid_NoError()
        {
            //Given
            testItem.BrandsHierarchyClassId = "12345";
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = int.MaxValue.ToString()));
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "3281291030"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_BrandIsInvalid_InvalidBrandError()
        {
            //Given
            testItem.BrandsHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "0"));
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "01234"));
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "-12340"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidBrand,
                ValidationErrorMessages.InvalidBrand, 
                nameof(testItem.BrandsHierarchyClassId),
                (i) => i.BrandsHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_BrandIsEmpty_InvalidBrandError()
        {
            //Given
            testItem.BrandsHierarchyClassId = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.InvalidBrand,
                ValidationErrorMessages.InvalidBrand.GetFormattedValidationMessage(nameof(testItem.BrandsHierarchyClassId), testItem.BrandsHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_CaseinFreeIsValid_NoError()
        {
            //Given
            testItem.CaseinFree = "1";
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CaseinFreeIsInvalid_InvalidCaseinFreeError()
        {
            //Given
            testItem.CaseinFree = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "00"));
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "11"));
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "320912-9%"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidCaseinFree,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.CaseinFree),
                (i) => i.CaseinFree);
        }

        [TestMethod]
        public void ValidateCollection_CaseinFreeIsEmpty_NoError()
        {
            //Given
            testItem.CaseinFree = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseMilkTypeIsValid_NoError()
        {
            //Given
            foreach (var milkType in MilkTypes.Descriptions.AsArray)
            {
                testItems.Add(CopyTestItem(i => i.CheeseMilkType = milkType));
            }
            testItem.CheeseMilkType = MilkTypes.Descriptions.AsArray[0];

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseMilkTypeIsInvalid_InvalidCheeseMilkTypeError()
        {
            //Given
            testItem.CheeseMilkType = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.CheeseMilkType = "789456123"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidCheeseMilkType,
                ValidationErrorMessages.InvalidCheeseMilkType,
                nameof(testItem.CheeseMilkType),
                (i) => i.CheeseMilkType);
        }

        [TestMethod]
        public void ValidateCollection_CheeseMilkTypeIsEmpty_NoError()
        {
            //Given
            testItem.CheeseMilkType = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseRawIsValid_NoError()
        {
            //Given
            testItem.CheeseRaw = "0";
            testItems.Add(CopyTestItem((i) => i.CheeseRaw = "1"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseRawIsInvalid_InvalidCheeseRawError()
        {
            //Given
            testItem.CheeseRaw = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.CheeseRaw = "789456123"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidCheeseRaw,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.CheeseRaw),
                (i) => i.CheeseRaw);
        }

        [TestMethod]
        public void ValidateCollection_CheeseRawIsEmpty_NoError()
        {
            //Given
            testItem.CheeseRaw = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateMerchandiseHierarchyClassIsFalse_NoError()
        {
            //Given
            testItem.ContainesDuplicateMerchandiseClass = false;

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateMerchandiseHierarchyClassIsTrue_NoError()
        {
            //Given
            testItem.ContainesDuplicateMerchandiseClass = true;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.DuplicateMerchandiseHierarchyClass,
                ValidationErrorMessages.DuplicateMerchandiseHierarchyClass);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateNationalHierarchyClassIsFalse_NoError()
        {
            //Given
            testItem.ContainesDuplicateNationalClass = false;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateNationalHierarchyClassIsTrue_NoError()
        {
            //Given
            testItem.ContainesDuplicateNationalClass = true;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.DuplicateNationalHierarchyClass, 
                ValidationErrorMessages.DuplicateNationalHierarchyClass);
        }

        [TestMethod]
        public void ValidateCollection_DeliverySystemIsValid_NoError()
        {
            //Given
            foreach (var description in DeliverySystems.Descriptions.AsArray)
            {
                testItems.Add(CopyTestItem(i => i.DeliverySystem = description));
            }
            testItem.DeliverySystem = DeliverySystems.Descriptions.AsArray[0];

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DeliverySystemIsInvalid_InvalidDeliverySystemError()
        {
            //Given
            testItem.DeliverySystem = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.DeliverySystem = "789456123"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidDeliverySystem,
                ValidationErrorMessages.InvalidDeliverySystem,
                nameof(testItem.DeliverySystem),
                (i) => i.DeliverySystem);
        }

        [TestMethod]
        public void ValidateCollection_DeliverySystemIsEmpty_NoError()
        {
            //Given
            testItem.DeliverySystem = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightIsValid_NoError()
        {
            //Given
            testItem.DrainedWeight = "124";
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "0"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "0.1234"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "123456789.1234"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightIsInvalid_InvalidDrainedWeightError()
        {
            //Given
            testItem.DrainedWeight = "Invalid value";
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "789456123.12345"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "789456123A.1234"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "789456123.aaaa"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "asdf.789"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "."));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "798.445."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidDrainedWeight,
                ValidationErrorMessages.InvalidDrainedWeight,
                nameof(testItem.DrainedWeight),
                (i) => i.DrainedWeight);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightIsEmpty_NoError()
        {
            //Given
            testItem.DrainedWeight = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightUomIsValid_NoError()
        {
            //Given
            testItem.DrainedWeightUom = DrainedWeightUoms.Ml;
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = DrainedWeightUoms.Oz));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightUomIsInvalid_InvalidDrainedWeightUomError()
        {
            //Given
            testItem.DrainedWeightUom = "Invalid value";
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = " "));
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = "789"));
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = "asdf"));
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = "A"));
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidDrainedWeightUom,
                ValidationErrorMessages.InvalidDrainedWeightUom,
                nameof(testItem.DrainedWeightUom),
                (i) => i.DrainedWeightUom);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightUomIsEmpty_NoError()
        {
            //Given
            testItem.DrainedWeightUom = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DryAgedIsValid_NoError()
        {
            //Given
            testItem.DryAged = "0";
            testItems.Add(CopyTestItem(i => i.DryAged = "1"));

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DryAgedIsInvalid_InvalidDryAgedError()
        {
            //Given
            testItem.DryAged = "Invalid value";
            testItems.Add(CopyTestItem(i => i.DryAged = " "));
            testItems.Add(CopyTestItem(i => i.DryAged = "789"));
            testItems.Add(CopyTestItem(i => i.DryAged = "asdf"));
            testItems.Add(CopyTestItem(i => i.DryAged = "A"));
            testItems.Add(CopyTestItem(i => i.DryAged = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidDryAged,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.DryAged),
                (i) => i.DryAged);
        }

        [TestMethod]
        public void ValidateCollection_DryAgedIsEmpty_NoError()
        {
            //Given
            testItem.DryAged = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_EcoScaleRatingIsValid_NoError()
        {
            //Given
            foreach (var description in EcoScaleRatings.Descriptions.AsArray)
            {
                testItems.Add(CopyTestItem(i => i.EcoScaleRating = description));
            }
            testItem.EcoScaleRating = EcoScaleRatings.Descriptions.AsArray[0];

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_EcoScaleRatingIsInvalid_InvalidEcoScaleRatingError()
        {
            //Given
            testItem.EcoScaleRating = "Invalid value";
            testItems.Add(CopyTestItem(i => i.EcoScaleRating = " "));
            testItems.Add(CopyTestItem(i => i.EcoScaleRating = "789"));
            testItems.Add(CopyTestItem(i => i.EcoScaleRating = "asdf"));
            testItems.Add(CopyTestItem(i => i.EcoScaleRating = "A"));
            testItems.Add(CopyTestItem(i => i.EcoScaleRating = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidEcoScaleRating,
                ValidationErrorMessages.InvalidEcoScaleRating,
                nameof(testItem.EcoScaleRating),
                (i) => i.EcoScaleRating);
        }

        [TestMethod]
        public void ValidateCollection_EcoScaleRatingIsEmpty_NoError()
        {
            //Given
            testItem.EcoScaleRating = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FairTradeCertifiedIsValid_NoError()
        {
            //Given
            testItem.FairTradeCertified = "Fair Trade USA";
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "Fair Trade International"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "IMO USA"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "Rainforest Alliance"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "Whole Foods Market"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = " some other thing !@#$%"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FairTradeCertified_DoesNotRestrictValuesToOldEnumeration()
        {
            //Given
            // this used to be restricted to "Fair Trade USA|Fair Trade International|IMO USA|Rainforest Alliance|Whole Foods Market"
            testItem.FairTradeCertified = "Invalid value";
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = " "));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "789"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "asdf"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "A"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FairTradeCertifiedIsEmpty_NoError()
        {
            //Given
            testItem.FairTradeCertified = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FinancialHierarchyClassIdIsValid_NoError()
        {
            //Given
            testItem.FinancialHierarchyClassId = "0123";
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "0000"));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "1000"));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "6200"));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "3700"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FinancialHierarchyClassIdIsInvalid_InvalidFinancialHierarchyClassIdError()
        {
            //Given
            testItem.FinancialHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = " "));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "789"));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "asdf"));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "A"));
            testItems.Add(CopyTestItem(i => i.FinancialHierarchyClassId = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidFinancialHierarchyClassId,
                ValidationErrorMessages.InvalidFinancialHierarchyClassId,
                nameof(testItem.FinancialHierarchyClassId),
                (i) => i.FinancialHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_FinancialHierarchyClassIdIsEmpty_InvalidFinancialHierarchyClassIdError()
        {
            //Given
            testItem.FinancialHierarchyClassId = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.InvalidFinancialHierarchyClassId,
                ValidationErrorMessages.InvalidFinancialHierarchyClassId.GetFormattedValidationMessage(nameof(testItem.FinancialHierarchyClassId), testItem.FinancialHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_FoodStampEligibleIsValid_NoError()
        {
            //Given
            testItem.FoodStampEligible = "0";
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = "1"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FoodStampEligibleIsInvalid_InvalidFoodStampEligibleError()
        {
            //Given
            testItem.FoodStampEligible = "Invalid value";
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = " "));
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = "789"));
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = "asdf"));
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = "A"));
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidFoodStampEligible,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.FoodStampEligible),
                (i) => i.FoodStampEligible);
        }

        [TestMethod]
        public void ValidateCollection_FoodStampEligibleIsEmpty_InvalidFoodStampEligibleError()
        {
            //Given
            testItem.FoodStampEligible = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.InvalidFoodStampEligible,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne.GetFormattedValidationMessage(nameof(testItem.FoodStampEligible), testItem.FoodStampEligible));
        }

        [TestMethod]
        public void ValidateCollection_FreeRangeIsValid_NoError()
        {
            //Given
            testItem.FreeRange = "0";
            testItems.Add(CopyTestItem(i => i.FreeRange = "1"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FreeRangeIsInvalid_InvalidFreeRangeError()
        {
            //Given
            testItem.FreeRange = "Invalid value";
            testItems.Add(CopyTestItem(i => i.FreeRange = " "));
            testItems.Add(CopyTestItem(i => i.FreeRange = "789"));
            testItems.Add(CopyTestItem(i => i.FreeRange = "asdf"));
            testItems.Add(CopyTestItem(i => i.FreeRange = "A"));
            testItems.Add(CopyTestItem(i => i.FreeRange = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidFreeRange,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.FreeRange),
                (i) => i.FreeRange);
        }

        [TestMethod]
        public void ValidateCollection_FreeRangeIsEmpty_NoError()
        {
            //Given
            testItem.FreeRange = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FreshOrFrozenIsValid_NoError()
        {
            //Given
            foreach (var description in SeafoodFreshOrFrozenTypes.Descriptions.AsArray)
            {
                testItems.Add(CopyTestItem(i => i.FreshOrFrozen = description));
            }
            testItem.FreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.AsArray[0];

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FreshOrFrozenIsInvalid_InvalidFreshOrFrozenError()
        {
            //Given
            testItem.FreshOrFrozen = "Invalid value";
            testItems.Add(CopyTestItem(i => i.FreshOrFrozen = " "));
            testItems.Add(CopyTestItem(i => i.FreshOrFrozen = "789"));
            testItems.Add(CopyTestItem(i => i.FreshOrFrozen = "asdf"));
            testItems.Add(CopyTestItem(i => i.FreshOrFrozen = "A"));
            testItems.Add(CopyTestItem(i => i.FreshOrFrozen = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidFreshOrFrozen,
                ValidationErrorMessages.InvalidFreshOrFrozen,
                nameof(testItem.FreshOrFrozen),
                (i) => i.FreshOrFrozen);
        }

        [TestMethod]
        public void ValidateCollection_FreshOrFrozenIsEmpty_NoError()
        {
            //Given
            testItem.FreshOrFrozen = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlutenFreeIsValid_NoError()
        {
            //Given
            testItem.GlutenFree = "Valid value";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlutenFreeIsInvalid_GlutenFreeError()
        {
            //Given
            testItem.GlutenFree = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidGlutenFree,
                ValidationErrorMessages.InvalidCertificationAgency,
                nameof(testItem.GlutenFree),
                (i) => i.GlutenFree);
        }

        [TestMethod]
        public void ValidateCollection_GlutenFreeIsEmpty_NoError()
        {
            //Given
            testItem.GlutenFree = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GrassFedIsValid_NoError()
        {
            //Given
            testItem.GrassFed = "0";
            testItems.Add(CopyTestItem(i => i.GrassFed = "1"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GrassFedIsInvalid_GrassFedError()
        {
            //Given
            testItem.GrassFed = "Invalid value";
            testItems.Add(CopyTestItem(i => i.GrassFed = "asdf"));
            testItems.Add(CopyTestItem(i => i.GrassFed = "1234"));
            testItems.Add(CopyTestItem(i => i.GrassFed = "."));
            testItems.Add(CopyTestItem(i => i.GrassFed = " "));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidGrassFed,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.GrassFed),
                (i) => i.GrassFed);
        }

        [TestMethod]
        public void ValidateCollection_GrassFedIsEmpty_NoError()
        {
            //Given
            testItem.GrassFed = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_HempIsValid_NoError()
        {
            //Given
            testItem.Hemp = "0";
            testItems.Add(CopyTestItem(i => i.Hemp = "1"));

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_HempIsInvalid_HempError()
        {
            //Given
            testItem.Hemp = "Invalid value";
            testItems.Add(CopyTestItem(i => i.Hemp = "asdf"));
            testItems.Add(CopyTestItem(i => i.Hemp = "1234"));
            testItems.Add(CopyTestItem(i => i.Hemp = "."));
            testItems.Add(CopyTestItem(i => i.Hemp = " "));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidHemp,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.Hemp),
                (i) => i.Hemp);
        }

        [TestMethod]
        public void ValidateCollection_HempIsEmpty_NoError()
        {
            //Given
            testItem.Hemp = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_HiddenItemIsValid_NoError()
        {
            //Given
            testItem.HiddenItem = "0";
            testItems.Add(CopyTestItem(i => i.HiddenItem = "1"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_HiddenItemIsInvalid_HiddenItemError()
        {
            //Given
            testItem.HiddenItem = "Invalid value";
            testItems.Add(CopyTestItem(i => i.HiddenItem = "asdf"));
            testItems.Add(CopyTestItem(i => i.HiddenItem = "1234"));
            testItems.Add(CopyTestItem(i => i.HiddenItem = "."));
            testItems.Add(CopyTestItem(i => i.HiddenItem = " "));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidHiddenItem,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.HiddenItem),
                (i) => i.HiddenItem);
        }

        [TestMethod]
        public void ValidateCollection_HiddenItemIsEmpty_NoError()
        {
            //Given
            testItem.HiddenItem = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_InforMessageIdIsValid_NoError()
        {
            //Given
            testItem.InforMessageId = Guid.NewGuid();

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_InforMessageIdIsInvalid_InforMessageIdError()
        {
            //Given
            testItem.InforMessageId = Guid.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidInforMessageId,
                ValidationErrorMessages.InvalidInforMessageId,
                nameof(testItem.InforMessageId),
                (i) => i.InforMessageId.ToString());
        }

        [TestMethod]
        public void ValidateCollection_InforMessageIdIsEmpty_InvalidInforMessageIdNoError()
        {
            //Given
            testItem.InforMessageId = Guid.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidInforMessageId,
                ValidationErrorMessages.InvalidInforMessageId,
                nameof(testItem.InforMessageId),
                (i) => i.InforMessageId.ToString());
        }

        [TestMethod]
        public void ValidateCollection_ItemIdIsValid_NoError()
        {
            //Given
            testItem.ItemId = 0;
            testItems.Add(CopyTestItem(i => i.ItemId = 1234));
            testItems.Add(CopyTestItem(i => i.ItemId = int.MaxValue));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ItemIdIsInvalid_InvalidItemIdError()
        {
            //Given
            testItem.ItemId = -1;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidItemId,
                ValidationErrorMessages.InvalidItemId,
                nameof(testItem.ItemId),
                (i) => i.ItemId.ToString());
        }

        [TestMethod]
        public void ValidateCollection_ItemTypeIdIsValid__NoError()
        {
            //Given
            foreach (var codes in ItemTypes.Ids.Keys)
            {
                testItems.Add(CopyTestItem(i => i.ItemTypeCode = codes));
            }
            testItem.ItemTypeCode = ItemTypes.Codes.RetailSale;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ItemTypeIdIsInvalid__InvalidItemTypeIdError()
        {
            // Given
            testItem.ItemTypeCode = 0.ToString();
            testItems.Add(CopyTestItem(i => i.ItemTypeCode = 123.ToString()));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidItemTypeCode,
                ValidationErrorMessages.InvalidItemTypeCode,
                nameof(testItem.ItemTypeCode),
                (i) => i.ItemTypeCode.ToString());
        }

        [TestMethod]
        public void ValidateCollection_KosherIsValid_NoError()
        {
            //Given
            testItem.Kosher = "Valid value";
            testItems.Add(CopyTestItem(i => i.Kosher = new string('a', 255)));

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_KosherIsInvalid_KosherError()
        {
            //Given
            testItem.Kosher = new string('a', 256);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidKosher,
                ValidationErrorMessages.InvalidCertificationAgency,
                nameof(testItem.Kosher),
                (i) => i.Kosher.ToString());
        }

        [TestMethod]
        public void ValidateCollection_KosherIsEmpty_NoError()
        {
            //Given
            testItem.Kosher = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_LocalLoanProducerIsValid_NoError()
        {
            //Given
            testItem.LocalLoanProducer = "1";
            testItems.Add(CopyTestItem(i => i.LocalLoanProducer = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_LocalLoanProducerIsInvalid_InvalidLocalLoanProducerError()
        {
            //Given
            testItem.LocalLoanProducer = "Invalid value";
            testItems.Add(CopyTestItem(i => i.LocalLoanProducer = "asdf"));
            testItems.Add(CopyTestItem(i => i.LocalLoanProducer = "1234"));
            testItems.Add(CopyTestItem(i => i.LocalLoanProducer = " "));
            testItems.Add(CopyTestItem(i => i.LocalLoanProducer = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidLocalLoanProducer,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.LocalLoanProducer),
                (i) => i.LocalLoanProducer);
        }

        [TestMethod]
        public void ValidateCollection_LocalLoanProducerIsEmpty_NoError()
        {
            //Given
            testItem.LocalLoanProducer = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MadeInHouseIsValid_NoError()
        {
            //Given
            testItem.MadeInHouse = "1";
            testItems.Add(CopyTestItem(i => i.MadeInHouse = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MadeInHouseIsInvalid_InvalidMadeInHouseError()
        {
            //Given
            testItem.MadeInHouse = "Invalid value";
            testItems.Add(CopyTestItem(i => i.MadeInHouse = "asdf"));
            testItems.Add(CopyTestItem(i => i.MadeInHouse = "1234"));
            testItems.Add(CopyTestItem(i => i.MadeInHouse = " "));
            testItems.Add(CopyTestItem(i => i.MadeInHouse = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidMadeInHouse,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.MadeInHouse),
                (i) => i.MadeInHouse);
        }

        [TestMethod]
        public void ValidateCollection_MadeInHouseIsEmpty_NoError()
        {
            //Given
            testItem.MadeInHouse = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MainProductNameIsValid_NoError()
        {
            //Given
            testItem.MainProductName = "Valid value";
            testItems.Add(CopyTestItem(i => i.MainProductName = "asdf"));
            testItems.Add(CopyTestItem(i => i.MainProductName = "1234"));
            testItems.Add(CopyTestItem(i => i.MainProductName = "dssdf.d1123032!@"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MainProductNameIsInvalid_InvalidMadeInHouseError()
        {
            //Given
            testItem.MainProductName = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidMainProductName,
                ValidationErrorMessages.InvalidMainProductName,
                nameof(testItem.MainProductName),
                (i) => i.MainProductName);
        }

        [TestMethod]
        public void ValidateCollection_MainProductNameIsEmpty_NoError()
        {
            //Given
            testItem.MainProductName = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MerchandiseHierarchyClassIdIsValid_NoError()
        {
            //Given
            testItem.MerchandiseHierarchyClassId = "1234567890";
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = "1234"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MerchandiseHierarchyClassIdIsInvalid_InvalidMerchandiseHierarchyClassIdError()
        {
            //Given
            testItem.MerchandiseHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = "1234a"));
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = " "));
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidMerchandiseHierarchyClassId,
                ValidationErrorMessages.InvalidMerchandiseHierarchyClassId,
                nameof(testItem.MerchandiseHierarchyClassId),
                (i) => i.MerchandiseHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_MerchandiseHierarchyClassIdIsEmpty_InvalidMerchandiseHierarchyClassIdError()
        {
            //Given
            testItem.MerchandiseHierarchyClassId = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.InvalidMerchandiseHierarchyClassId,
                ValidationErrorMessages.InvalidMerchandiseHierarchyClassId.GetFormattedValidationMessage(nameof(testItem.MerchandiseHierarchyClassId), testItem.MerchandiseHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_MscIsValid_NoError()
        {
            //Given
            testItem.Msc = "1";
            testItems.Add(CopyTestItem(i => i.Msc = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MscIsInvalid_InvalidMscIdError()
        {
            //Given
            testItem.Msc = "Invalid value";
            testItems.Add(CopyTestItem(i => i.Msc = "1234a"));
            testItems.Add(CopyTestItem(i => i.Msc = " "));
            testItems.Add(CopyTestItem(i => i.Msc = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidMsc,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.Msc),
                (i) => i.Msc);
        }

        [TestMethod]
        public void ValidateCollection_MscIsEmpty_NoError()
        {
            //Given
            testItem.Msc = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NationalHierarchyClassIdIsValid_NoError()
        {
            //Given
            testItem.NationalHierarchyClassId = "1234567890"; ;
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = "123456"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NationalHierarchyClassIdIsInvalid_InvalidNationalHierarchyClassIdIdError()
        {
            //Given
            testItem.NationalHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = "1234a"));
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = " "));
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = "."));
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = "0"));
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = "01234"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidNationalHierarchyClassId,
                ValidationErrorMessages.InvalidNationalHierarchyClassId,
                nameof(testItem.NationalHierarchyClassId),
                (i) => i.NationalHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NationalHierarchyClassIdIsEmpty_NoError()
        {
            //Given
            testItem.NationalHierarchyClassId = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidNationalHierarchyClassId,
                ValidationErrorMessages.InvalidNationalHierarchyClassId,
                nameof(testItem.NationalHierarchyClassId),
                (i) => i.NationalHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NonGmoIsValid_NoError()
        {
            //Given
            testItem.NonGmo = "Valid value"; ;
            testItems.Add(CopyTestItem(i => i.NonGmo = new string('a', MaxLengths.StandardProperty255)));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NonGmoIsInvalid_InvalidNonGmoError()
        {
            //Given
            testItem.NonGmo = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidNonGmo,
                ValidationErrorMessages.InvalidCertificationAgency,
                nameof(testItem.NonGmo),
                (i) => i.NonGmo);
        }

        [TestMethod]
        public void ValidateCollection_NonGmoIsEmpty_NoError()
        {
            //Given
            testItem.NonGmo = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NotesIsValid_NoError()
        {
            //Given
            testItem.Notes = "Valid value"; ;
            testItems.Add(CopyTestItem(i => i.Notes = new string('a', MaxLengths.StandardProperty255)));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NotesIsInvalid_InvalidNotesError()
        {
            //Given
            testItem.Notes = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidNotes,
                ValidationErrorMessages.InvalidNotes,
                nameof(testItem.Notes),
                (i) => i.Notes);
        }

        [TestMethod]
        public void ValidateCollection_NotesIsEmpty_NoError()
        {
            //Given
            testItem.Notes = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NutritionRequiredIsValid_NoError()
        {
            //Given
            testItem.NutritionRequired = "1";
            testItems.Add(CopyTestItem(i => i.NutritionRequired = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NutritionRequiredIsInvalid_InvalidNutritionRequiredError()
        {
            //Given
            testItem.NutritionRequired = "Invalid value";
            testItems.Add(CopyTestItem(i => i.NutritionRequired = "asdf"));
            testItems.Add(CopyTestItem(i => i.NutritionRequired = "1234"));
            testItems.Add(CopyTestItem(i => i.NutritionRequired = " "));
            testItems.Add(CopyTestItem(i => i.NutritionRequired = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidNutritionRequired,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.NutritionRequired),
                (i) => i.NutritionRequired);
        }

        [TestMethod]
        public void ValidateCollection_NutritionRequiredIsEmpty_NoError()
        {
            //Given
            testItem.NutritionRequired = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CustomerFriendlyDescriptionIsValid_NoError()
        {
            //Given
            testItem.CustomerFriendlyDescription = "!@#$%^&*()_+=-[]{}\\|':'aeionvcxv;198118341~`";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CustomerFriendlyDescriptionIsEmpty_NoError()
        {
            //Given
            testItem.CustomerFriendlyDescription = string.Empty;

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CustomerFriendlyDescriptionIsInvalid_InvalidCustomerFriendlyDescriptionError()
        {
            //Given
            testItem.CustomerFriendlyDescription = "123456789123456789123456789123456789123456789123456789123456789";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidCustomerFriendlyDescription,
                ValidationErrorMessages.InvalidCustomerFriendlyDescription,
                nameof(testItem.CustomerFriendlyDescription),
                (i) => i.CustomerFriendlyDescription);
        }

        [TestMethod]
        public void ValidateCollection_OrganicIsValid_NoError()
        {
            //Given
            testItem.Organic = "Valid value"; ;
            testItems.Add(CopyTestItem(i => i.Organic = new string('a', MaxLengths.StandardProperty255)));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_OrganicIsInvalid_InvalidOrganicError()
        {
            //Given
            testItem.Organic = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidOrganic,
                ValidationErrorMessages.InvalidCertificationAgency,
                nameof(testItem.Organic),
                (i) => i.Organic);
        }

        [TestMethod]
        public void ValidateCollection_OrganicIsEmpty_NoError()
        {
            //Given
            testItem.Organic = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_OrganicPersonalCareIsValid_NoError()
        {
            //Given
            testItem.OrganicPersonalCare = "1";
            testItems.Add(CopyTestItem(i => i.OrganicPersonalCare = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_OrganicPersonalCareIsInvalid_InvalidOrganicPersonalCareError()
        {
            //Given
            testItem.OrganicPersonalCare = "Invalid value";
            testItems.Add(CopyTestItem(i => i.OrganicPersonalCare = "asdf"));
            testItems.Add(CopyTestItem(i => i.OrganicPersonalCare = "1234"));
            testItems.Add(CopyTestItem(i => i.OrganicPersonalCare = " "));
            testItems.Add(CopyTestItem(i => i.OrganicPersonalCare = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidOrganicPersonalCare,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.OrganicPersonalCare),
                (i) => i.OrganicPersonalCare);
        }

        [TestMethod]
        public void ValidateCollection_OrganicPersonalCareIsEmpty_NoError()
        {
            //Given
            testItem.OrganicPersonalCare = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PackageUnitIsValid_NoError()
        {
            //Given
            testItem.PackageUnit = "1";
            testItems.Add(CopyTestItem(i => i.PackageUnit = "999"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "99"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "9"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "900"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "123"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "180"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PackageUnitIsInvalid_InvalidPackageUnitError()
        {
            //Given
            testItem.PackageUnit = "Invalid value";
            testItems.Add(CopyTestItem(i => i.PackageUnit = "asdf"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "1234a"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = " "));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "0"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "0"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "04"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "000"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "001"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPackageUnit,
                ValidationErrorMessages.InvalidPackageUnit,
                nameof(testItem.PackageUnit),
                (i) => i.PackageUnit);
        }

        [TestMethod]
        public void ValidateCollection_PackageUnitIsEmpty_InvalidPackageUnitError()
        {
            //Given
            testItem.PackageUnit = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPackageUnit,
                ValidationErrorMessages.InvalidPackageUnit,
                nameof(testItem.PackageUnit),
                (i) => i.PackageUnit);
        }

        [TestMethod]
        public void ValidateCollection_PaleoIsValid_NoError()
        {
            //Given
            testItem.Paleo = "1";
            testItems.Add(CopyTestItem(i => i.Paleo = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PaleoIsInvalid_InvalidLocalPaleoError()
        {
            //Given
            testItem.Paleo = "Invalid value";
            testItems.Add(CopyTestItem(i => i.Paleo = "asdf"));
            testItems.Add(CopyTestItem(i => i.Paleo = "1234"));
            testItems.Add(CopyTestItem(i => i.Paleo = " "));
            testItems.Add(CopyTestItem(i => i.Paleo = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPaleo,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.Paleo),
                (i) => i.Paleo);
        }

        [TestMethod]
        public void ValidateCollection_PaleoIsEmpty_NoError()
        {
            //Given
            testItem.Paleo = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PastureRaisedIsValid_NoError()
        {
            //Given
            testItem.PastureRaised = "1";
            testItems.Add(CopyTestItem(i => i.PastureRaised = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PastureRaisedIsInvalid_InvalidPastureRaisedError()
        {
            //Given
            testItem.PastureRaised = "Invalid value";
            testItems.Add(CopyTestItem(i => i.PastureRaised = "asdf"));
            testItems.Add(CopyTestItem(i => i.PastureRaised = "1234"));
            testItems.Add(CopyTestItem(i => i.PastureRaised = " "));
            testItems.Add(CopyTestItem(i => i.PastureRaised = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPastureRaised,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.PastureRaised),
                (i) => i.PastureRaised);
        }

        [TestMethod]
        public void ValidateCollection_PastureRaisedIsEmpty_NoError()
        {
            //Given
            testItem.PastureRaised = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PosDescriptionIsValid_NoError()
        {
            //Given
            testItem.PosDescription = "Valid value";
            testItems.Add(CopyTestItem(i => i.PosDescription = new string('a', MaxLengths.PosDescription)));
            testItems.Add(CopyTestItem(i => i.PosDescription = "abc123%!@"));

            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PosDescriptionIsInvalid_InvalidPosDescriptionError()
        {
            //Given
            testItem.PosDescription = new string('a', MaxLengths.PosDescription + 1);
            testItems.Add(CopyTestItem(i => i.PosDescription = "+"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPosDescription,
                ValidationErrorMessages.InvalidPosDescription,
                nameof(testItem.PosDescription),
                (i) => i.PosDescription);
        }

        [TestMethod]
        public void ValidateCollection_PosDescriptionIsEmpty_InvalidPosDescriptionError()
        {
            //Given
            testItem.PosDescription = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPosDescription,
                ValidationErrorMessages.InvalidPosDescription,
                nameof(testItem.PosDescription),
                (i) => i.PosDescription);
        }

        [TestMethod]
        public void ValidateCollection_PosScaleTareIsValid_NoError()
        {
            //Given
            testItem.PosScaleTare = "1";
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "0"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "0.0000"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "2"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "3.765"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "3.7654"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "9"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "9.9"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "9.99"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "9.999"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "9.9999"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PosScaleTareIsInvalid_InvalidPosScaleTareError()
        {
            //Given
            testItem.PosScaleTare = "a";
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "+"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "/"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "10"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "11"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "19"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "9.99999"));
            testItems.Add(CopyTestItem(i => i.PosScaleTare = "0.00000"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPosScaleTare,
                ValidationErrorMessages.InvalidPosScaleTare,
                nameof(testItem.PosScaleTare),
                (i) => i.PosScaleTare);
        }

        [TestMethod]
        public void ValidateCollection_PosScaleTareIsEmpty_InvalidPosScaleTareError()
        {
            //Given
            testItem.PosScaleTare = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPosScaleTare,
                ValidationErrorMessages.InvalidPosScaleTare,
                nameof(testItem.PosScaleTare),
                (i) => i.PosScaleTare);
        }

        [TestMethod]
        public void ValidateCollection_PremiumBodyCareIsValid_NoError()
        {
            //Given
            testItem.PremiumBodyCare = "1";
            testItems.Add(CopyTestItem(i => i.PremiumBodyCare = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PremiumBodyCareIsInvalid_InvalidPremiumBodyCareError()
        {
            //Given
            testItem.PremiumBodyCare = "Invalid value";
            testItems.Add(CopyTestItem(i => i.PremiumBodyCare = "asdf"));
            testItems.Add(CopyTestItem(i => i.PremiumBodyCare = "1234"));
            testItems.Add(CopyTestItem(i => i.PremiumBodyCare = " "));
            testItems.Add(CopyTestItem(i => i.PremiumBodyCare = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPremiumBodyCare,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.PremiumBodyCare),
                (i) => i.PremiumBodyCare);
        }

        [TestMethod]
        public void ValidateCollection_PremiumBodyCareIsEmpty_NoError()
        {
            //Given
            testItem.PremiumBodyCare = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProductDescriptionIsValid_NoError()
        {
            //Given
            testItem.ProductDescription = "Valid value";
            testItems.Add(CopyTestItem(i => i.ProductDescription = new string('a', MaxLengths.ProductDescription)));
            testItems.Add(CopyTestItem(i => i.ProductDescription = "abc123%!@"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProductDescriptionIsInvalid_InvalidProductDescriptionError()
        {
            //Given
            testItem.ProductDescription = new string('a', MaxLengths.ProductDescription + 1);
            testItems.Add(CopyTestItem(i => i.ProductDescription = "+"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidProductDescription,
                ValidationErrorMessages.InvalidProductDescription,
                nameof(testItem.ProductDescription),
                (i) => i.ProductDescription);
        }

        [TestMethod]
        public void ValidateCollection_ProductDescriptionIsEmpty_InvalidProductDescriptionError()
        {
            //Given
            testItem.ProductDescription = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidProductDescription,
                ValidationErrorMessages.InvalidProductDescription,
                nameof(testItem.ProductDescription),
                (i) => i.ProductDescription);
        }

        [TestMethod]
        public void ValidateCollection_ProductFlavorTypeIsValid_NoError()
        {
            //Given
            testItem.ProductFlavorType = "Valid value";
            testItems.Add(CopyTestItem(i => i.ProductFlavorType = new string('a', MaxLengths.StandardProperty255)));
            testItems.Add(CopyTestItem(i => i.ProductFlavorType = "abc123%!@"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProductFlavorTypeIsInvalid_InvalidProductFlavorTypeError()
        {
            //Given
            testItem.ProductFlavorType = new string('a', MaxLengths.StandardProperty255 + 1);
            testItems.Add(CopyTestItem(i => i.ProductFlavorType = "+"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidProductFlavorType,
                ValidationErrorMessages.InvalidProductFlavorType,
                nameof(testItem.ProductFlavorType),
                (i) => i.ProductFlavorType);
        }

        [TestMethod]
        public void ValidateCollection_ProductFlavorTypeIsEmpty_InvalidProductFlavorTypeError()
        {
            //Given
            testItem.ProductFlavorType = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProhibitDiscountIsValid_NoError()
        {
            //Given
            testItem.ProhibitDiscount = "1";
            testItems.Add(CopyTestItem(i => i.ProhibitDiscount = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProhibitDiscountIsInvalid_InvalidProhibitDiscountError()
        {
            //Given
            testItem.ProhibitDiscount = "Invalid value";
            testItems.Add(CopyTestItem(i => i.ProhibitDiscount = "asdf"));
            testItems.Add(CopyTestItem(i => i.ProhibitDiscount = "1234"));
            testItems.Add(CopyTestItem(i => i.ProhibitDiscount = " "));
            testItems.Add(CopyTestItem(i => i.ProhibitDiscount = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidProhibitDiscount,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.ProhibitDiscount),
                (i) => i.ProhibitDiscount);
        }

        [TestMethod]
        public void ValidateCollection_ProhibitDiscountIsEmpty_InvalidProhibitDiscountError()
        {
            //Given
            testItem.ProhibitDiscount = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidProhibitDiscount,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.ProhibitDiscount),
                (i) => i.ProhibitDiscount);
        }

        [TestMethod]
        public void ValidateCollection_RetailSizeIsValid_NoError()
        {
            //Given
            testItem.RetailSize = "1";
            testItems.Add(CopyTestItem(i => i.RetailSize = "990"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "123.123"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "7896"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "0.24"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RetailSizeIsInvalid_InvalidRetailSizeError()
        {
            //Given
            testItem.RetailSize = "Invalid value";
            testItems.Add(CopyTestItem(i => i.RetailSize = "asdf"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "123456"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "99999.12345"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "100000"));
            testItems.Add(CopyTestItem(i => i.RetailSize = "0.12345"));
            testItems.Add(CopyTestItem(i => i.RetailSize = " "));
            testItems.Add(CopyTestItem(i => i.RetailSize = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidRetailSize,
                ValidationErrorMessages.InvalidRetailSize,
                nameof(testItem.RetailSize),
                (i) => i.RetailSize);
        }

        [TestMethod]
        public void ValidateCollection_RetailSizeIsEmpty_InvalidRetailSizeError()
        {
            //Given
            testItem.RetailSize = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidRetailSize,
                ValidationErrorMessages.InvalidRetailSize,
                nameof(testItem.RetailSize),
                (i) => i.RetailSize);
        }

        [TestMethod]
        public void ValidateCollection_RetailUomIsValid_NoError()
        {
            //Given
            foreach (var description in UomCodes.ByName.Values)
            {
                testItems.Add(CopyTestItem(i => i.RetailUom = description));
            }
            testItem.RetailUom = UomCodes.Each;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RetailUomIsInvalid_InvalidRetailUomError()
        {
            //Given
            testItem.RetailUom = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.RetailUom = "789456123"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidRetailUom,
                ValidationErrorMessages.InvalidRetailUom,
                nameof(testItem.RetailUom),
                (i) => i.RetailUom);
        }

        [TestMethod]
        public void ValidateCollection_RetailUomIsEmpty_InvalidRetailUomError()
        {
            //Given
            testItem.RetailUom = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidRetailUom,
                ValidationErrorMessages.InvalidRetailUom,
                nameof(testItem.RetailUom),
                (i) => i.RetailUom);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeIsValid_NoError()
        {
            //Given
            testItem.ScanCode = "1234";
            testItems.Add(CopyTestItem(i => i.ScanCode = new string('1', MaxLengths.ScanCode)));
            testItems.Add(CopyTestItem(i => i.ScanCode = "100000000"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeIsInvalid_InvalidScanCodeError()
        {
            //Given
            testItem.ScanCode = new string('1', MaxLengths.ScanCode + 1);
            testItems.Add(CopyTestItem(i => i.ScanCode = "0"));
            testItems.Add(CopyTestItem(i => i.ScanCode = "01234"));
            testItems.Add(CopyTestItem(i => i.ScanCode = " "));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidScanCode,
                ValidationErrorMessages.InvalidScanCode,
                nameof(testItem.ScanCode),
                (i) => i.ScanCode);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeIsEmpty_InvalidScanCodeError()
        {
            //Given
            testItem.ScanCode = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidScanCode,
                ValidationErrorMessages.InvalidScanCode,
                nameof(testItem.ScanCode),
                (i) => i.ScanCode);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeTypeIdIsValid_NoError()
        {
            //Given
            foreach (var description in ScanCodeTypes.Ids.Keys)
            {
                testItems.Add(CopyTestItem(i => i.ScanCodeType = description));
            }
            testItem.ScanCodeType = ScanCodeTypes.Descriptions.PosPlu;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeTypeIdIsInvalid_InvalidScanCodeTypeIdError()
        {
            //Given
            testItem.ScanCodeType = (-1).ToString();
            testItems.Add(CopyTestItem(i => i.ScanCodeType = 10.ToString()));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidScanCodeType,
                ValidationErrorMessages.InvalidScanCodeType,
                nameof(testItem.ScanCodeType),
                (i) => i.ScanCodeType.ToString());
        }

        [TestMethod]
        public void ValidateCollection_SeafoodCatchTypeIsValid_NoError()
        {
            //Given
            foreach (var description in SeafoodCatchTypes.Descriptions.AsArray)
            {
                testItems.Add(CopyTestItem(i => i.SeafoodCatchType = description));
            }
            testItem.SeafoodCatchType = SeafoodCatchTypes.Descriptions.AsArray[0];

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SeafoodCatchTypeIsInvalid_InvalidSeafoodCatchTypeError()
        {
            //Given
            testItem.SeafoodCatchType = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.SeafoodCatchType = "789456123"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidSeafoodCatchType,
                ValidationErrorMessages.InvalidSeafoodCatchType,
                nameof(testItem.SeafoodCatchType),
                (i) => i.SeafoodCatchType);
        }

        [TestMethod]
        public void ValidateCollection_SeafoodCatchTypeIsEmpty_InvalidSeafoodCatchTypeError()
        {
            //Given
            testItem.SeafoodCatchType = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SequenceIdIsValid_NoError()
        {
            //Given
            testItem.SequenceId = 10;
            testItems.Add(CopyTestItem(i => i.SequenceId = 11));
            testItems.Add(CopyTestItem(i => i.SequenceId = 12));
            testItems.Add(CopyTestItem(i => i.SequenceId = 100));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SequenceIdIsInvalid_OutOfSyncItemUpdateError()
        {
            //Given
            testItem.SequenceId = 9;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.OutOfSyncItemUpdateErrorCode,
                ValidationErrorMessages.OutOfSyncItemUpdateSequenceIdErrorCode,
                testItem.SequenceId.ToString(),
                testItemValidationPropertiesResultModel.SequenceId.Value.ToString());
        }

        [TestMethod]
        public void ValidateCollection_SequenceIdIsInvalidButValidateSequenceIdIsTurnedOff_NoError()
        {
            //Given
            settings.ValidateSequenceId = false;
            testItem.SequenceId = 9;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_TaxHierarchyClassIdIsValid_NoError()
        {
            //Given
            testItem.TaxHierarchyClassId = "0123456";
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "0000000"));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "1000000"));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "0101010"));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "1023401"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_TaxHierarchyClassIdIsInvalid_InvalidTaxHierarchyClassIdError()
        {
            //Given
            testItem.TaxHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = " "));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "789"));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "asdf"));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "A"));
            testItems.Add(CopyTestItem(i => i.TaxHierarchyClassId = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidTaxHierarchyClassId,
                ValidationErrorMessages.InvalidTaxHierarchyClassId,
                nameof(testItem.TaxHierarchyClassId),
                (i) => i.TaxHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_TaxHierarchyClassIdIsEmpty_InvalidTaxHierarchyClassIdError()
        {
            //Given
            testItem.TaxHierarchyClassId = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(
                ValidationErrorCodes.InvalidTaxHierarchyClassId,
                ValidationErrorMessages.InvalidTaxHierarchyClassId.GetFormattedValidationMessage(nameof(testItem.TaxHierarchyClassId), testItem.TaxHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_VeganIsValid_NoError()
        {
            //Given
            testItem.Vegan = "Valid value";
            testItems.Add(CopyTestItem(i => i.Vegan = new string('a', MaxLengths.StandardProperty255)));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_VeganIsInvalid_VeganError()
        {
            //Given
            testItem.Vegan = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidVegan,
                ValidationErrorMessages.InvalidCertificationAgency,
                nameof(testItem.Vegan),
                (i) => i.Vegan);
        }

        [TestMethod]
        public void ValidateCollection_VeganIsEmpty_NoError()
        {
            //Given
            testItem.Vegan = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_VegetarianIsValid_NoError()
        {
            //Given
            testItem.Vegetarian = "1";
            testItems.Add(CopyTestItem(i => i.Vegetarian = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_VegetarianIsInvalid_InvalidVegetarianError()
        {
            //Given
            testItem.Vegetarian = "Invalid value";
            testItems.Add(CopyTestItem(i => i.Vegetarian = "asdf"));
            testItems.Add(CopyTestItem(i => i.Vegetarian = "1234"));
            testItems.Add(CopyTestItem(i => i.Vegetarian = " "));
            testItems.Add(CopyTestItem(i => i.Vegetarian = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidVegetarian,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.Vegetarian),
                (i) => i.Vegetarian);
        }

        [TestMethod]
        public void ValidateCollection_VegetarianIsEmpty_NoError()
        {
            //Given
            testItem.Vegetarian = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_WholeTradeIsValid_NoError()
        {
            //Given
            testItem.WholeTrade = "1";
            testItems.Add(CopyTestItem(i => i.WholeTrade = "0"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_WholeTradeIsInvalid_InvalidWholeTradeError()
        {
            //Given
            testItem.WholeTrade = "Invalid value";
            testItems.Add(CopyTestItem(i => i.WholeTrade = "asdf"));
            testItems.Add(CopyTestItem(i => i.WholeTrade = "1234"));
            testItems.Add(CopyTestItem(i => i.WholeTrade = " "));
            testItems.Add(CopyTestItem(i => i.WholeTrade = "."));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidWholeTrade,
                ValidationErrorMessages.InvalidBooleanStringZeroOrOne,
                nameof(testItem.WholeTrade),
                (i) => i.WholeTrade);
        }

        [TestMethod]
        public void ValidateCollection_WholeTradeIsEmpty_NoError()
        {
            //Given
            testItem.WholeTrade = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NonExistentBrand_NonExistentBrandError()
        {
            //Given
            testItemValidationPropertiesResultModel.BrandId = null;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.NonExistentBrand,
                ValidationErrorMessages.NonExistentBrand,
                testItem.BrandsHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NonExistentSubTeam_NonExistentSubTeamError()
        {
            //Given
            testItemValidationPropertiesResultModel.SubTeamId = null;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.NonExistentSubTeam,
                ValidationErrorMessages.NonExistentSubTeam,
                testItem.FinancialHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NonExistentSubBrick_NonExistentSubBrickError()
        {
            //Given
            testItemValidationPropertiesResultModel.SubBrickId = null;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.NonExistentSubBrick,
                ValidationErrorMessages.NonExistentSubBrick,
                testItem.MerchandiseHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NonExistentNational_NonExistentNationalClassError()
        {
            //Given
            testItemValidationPropertiesResultModel.NationalClassId = null;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.NonExistentNationalClass,
                ValidationErrorMessages.NonExistentNationalClass,
                testItem.NationalHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NonExistentTax_NonExistentTaxError()
        {
            //Given
            testItemValidationPropertiesResultModel.TaxClassId = null;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.NonExistentTax,
                ValidationErrorMessages.NonExistentTax,
                testItem.TaxHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_OutOfSyncItemUpdate_OutOfSyncItemUpdateError()
        {
            //Given
            testItemValidationPropertiesResultModel.ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.OutOfSyncItemUpdateErrorCode,
                ValidationErrorMessages.OutOfSyncItemUpdateErrorCode,
                testItem.ModifiedDate,
                testItemValidationPropertiesResultModel.ModifiedDate.ToString());
        }

        [TestMethod]
        public void ValidateCollection_ModifiedDateHasSameNumberOfMilliseconds_NoOutOfSyncItemUpdateError()
        {
            //Given
            var testModifiedDate = DateTime.Now;
            testItem.ModifiedDate = testModifiedDate.AddTicks(-testModifiedDate.Ticks % TimeSpan.TicksPerMillisecond).ToString();
            testItemValidationPropertiesResultModel.ModifiedDate = testModifiedDate.ToString();

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ModifiedDateWithProdDataFromFailedMessage_NoOutOfSyncItemUpdateError()
        {
            //Given
            testItem.ModifiedDate = "2015-04-14T10:05:06.661Z";
            testItemValidationPropertiesResultModel.ModifiedDate = "2015-04-14 10:05:06.6613621";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlobalPricingProgramIsValid_NoError()
        {
            //Given
            testItem.GlobalPricingProgram = "Everyday Low Price (NTZ)";
            testItems.Add(CopyTestItem(i => i.FlexibleText = @"The GlobalPricingProgram field should allow most any character 1234567890 `~!@#$%^&*()_+-=[]{};':"",./<>? gLoBaLpRiCiNgPrOgRaM gLoBaLpRiCiNgPrOgRaM gLoBaLpRiCiNgPrOgRaM  gLoBaLpRiCiNgPrOgRaM  gLoBaLpRiCiNgPrOgRaM  gLoBaLpRiCiNgPrOgRaM maximum len. is 255"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlobalPricingProgramIsEmpty_NoError()
        {
            //Given
            testItem.GlobalPricingProgram = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlobalPricingProgramsInvalid_GlobalPricingProgramError()
        {
            //Given
            testItem.GlobalPricingProgram = new string('a', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidGlobalPricingProgram,
                ValidationErrorMessages.InvalidGlobalPricingProgram,
                nameof(testItem.GlobalPricingProgram),
                (i) => i.GlobalPricingProgram);
        }

        [TestMethod]
        public void ValidateCollection_FlexibleTextIsValid_NoError()
        {
            //Given
            testItem.FlexibleText = "Flexible text is wonderful!";
            testItems.Add(CopyTestItem(i => i.FlexibleText = @"The Flexible Text field should allow most any character 1234567890 `~!@#$%^&*()_+-=[]{};':"",./<>? fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT fLeXiBlEtExT maximum len. is 300"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FlexibleTextIsEmpty_NoError()
        {
            //Given
            testItem.FlexibleText = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FlexibleTextIsInvalid_FlexibleTextError()
        {
            //Given
            testItem.FlexibleText = new string('x', MaxLengths.FlexibleText + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidFlexibleText,
                ValidationErrorMessages.InvalidFlexibleTextLength,
                nameof(testItem.FlexibleText),
                (i) => i.FlexibleText);
        }

        [TestMethod]
        public void ValidateCollection_MadeWithOrganicGrapesIsValid_NoError()
        {
            //Given
            testItem.MadeWithOrganicGrapes = "0";
            testItems.Add(CopyTestItem(i => i.MadeWithOrganicGrapes = "1"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MadeWithOrganicGrapesIsEmpty_NoError()
        {
            //Given
            testItem.MadeWithOrganicGrapes = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MadeWithOrganicGrapesIsInvalid_MadeWithOrganicGrapesError()
        {
            //Given
            testItem.MadeWithOrganicGrapes = new string('x', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidMadeWithOrganicGrapes,
                ValidationErrorMessages.InvalidMadeWithOrganicGrapes,
                nameof(testItem.MadeWithOrganicGrapes),
                (i) => i.MadeWithOrganicGrapes);
        }

        [TestMethod]
        public void ValidateCollection_PrimeBeefIsValid_NoError()
        {
            //Given
            testItem.PrimeBeef = "YES";
            testItems.Add(CopyTestItem(i => i.PrimeBeef = "NO"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PrimeBeefIsEmpty_NoError()
        {
            //Given
            testItem.PrimeBeef = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PrimeBeefIsInvalid_PrimeBeefError()
        {
            //Given
            testItem.PrimeBeef = "I'm not sure if it's Prime Beef or not. Could be?";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidPrimeBeef,
                ValidationErrorMessages.InvalidPrimeBeef,
                nameof(testItem.PrimeBeef),
                (i) => i.PrimeBeef);
        }

        [TestMethod]
        public void ValidateCollection_RainforestAllianceIsValid_NoError()
        {
            //Given
            testItem.RainforestAlliance = "no";
            testItems.Add(CopyTestItem(i => i.RainforestAlliance = "yes"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RainforestAllianceIsEmpty_NoError()
        {
            //Given
            testItem.RainforestAlliance = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RainforestAllianceIsInvalid_RainforestAllianceError()
        {
            //Given
            testItem.RainforestAlliance = "nyet";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidRainforestAlliance,
                ValidationErrorMessages.InvalidRainforestAlliance,
                nameof(testItem.RainforestAlliance),
                (i) => i.RainforestAlliance);
        }

        [TestMethod]
        public void ValidateCollection_RefrigeratedIsValid_NoError()
        {
            //Given
            testItem.Refrigerated = "Refrigerated";
            testItems.Add(CopyTestItem(i => i.Refrigerated = "Shelf Stable"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RefrigeratedIsEmpty_NoError()
        {
            //Given
            testItem.Refrigerated = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RefrigeratedIsInvalid_RefrigeratedError()
        {
            //Given
            testItem.Refrigerated = "yes";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidRefrigerated,
                ValidationErrorMessages.InvalidRefrigerated,
                nameof(testItem.Refrigerated),
                (i) => i.Refrigerated);
        }

        [TestMethod]
        public void ValidateCollection_SmithsonianBirdFriendlyIsValid_NoError()
        {
            //Given
            testItem.SmithsonianBirdFriendly = "Yes";
            testItems.Add(CopyTestItem(i => i.SmithsonianBirdFriendly = "No"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SmithsonianBirdFriendlyIsEmpty_NoError()
        {
            //Given
            testItem.SmithsonianBirdFriendly = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SmithsonianBirdFriendlyIsInvalid_SmithsonianBirdFriendlyError()
        {
            //Given
            testItem.SmithsonianBirdFriendly = "100";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidSmithsonianBirdFriendly,
                ValidationErrorMessages.InvalidSmithsonianBirdFriendly,
                nameof(testItem.SmithsonianBirdFriendly),
                (i) => i.SmithsonianBirdFriendly);
        }

        [TestMethod]
        public void ValidateCollection_WicEligibleIsValid_NoError()
        {
            //Given
            testItem.WicEligible = "no";
            testItems.Add(CopyTestItem(i => i.WicEligible = "Yes"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_WicEligibleIsEmpty_NoError()
        {
            //Given
            testItem.WicEligible = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_WicEligibleIsInvalid_WicEligibleError()
        {
            //Given
            testItem.WicEligible = new string('W', MaxLengths.StandardProperty255 + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidWicEligible,
                ValidationErrorMessages.InvalidWicEligible,
                nameof(testItem.WicEligible),
                (i) => i.WicEligible);
        }

        [TestMethod]
        public void ValidateCollection_ShelfLifeIsValid_NoError()
        {
            //Given
            testItem.ShelfLife = "30";
            testItems.Add(CopyTestItem(i => i.ShelfLife = "0"));
            testItems.Add(CopyTestItem(i => i.ShelfLife = "1"));
            testItems.Add(CopyTestItem(i => i.ShelfLife = "3"));
            testItems.Add(CopyTestItem(i => i.ShelfLife = "100"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ShelfLifeIsEmpty_NoError()
        {
            //Given
            testItem.ShelfLife = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ShelfLifeIsInvalid_ShelfLifeError()
        {
            //Given
            testItem.ShelfLife = "101";

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidShelfLife,
                ValidationErrorMessages.InvalidShelfLife,
                nameof(testItem.ShelfLife),
                (i) => i.ShelfLife);
        }

        [TestMethod]
        public void ValidateCollection_SelfCheckoutItemTareGroupIsValid_NoError()
        {
            //Given
            testItem.SelfCheckoutItemTareGroup = "$elf-checkout item tare group";
            testItems.Add(CopyTestItem(i => i.SelfCheckoutItemTareGroup = @"most text is OK 1234567890  `~!@#$%^&*()_+-=[]{};':"",./<>?"));

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SelfCheckoutItemTareGroupIsEmpty_NoError()
        {
            //Given
            testItem.SelfCheckoutItemTareGroup = string.Empty;

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrors(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SelfCheckoutItemTareGroupIsInvalid_SelfCheckoutItemTareGroupError()
        {
            //Given
            testItem.SelfCheckoutItemTareGroup = new string('$', MaxLengths.SelfCheckoutItemTareGroup + 1);

            //When
            validator.ValidateCollection(testItems);

            //Then
            AssertExpectedErrorsWithExpectedMessages(
                ValidationErrorCodes.InvalidSelfCheckoutItemTareGroup,
                ValidationErrorMessages.InvalidSelfCheckoutItemTareGroupLength,
                nameof(testItem.SelfCheckoutItemTareGroup),
                (i) => i.SelfCheckoutItemTareGroup);
        }

        private void AssertExpectedErrors(string expectedErrorCode, string expectedErrorDetails)
        {
            //Then
            foreach (var item in testItems)
            {
                Assert.AreEqual(expectedErrorCode, item.ErrorCode, item.ErrorDetails);
                Assert.AreEqual(expectedErrorDetails, item.ErrorDetails);
            }
        }

        private void AssertExpectedErrorsWithExpectedMessages(string expectedErrorCode, string expectedErrorDetails,
            string propertyName, Func<ItemModel, string> getValueForErrorDetails)
        {
            foreach (var item in testItems)
            {
                Assert.AreEqual(expectedErrorCode, item.ErrorCode);

                var expectedErrorDetailsWithFormattedValues = expectedErrorDetails
                    .GetFormattedValidationMessage(propertyName, getValueForErrorDetails(item));
                Assert.AreEqual(expectedErrorDetailsWithFormattedValues, item.ErrorDetails);
            }
        }

        private void AssertExpectedErrorsWithExpectedMessages(string expectedErrorCode, string expectedErrorDetails,
            params string[] propertyValues)
        {
            foreach (var item in testItems)
            {
                Assert.AreEqual(expectedErrorCode, item.ErrorCode);

                var expectedErrorDetailsWithFormattedValues = expectedErrorDetails
                    .GetFormattedValidationMessageWithStringFormat(propertyValues);
                Assert.AreEqual(expectedErrorDetailsWithFormattedValues, item.ErrorDetails);
            }
        }

        private ItemModel CopyTestItem(Action<ItemModel> setter = null)
        {
            ItemModel newTestItemModel = new ItemModel();

            foreach (var property in typeof(ItemModel).GetProperties())
            {
                property.SetValue(
                    newTestItemModel,
                    property.GetValue(testItem));
            }

            if(setter != null)
                setter(newTestItemModel);

            return newTestItemModel;
        }
    }
}
