using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.Item.Validators;
using Icon.Infor.Listeners.Item.Models;
using System.Collections.Generic;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Constants;
using System.Text.RegularExpressions;
using System.Globalization;
using Icon.Common.DataAccess;
using Moq;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Notifiers;
using Icon.Common.Email;
using System.IO;
using Icon.Esb.Subscriber;
using System.Linq;

namespace Icon.Infor.Listeners.Item.Tests.Validators
{
    [TestClass]
    public class ItemModelValidatorTests
    {
        private ItemModelValidator validator;
        private Mock<ICommandHandler<ValidateItemsCommand>> mockValidateItemsCommandHandler;
        private List<ItemModel> testItems;
        private ItemModel testItem;

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
                FoodStampEligible = ItemValues.TrueBooleanValue,
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
                ModifiedDate = "2016-08-01T14:49:29.284Z",
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
                TaxHierarchyClassId = "0123456",
                Vegan = string.Empty,
                Vegetarian = string.Empty,
                WholeTrade = string.Empty
            };

            testItems = new List<ItemModel>
            {
                testItem
            };

            mockValidateItemsCommandHandler = new Mock<ICommandHandler<ValidateItemsCommand>>();
            validator = new ItemModelValidator(mockValidateItemsCommandHandler.Object);
        }

        [TestMethod]
        public void ValidateCollection_AllPropertiesAreValid_ItemShouldBeValid()
        {
            //When
            validator.ValidateCollection(testItems);

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AirChilledIsValid_NoError()
        {
            // Given
            testItem.AirChilled = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.AirChilled = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AirChilledIsInvalid_NoError()
        {
            // Given
            testItem.AirChilled = " ";
            testItems.Add(CopyTestItem(i => i.AirChilled = "asdf"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidAirChilled,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.AirChilled),
                (i) => i.AirChilled);
        }

        [TestMethod]
        public void ValidateCollection_AirChilledIsEmpty_NoError()
        {
            // Given
            testItem.AirChilled = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidAlcoholByVolume,
                ValidationErrors.Messages.InvalidAlcoholByVolume,
                nameof(testItem.AlcoholByVolume),
                (i) => i.AlcoholByVolume);
        }

        [TestMethod]
        public void ValidateCollection_AlcoholByVolumeIsEmpty_NoError()
        {
            // Given
            testItem.AlcoholByVolume = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_AnimalWelfareRatingIsInvalid_InvalidAnimalWelfareRatingError()
        {
            //Given
            testItem.AnimalWelfareRating = "Invalid value";

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidAnimalWelfareRating,
                ValidationErrors.Messages.InvalidAnimalWelfareRating,
                nameof(testItem.AnimalWelfareRating),
                (i) => i.AnimalWelfareRating);
        }

        [TestMethod]
        public void ValidateCollection_AnimalWelfareRatingIsEmpty_NoError()
        {
            //Given
            testItem.AnimalWelfareRating = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_BiodynamicIsValid_NoError()
        {
            //Given
            testItem.Biodynamic = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem((i) => i.Biodynamic = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_BiodynamicIsInvalid_InvalidBiodynamicError()
        {
            //Given
            testItem.Biodynamic = "Invalid value";

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidBiodynamic,
                ValidationErrors.Messages.InvalidBooleanString.GetFormattedValidationMessage(nameof(testItem.Biodynamic), testItem.Biodynamic));
        }

        [TestMethod]
        public void ValidateCollection_BiodynamicIsEmpty_NoError()
        {
            //Given
            testItem.Biodynamic = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_BrandIsInvalid_InvalidBrandError()
        {
            //Given
            testItem.BrandsHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "0"));
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "01234"));
            testItems.Add(CopyTestItem((i) => i.BrandsHierarchyClassId = "-12340"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidBrand,
                ValidationErrors.Messages.InvalidBrand, 
                nameof(testItem.BrandsHierarchyClassId),
                (i) => i.BrandsHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_BrandIsEmpty_InvalidBrandError()
        {
            //Given
            testItem.BrandsHierarchyClassId = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidBrand,
                ValidationErrors.Messages.InvalidBrand.GetFormattedValidationMessage(nameof(testItem.BrandsHierarchyClassId), testItem.BrandsHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_CaseinFreeIsValid_NoError()
        {
            //Given
            testItem.CaseinFree = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem((i) => i.CaseinFree = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CaseinFreeIsInvalid_InvalidCaseinFreeError()
        {
            //Given
            testItem.CaseinFree = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "00"));
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "11"));
            testItems.Add(CopyTestItem((i) => i.CaseinFree = "320912-9%"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidCaseinFree,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.CaseinFree),
                (i) => i.CaseinFree);
        }

        [TestMethod]
        public void ValidateCollection_CaseinFreeIsEmpty_NoError()
        {
            //Given
            testItem.CaseinFree = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseMilkTypeIsInvalid_InvalidCheeseMilkTypeError()
        {
            //Given
            testItem.CheeseMilkType = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.CheeseMilkType = "789456123"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidCheeseMilkType,
                ValidationErrors.Messages.InvalidCheeseMilkType,
                nameof(testItem.CheeseMilkType),
                (i) => i.CheeseMilkType);
        }

        [TestMethod]
        public void ValidateCollection_CheeseMilkTypeIsEmpty_NoError()
        {
            //Given
            testItem.CheeseMilkType = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseRawIsValid_NoError()
        {
            //Given
            testItem.CheeseRaw = ItemValues.FalseBooleanValue;
            testItem.CheeseRaw = ItemValues.TrueBooleanValue;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_CheeseRawIsInvalid_InvalidCheeseRawError()
        {
            //Given
            testItem.CheeseRaw = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.CheeseRaw = "789456123"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidCheeseRaw,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.CheeseRaw),
                (i) => i.CheeseRaw);
        }

        [TestMethod]
        public void ValidateCollection_CheeseRawIsEmpty_NoError()
        {
            //Given
            testItem.CheeseRaw = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateMerchandiseHierarchyClassIsFalse_NoError()
        {
            //Given
            testItem.ContainesDuplicateMerchandiseClass = false;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateMerchandiseHierarchyClassIsTrue_NoError()
        {
            //Given
            testItem.ContainesDuplicateMerchandiseClass = true;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.DuplicateMerchandiseHierarchyClass,
                ValidationErrors.Messages.DuplicateMerchandiseHierarchyClass);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateNationalHierarchyClassIsFalse_NoError()
        {
            //Given
            testItem.ContainesDuplicateNationalClass = false;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ContainsDuplicateNationalHierarchyClassIsTrue_NoError()
        {
            //Given
            testItem.ContainesDuplicateNationalClass = true;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.DuplicateNationalHierarchyClass, 
                ValidationErrors.Messages.DuplicateNationalHierarchyClass);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DeliverySystemIsInvalid_InvalidDeliverySystemError()
        {
            //Given
            testItem.DeliverySystem = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.DeliverySystem = "789456123"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidDeliverySystem,
                ValidationErrors.Messages.InvalidDeliverySystem,
                nameof(testItem.DeliverySystem),
                (i) => i.DeliverySystem);
        }

        [TestMethod]
        public void ValidateCollection_DeliverySystemIsEmpty_NoError()
        {
            //Given
            testItem.DeliverySystem = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightIsValid_NoError()
        {
            //Given
            testItem.DrainedWeight = "124";
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "0"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "0.1234"));
            testItems.Add(CopyTestItem(i => i.DrainedWeight = "123456789.1234"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidDrainedWeight,
                ValidationErrors.Messages.InvalidDrainedWeight,
                nameof(testItem.DrainedWeight),
                (i) => i.DrainedWeight);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightIsEmpty_NoError()
        {
            //Given
            testItem.DrainedWeight = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightUomIsValid_NoError()
        {
            //Given
            testItem.DrainedWeightUom = DrainedWeightUoms.Ml;
            testItems.Add(CopyTestItem(i => i.DrainedWeightUom = DrainedWeightUoms.Oz));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidDrainedWeightUom,
                ValidationErrors.Messages.InvalidDrainedWeightUom,
                nameof(testItem.DrainedWeightUom),
                (i) => i.DrainedWeightUom);
        }

        [TestMethod]
        public void ValidateCollection_DrainedWeightUomIsEmpty_NoError()
        {
            //Given
            testItem.DrainedWeightUom = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_DryAgedIsValid_NoError()
        {
            //Given
            testItem.DryAged = ItemValues.FalseBooleanValue;
            testItems.Add(CopyTestItem(i => i.DryAged = ItemValues.TrueBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidDryAged,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.DryAged),
                (i) => i.DryAged);
        }

        [TestMethod]
        public void ValidateCollection_DryAgedIsEmpty_NoError()
        {
            //Given
            testItem.DryAged = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidEcoScaleRating,
                ValidationErrors.Messages.InvalidEcoScaleRating,
                nameof(testItem.EcoScaleRating),
                (i) => i.EcoScaleRating);
        }

        [TestMethod]
        public void ValidateCollection_EcoScaleRatingIsEmpty_NoError()
        {
            //Given
            testItem.EcoScaleRating = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FairTradeCertifiedIsValid_NoError()
        {
            //Given
            foreach (var description in FairTradeCertifiedValues.AsArray)
            {
                testItems.Add(CopyTestItem(i => i.FairTradeCertified = description));
            }
            testItem.FairTradeCertified = FairTradeCertifiedValues.AsArray[0];

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_FairTradeCertifiedIsInvalid_InvalidFairTradeCertifiedError()
        {
            //Given
            testItem.FairTradeCertified = "Invalid value";
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = " "));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "789"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "asdf"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "A"));
            testItems.Add(CopyTestItem(i => i.FairTradeCertified = "."));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFairTradeCertified,
                ValidationErrors.Messages.InvalidFairTradeCertified,
                nameof(testItem.FairTradeCertified),
                (i) => i.FairTradeCertified);
        }

        [TestMethod]
        public void ValidateCollection_FairTradeCertifiedIsEmpty_NoError()
        {
            //Given
            testItem.FairTradeCertified = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFinancialHierarchyClassId,
                ValidationErrors.Messages.InvalidFinancialHierarchyClassId,
                nameof(testItem.FinancialHierarchyClassId),
                (i) => i.FinancialHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_FinancialHierarchyClassIdIsEmpty_InvalidFinancialHierarchyClassIdError()
        {
            //Given
            testItem.FinancialHierarchyClassId = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFinancialHierarchyClassId,
                ValidationErrors.Messages.InvalidFinancialHierarchyClassId.GetFormattedValidationMessage(nameof(testItem.FinancialHierarchyClassId), testItem.FinancialHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_FoodStampEligibleIsValid_NoError()
        {
            //Given
            testItem.FoodStampEligible = ItemValues.FalseBooleanValue;
            testItems.Add(CopyTestItem(i => i.FoodStampEligible = ItemValues.TrueBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFoodStampEligible,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.FoodStampEligible),
                (i) => i.FoodStampEligible);
        }

        [TestMethod]
        public void ValidateCollection_FoodStampEligibleIsEmpty_InvalidFoodStampEligibleError()
        {
            //Given
            testItem.FoodStampEligible = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFoodStampEligible,
                ValidationErrors.Messages.InvalidBooleanString.GetFormattedValidationMessage(nameof(testItem.FoodStampEligible), testItem.FoodStampEligible));
        }

        [TestMethod]
        public void ValidateCollection_FreeRangeIsValid_NoError()
        {
            //Given
            testItem.FreeRange = ItemValues.FalseBooleanValue;
            testItems.Add(CopyTestItem(i => i.FreeRange = ItemValues.TrueBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFreeRange,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.FreeRange),
                (i) => i.FreeRange);
        }

        [TestMethod]
        public void ValidateCollection_FreeRangeIsEmpty_NoError()
        {
            //Given
            testItem.FreeRange = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidFreshOrFrozen,
                ValidationErrors.Messages.InvalidFreshOrFrozen,
                nameof(testItem.FreshOrFrozen),
                (i) => i.FreshOrFrozen);
        }

        [TestMethod]
        public void ValidateCollection_FreshOrFrozenIsEmpty_NoError()
        {
            //Given
            testItem.FreshOrFrozen = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlutenFreeIsValid_NoError()
        {
            //Given
            testItem.GlutenFree = "Valid value";

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GlutenFreeIsInvalid_GlutenFreeError()
        {
            //Given
            testItem.GlutenFree = new string('a', ItemValues.MaxPropertyStringLength + 1);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidGlutenFree,
                ValidationErrors.Messages.InvalidCertificationAgency,
                nameof(testItem.GlutenFree),
                (i) => i.GlutenFree);
        }

        [TestMethod]
        public void ValidateCollection_GlutenFreeIsEmpty_NoError()
        {
            //Given
            testItem.GlutenFree = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_GrassFedIsValid_NoError()
        {
            //Given
            testItem.GrassFed = ItemValues.FalseBooleanValue;
            testItems.Add(CopyTestItem(i => i.GrassFed = ItemValues.TrueBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidGrassFed,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.GrassFed),
                (i) => i.GrassFed);
        }

        [TestMethod]
        public void ValidateCollection_GrassFedIsEmpty_NoError()
        {
            //Given
            testItem.GrassFed = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_HempIsValid_NoError()
        {
            //Given
            testItem.Hemp = ItemValues.FalseBooleanValue;
            testItems.Add(CopyTestItem(i => i.Hemp = ItemValues.TrueBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidHemp,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.Hemp),
                (i) => i.Hemp);
        }

        [TestMethod]
        public void ValidateCollection_HempIsEmpty_NoError()
        {
            //Given
            testItem.Hemp = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_HiddenItemIsValid_NoError()
        {
            //Given
            testItem.HiddenItem = ItemValues.FalseBooleanValue;
            testItems.Add(CopyTestItem(i => i.HiddenItem = ItemValues.TrueBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidHiddenItem,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.HiddenItem),
                (i) => i.HiddenItem);
        }

        [TestMethod]
        public void ValidateCollection_HiddenItemIsEmpty_NoError()
        {
            //Given
            testItem.HiddenItem = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_InforMessageIdIsValid_NoError()
        {
            //Given
            testItem.InforMessageId = Guid.NewGuid();

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_InforMessageIdIsInvalid_InforMessageIdError()
        {
            //Given
            testItem.InforMessageId = Guid.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidInforMessageId,
                ValidationErrors.Messages.InvalidInforMessageId,
                nameof(testItem.InforMessageId),
                (i) => i.InforMessageId.ToString());
        }

        [TestMethod]
        public void ValidateCollection_InforMessageIdIsEmpty_InvalidInforMessageIdNoError()
        {
            //Given
            testItem.InforMessageId = Guid.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidInforMessageId,
                ValidationErrors.Messages.InvalidInforMessageId,
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ItemIdIsInvalid_InvalidItemIdError()
        {
            //Given
            testItem.ItemId = -1;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidItemId,
                ValidationErrors.Messages.InvalidItemId,
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ItemTypeIdIsInvalid__InvalidItemTypeIdError()
        {
            // Given
            testItem.ItemTypeCode = 0.ToString();
            testItems.Add(CopyTestItem(i => i.ItemTypeCode = 123.ToString()));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidItemTypeCode,
                ValidationErrors.Messages.InvalidItemTypeCode,
                nameof(testItem.ItemTypeCode),
                (i) => i.ItemTypeCode.ToString());
        }

        [TestMethod]
        public void ValidateCollection_KosherIsValid_NoError()
        {
            //Given
            testItem.Kosher = "Valid value";
            testItems.Add(CopyTestItem(i => i.Kosher = new string('a', 255)));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_KosherIsInvalid_KosherError()
        {
            //Given
            testItem.Kosher = new string('a', 256);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidKosher,
                ValidationErrors.Messages.InvalidCertificationAgency,
                nameof(testItem.Kosher),
                (i) => i.Kosher.ToString());
        }

        [TestMethod]
        public void ValidateCollection_KosherIsEmpty_NoError()
        {
            //Given
            testItem.Kosher = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_LocalLoanProducerIsValid_NoError()
        {
            //Given
            testItem.LocalLoanProducer = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.LocalLoanProducer = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidLocalLoanProducer,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.LocalLoanProducer),
                (i) => i.LocalLoanProducer);
        }

        [TestMethod]
        public void ValidateCollection_LocalLoanProducerIsEmpty_NoError()
        {
            //Given
            testItem.LocalLoanProducer = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MadeInHouseIsValid_NoError()
        {
            //Given
            testItem.MadeInHouse = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.MadeInHouse = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMadeInHouse,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.MadeInHouse),
                (i) => i.MadeInHouse);
        }

        [TestMethod]
        public void ValidateCollection_MadeInHouseIsEmpty_NoError()
        {
            //Given
            testItem.MadeInHouse = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MainProductNameIsValid_NoError()
        {
            //Given
            testItem.MainProductName = "Valid value";
            testItems.Add(CopyTestItem(i => i.MainProductName = "asdf"));
            testItems.Add(CopyTestItem(i => i.MainProductName = "1234"));
            testItems.Add(CopyTestItem(i => i.MainProductName = "dssdf.d1123032!@"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MainProductNameIsInvalid_InvalidMadeInHouseError()
        {
            //Given
            testItem.MainProductName = new string('a', ItemValues.MaxPropertyStringLength + 1);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMainProductName,
                ValidationErrors.Messages.InvalidMainProductName,
                nameof(testItem.MainProductName),
                (i) => i.MainProductName);
        }

        [TestMethod]
        public void ValidateCollection_MainProductNameIsEmpty_NoError()
        {
            //Given
            testItem.MainProductName = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MerchandiseHierarchyClassIdIsValid_NoError()
        {
            //Given
            testItem.MerchandiseHierarchyClassId = "1234567890";
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = "1234"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MerchandiseHierarchyClassIdIsInvalid_InvalidMerchandiseHierarchyClassIdError()
        {
            //Given
            testItem.MerchandiseHierarchyClassId = "Invalid value";
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = "1234a"));
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = " "));
            testItems.Add(CopyTestItem(i => i.MerchandiseHierarchyClassId = "."));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMerchandiseHierarchyClassId,
                ValidationErrors.Messages.InvalidMerchandiseHierarchyClassId,
                nameof(testItem.MerchandiseHierarchyClassId),
                (i) => i.MerchandiseHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_MerchandiseHierarchyClassIdIsEmpty_InvalidMerchandiseHierarchyClassIdError()
        {
            //Given
            testItem.MerchandiseHierarchyClassId = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMerchandiseHierarchyClassId,
                ValidationErrors.Messages.InvalidMerchandiseHierarchyClassId.GetFormattedValidationMessage(nameof(testItem.MerchandiseHierarchyClassId), testItem.MerchandiseHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_MscIsValid_NoError()
        {
            //Given
            testItem.Msc = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.Msc = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_MscIsInvalid_InvalidMscIdError()
        {
            //Given
            testItem.Msc = "Invalid value";
            testItems.Add(CopyTestItem(i => i.Msc = "1234a"));
            testItems.Add(CopyTestItem(i => i.Msc = " "));
            testItems.Add(CopyTestItem(i => i.Msc = "."));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidMsc,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.Msc),
                (i) => i.Msc);
        }

        [TestMethod]
        public void ValidateCollection_MscIsEmpty_NoError()
        {
            //Given
            testItem.Msc = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NationalHierarchyClassIdIsValid_NoError()
        {
            //Given
            testItem.NationalHierarchyClassId = "1234567890"; ;
            testItems.Add(CopyTestItem(i => i.NationalHierarchyClassId = "123456"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNationalHierarchyClassId,
                ValidationErrors.Messages.InvalidNationalHierarchyClassId,
                nameof(testItem.NationalHierarchyClassId),
                (i) => i.NationalHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NationalHierarchyClassIdIsEmpty_NoError()
        {
            //Given
            testItem.NationalHierarchyClassId = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNationalHierarchyClassId,
                ValidationErrors.Messages.InvalidNationalHierarchyClassId,
                nameof(testItem.NationalHierarchyClassId),
                (i) => i.NationalHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_NonGmoIsValid_NoError()
        {
            //Given
            testItem.NonGmo = "Valid value"; ;
            testItems.Add(CopyTestItem(i => i.NonGmo = new string('a', ItemValues.MaxPropertyStringLength)));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NonGmoIsInvalid_InvalidNonGmoError()
        {
            //Given
            testItem.NonGmo = new string('a', ItemValues.MaxPropertyStringLength + 1);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNonGmo,
                ValidationErrors.Messages.InvalidCertificationAgency,
                nameof(testItem.NonGmo),
                (i) => i.NonGmo);
        }

        [TestMethod]
        public void ValidateCollection_NonGmoIsEmpty_NoError()
        {
            //Given
            testItem.NonGmo = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NotesIsValid_NoError()
        {
            //Given
            testItem.Notes = "Valid value"; ;
            testItems.Add(CopyTestItem(i => i.Notes = new string('a', ItemValues.MaxPropertyStringLength)));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NotesIsInvalid_InvalidNotesError()
        {
            //Given
            testItem.Notes = new string('a', ItemValues.MaxPropertyStringLength + 1);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNotes,
                ValidationErrors.Messages.InvalidNotes,
                nameof(testItem.Notes),
                (i) => i.Notes);
        }

        [TestMethod]
        public void ValidateCollection_NotesIsEmpty_NoError()
        {
            //Given
            testItem.Notes = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_NutritionRequiredIsValid_NoError()
        {
            //Given
            testItem.NutritionRequired = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.NutritionRequired = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidNutritionRequired,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.NutritionRequired),
                (i) => i.NutritionRequired);
        }

        [TestMethod]
        public void ValidateCollection_NutritionRequiredIsEmpty_NoError()
        {
            //Given
            testItem.NutritionRequired = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_OrganicIsValid_NoError()
        {
            //Given
            testItem.Organic = "Valid value"; ;
            testItems.Add(CopyTestItem(i => i.Organic = new string('a', ItemValues.MaxPropertyStringLength)));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_OrganicIsInvalid_InvalidOrganicError()
        {
            //Given
            testItem.Organic = new string('a', ItemValues.MaxPropertyStringLength + 1);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidOrganic,
                ValidationErrors.Messages.InvalidCertificationAgency,
                nameof(testItem.Organic),
                (i) => i.Organic);
        }

        [TestMethod]
        public void ValidateCollection_OrganicIsEmpty_NoError()
        {
            //Given
            testItem.Organic = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_OrganicPersonalCareIsValid_NoError()
        {
            //Given
            testItem.OrganicPersonalCare = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.OrganicPersonalCare = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidOrganicPersonalCare,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.OrganicPersonalCare),
                (i) => i.OrganicPersonalCare);
        }

        [TestMethod]
        public void ValidateCollection_OrganicPersonalCareIsEmpty_NoError()
        {
            //Given
            testItem.OrganicPersonalCare = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PackageUnitIsValid_NoError()
        {
            //Given
            testItem.PackageUnit = "1";
            testItems.Add(CopyTestItem(i => i.PackageUnit = "9.999"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "9.9"));
            testItems.Add(CopyTestItem(i => i.PackageUnit = "9.99"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPackageUnit,
                ValidationErrors.Messages.InvalidPackageUnit,
                nameof(testItem.PackageUnit),
                (i) => i.PackageUnit);
        }

        [TestMethod]
        public void ValidateCollection_PackageUnitIsEmpty_InvalidPackageUnitError()
        {
            //Given
            testItem.PackageUnit = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPackageUnit,
                ValidationErrors.Messages.InvalidPackageUnit,
                nameof(testItem.PackageUnit),
                (i) => i.PackageUnit);
        }

        [TestMethod]
        public void ValidateCollection_PaleoIsValid_NoError()
        {
            //Given
            testItem.Paleo = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.Paleo = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPaleo,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.Paleo),
                (i) => i.Paleo);
        }

        [TestMethod]
        public void ValidateCollection_PaleoIsEmpty_NoError()
        {
            //Given
            testItem.Paleo = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PastureRaisedIsValid_NoError()
        {
            //Given
            testItem.PastureRaised = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.PastureRaised = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPastureRaised,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.PastureRaised),
                (i) => i.PastureRaised);
        }

        [TestMethod]
        public void ValidateCollection_PastureRaisedIsEmpty_NoError()
        {
            //Given
            testItem.PastureRaised = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PosDescriptionIsValid_NoError()
        {
            //Given
            testItem.PosDescription = "Valid value";
            testItems.Add(CopyTestItem(i => i.PosDescription = new string('a', ItemValues.PosDescriptionMaxLength)));
            testItems.Add(CopyTestItem(i => i.PosDescription = "abc123%!@"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_PosDescriptionIsInvalid_InvalidPosDescriptionError()
        {
            //Given
            testItem.PosDescription = new string('a', ItemValues.PosDescriptionMaxLength + 1);
            testItems.Add(CopyTestItem(i => i.PosDescription = "+"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPosDescription,
                ValidationErrors.Messages.InvalidPosDescription,
                nameof(testItem.PosDescription),
                (i) => i.PosDescription);
        }

        [TestMethod]
        public void ValidateCollection_PosDescriptionIsEmpty_InvalidPosDescriptionError()
        {
            //Given
            testItem.PosDescription = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPosDescription,
                ValidationErrors.Messages.InvalidPosDescription,
                nameof(testItem.PosDescription),
                (i) => i.PosDescription);
        }

        [TestMethod]
        public void ValidateCollection_PremiumBodyCareIsValid_NoError()
        {
            //Given
            testItem.PremiumBodyCare = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.PremiumBodyCare = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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


            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidPremiumBodyCare,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.PremiumBodyCare),
                (i) => i.PremiumBodyCare);
        }

        [TestMethod]
        public void ValidateCollection_PremiumBodyCareIsEmpty_NoError()
        {
            //Given
            testItem.PremiumBodyCare = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProductDescriptionIsValid_NoError()
        {
            //Given
            testItem.ProductDescription = "Valid value";
            testItems.Add(CopyTestItem(i => i.ProductDescription = new string('a', ItemValues.ProductDescriptionMaxLength)));
            testItems.Add(CopyTestItem(i => i.ProductDescription = "abc123%!@"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProductDescriptionIsInvalid_InvalidProductDescriptionError()
        {
            //Given
            testItem.ProductDescription = new string('a', ItemValues.ProductDescriptionMaxLength + 1);
            testItems.Add(CopyTestItem(i => i.ProductDescription = "+"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidProductDescription,
                ValidationErrors.Messages.InvalidProductDescription,
                nameof(testItem.ProductDescription),
                (i) => i.ProductDescription);
        }

        [TestMethod]
        public void ValidateCollection_ProductDescriptionIsEmpty_InvalidProductDescriptionError()
        {
            //Given
            testItem.ProductDescription = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidProductDescription,
                ValidationErrors.Messages.InvalidProductDescription,
                nameof(testItem.ProductDescription),
                (i) => i.ProductDescription);
        }

        [TestMethod]
        public void ValidateCollection_ProductFlavorTypeIsValid_NoError()
        {
            //Given
            testItem.ProductFlavorType = "Valid value";
            testItems.Add(CopyTestItem(i => i.ProductFlavorType = new string('a', ItemValues.MaxPropertyStringLength)));
            testItems.Add(CopyTestItem(i => i.ProductFlavorType = "abc123%!@"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProductFlavorTypeIsInvalid_InvalidProductFlavorTypeError()
        {
            //Given
            testItem.ProductFlavorType = new string('a', ItemValues.MaxPropertyStringLength + 1);
            testItems.Add(CopyTestItem(i => i.ProductFlavorType = "+"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidProductFlavorType,
                ValidationErrors.Messages.InvalidProductFlavorType,
                nameof(testItem.ProductFlavorType),
                (i) => i.ProductFlavorType);
        }

        [TestMethod]
        public void ValidateCollection_ProductFlavorTypeIsEmpty_InvalidProductFlavorTypeError()
        {
            //Given
            testItem.ProductFlavorType = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ProhibitDiscountIsValid_NoError()
        {
            //Given
            testItem.ProhibitDiscount = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.ProhibitDiscount = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidProhibitDiscount,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.ProhibitDiscount),
                (i) => i.ProhibitDiscount);
        }

        [TestMethod]
        public void ValidateCollection_ProhibitDiscountIsEmpty_InvalidProhibitDiscountError()
        {
            //Given
            testItem.ProhibitDiscount = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidProhibitDiscount,
                ValidationErrors.Messages.InvalidBooleanString,
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidRetailSize,
                ValidationErrors.Messages.InvalidRetailSize,
                nameof(testItem.RetailSize),
                (i) => i.RetailSize);
        }

        [TestMethod]
        public void ValidateCollection_RetailSizeIsEmpty_InvalidRetailSizeError()
        {
            //Given
            testItem.RetailSize = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidRetailSize,
                ValidationErrors.Messages.InvalidRetailSize,
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_RetailUomIsInvalid_InvalidRetailUomError()
        {
            //Given
            testItem.RetailUom = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.RetailUom = "789456123"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidRetailUom,
                ValidationErrors.Messages.InvalidRetailUom,
                nameof(testItem.RetailUom),
                (i) => i.RetailUom);
        }

        [TestMethod]
        public void ValidateCollection_RetailUomIsEmpty_InvalidRetailUomError()
        {
            //Given
            testItem.RetailUom = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidRetailUom,
                ValidationErrors.Messages.InvalidRetailUom,
                nameof(testItem.RetailUom),
                (i) => i.RetailUom);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeIsValid_NoError()
        {
            //Given
            testItem.ScanCode = "1234";
            testItems.Add(CopyTestItem(i => i.ScanCode = new string('1', ItemValues.ScanCodeMaxLength)));
            testItems.Add(CopyTestItem(i => i.ScanCode = "100000000"));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeIsInvalid_InvalidScanCodeError()
        {
            //Given
            testItem.ScanCode = new string('1', ItemValues.ScanCodeMaxLength + 1);
            testItems.Add(CopyTestItem(i => i.ScanCode = "0"));
            testItems.Add(CopyTestItem(i => i.ScanCode = "01234"));
            testItems.Add(CopyTestItem(i => i.ScanCode = " "));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidScanCode,
                ValidationErrors.Messages.InvalidScanCode,
                nameof(testItem.ScanCode),
                (i) => i.ScanCode);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeIsEmpty_InvalidScanCodeError()
        {
            //Given
            testItem.ScanCode = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidScanCode,
                ValidationErrors.Messages.InvalidScanCode,
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_ScanCodeTypeIdIsInvalid_InvalidScanCodeTypeIdError()
        {
            //Given
            testItem.ScanCodeType = (-1).ToString();
            testItems.Add(CopyTestItem(i => i.ScanCodeType = 10.ToString()));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidScanCodeType,
                ValidationErrors.Messages.InvalidScanCodeType,
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_SeafoodCatchTypeIsInvalid_InvalidSeafoodCatchTypeError()
        {
            //Given
            testItem.SeafoodCatchType = "Invalid value";
            testItems.Add(CopyTestItem((i) => i.SeafoodCatchType = "789456123"));

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidSeafoodCatchType,
                ValidationErrors.Messages.InvalidSeafoodCatchType,
                nameof(testItem.SeafoodCatchType),
                (i) => i.SeafoodCatchType);
        }

        [TestMethod]
        public void ValidateCollection_SeafoodCatchTypeIsEmpty_InvalidSeafoodCatchTypeError()
        {
            //Given
            testItem.SeafoodCatchType = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidTaxHierarchyClassId,
                ValidationErrors.Messages.InvalidTaxHierarchyClassId,
                nameof(testItem.TaxHierarchyClassId),
                (i) => i.TaxHierarchyClassId);
        }

        [TestMethod]
        public void ValidateCollection_TaxHierarchyClassIdIsEmpty_InvalidTaxHierarchyClassIdError()
        {
            //Given
            testItem.TaxHierarchyClassId = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidTaxHierarchyClassId,
                ValidationErrors.Messages.InvalidTaxHierarchyClassId.GetFormattedValidationMessage(nameof(testItem.TaxHierarchyClassId), testItem.TaxHierarchyClassId));
        }

        [TestMethod]
        public void ValidateCollection_VeganIsValid_NoError()
        {
            //Given
            testItem.Vegan = "Valid value";
            testItems.Add(CopyTestItem(i => i.Vegan = new string('a', ItemValues.MaxPropertyStringLength)));

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_VeganIsInvalid_VeganError()
        {
            //Given
            testItem.Vegan = new string('a', ItemValues.MaxPropertyStringLength + 1);

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidVegan,
                ValidationErrors.Messages.InvalidCertificationAgency,
                nameof(testItem.Vegan),
                (i) => i.Vegan);
        }

        [TestMethod]
        public void ValidateCollection_VeganIsEmpty_NoError()
        {
            //Given
            testItem.Vegan = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_VegetarianIsValid_NoError()
        {
            //Given
            testItem.Vegetarian = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.Vegetarian = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidVegetarian,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.Vegetarian),
                (i) => i.Vegetarian);
        }

        [TestMethod]
        public void ValidateCollection_VegetarianIsEmpty_NoError()
        {
            //Given
            testItem.Vegetarian = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        [TestMethod]
        public void ValidateCollection_WholeTradeIsValid_NoError()
        {
            //Given
            testItem.WholeTrade = ItemValues.TrueBooleanValue;
            testItems.Add(CopyTestItem(i => i.WholeTrade = ItemValues.FalseBooleanValue));

            PerformValidateCollectionWhenAndThenSteps(null, null);
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

            PerformValidateCollectionWhenAndThenSteps(
                ValidationErrors.Codes.InvalidWholeTrade,
                ValidationErrors.Messages.InvalidBooleanString,
                nameof(testItem.WholeTrade),
                (i) => i.WholeTrade);
        }

        [TestMethod]
        public void ValidateCollection_WholeTradeIsEmpty_NoError()
        {
            //Given
            testItem.Vegetarian = string.Empty;

            PerformValidateCollectionWhenAndThenSteps(null, null);
        }

        private void PerformValidateCollectionWhenAndThenSteps(string expectedErrorCode, string expectedErrorDetails)
        {
            //When
            validator.ValidateCollection(testItems);

            //Then
            foreach (var item in testItems)
            {
                Assert.AreEqual(expectedErrorCode, item.ErrorCode);
                Assert.AreEqual(expectedErrorDetails, item.ErrorDetails);
            }
        }

        private void PerformValidateCollectionWhenAndThenSteps(string expectedErrorCode, string expectedErrorDetails, string propertyName, Func<ItemModel, string> getValueForErrorDetails)
        {
            //When
            validator.ValidateCollection(testItems);

            //Then
            foreach (var item in testItems)
            {
                Assert.AreEqual(expectedErrorCode, item.ErrorCode);

                var expectedErrorDetailsWithFormattedValues = expectedErrorDetails.GetFormattedValidationMessage(propertyName, getValueForErrorDetails(item));
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
