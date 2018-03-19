using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Constants;
using Icon.Infor.Listeners.Item.Constants.ItemValidation;
using Icon.Infor.Listeners.Item.Models;
using Icon.Infor.Listeners.Item.Queries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Icon.Infor.Listeners.Item.Validators
{
    public class ItemModelValidator : AbstractValidator<ItemModel>, ICollectionValidator<ItemModel>
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
        private string[] animalWelfareRatingDescriptions = AnimalWelfareRatings.Descriptions.AsArray;

        private ItemListenerSettings settings;
        private IQueryHandler<GetItemValidationPropertiesParameters, List<GetItemValidationPropertiesResultModel>> getItemValidationPropertiesQueryHandler;

        public ItemModelValidator(
            ItemListenerSettings settings,
            IQueryHandler<GetItemValidationPropertiesParameters, List<GetItemValidationPropertiesResultModel>> getItemValidationPropertiesQueryHandler)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(i => i.ItemId)
                .Must(i => i > -1)
                .WithErrorCode(ValidationErrorCodes.InvalidItemId)
                .WithMessage(ValidationErrorMessages.InvalidItemId);
            RuleFor(i => i.ItemTypeCode)
                .Must(t => ItemTypes.Ids.ContainsKey(t))
                .WithErrorCode(ValidationErrorCodes.InvalidItemTypeCode)
                .WithMessage(ValidationErrorMessages.InvalidItemTypeCode);
            RuleFor(i => i.AirChilled)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidAirChilled)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.AlcoholByVolume)
                .Matches(TraitPatterns.AlcoholByVolume)
                .Unless(i => i.AlcoholByVolume == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidAlcoholByVolume)
                .WithMessage(ValidationErrorMessages.InvalidAlcoholByVolume);
            RuleFor(i => i.AnimalWelfareRating)
                .Must(r => animalWelfareRatingDescriptions.Contains(r))
                .Unless(i => i.AnimalWelfareRating == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidAnimalWelfareRating)
                .WithMessage(ValidationErrorMessages.InvalidAnimalWelfareRating);
            RuleFor(i => i.Biodynamic)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidBiodynamic)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.BrandsHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidBrand)
                .WithMessage(ValidationErrorMessages.InvalidBrand)
                .Matches(RegularExpressions.NumericHierarchyClassId)
                .WithErrorCode(ValidationErrorCodes.InvalidBrand)
                .WithMessage(ValidationErrorMessages.InvalidBrand);
            RuleFor(i => i.CaseinFree)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidCaseinFree)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.CheeseMilkType)
                .Must(v => MilkTypes.Descriptions.AsArray.Contains(v))
                .Unless(i => i.CheeseMilkType == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidCheeseMilkType)
                .WithMessage(ValidationErrorMessages.InvalidCheeseMilkType);
            RuleFor(i => i.CheeseRaw)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidCheeseRaw)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.DeliverySystem)
                .Must(v => DeliverySystems.Descriptions.AsArray.Contains(v))
                .Unless(i => i.DeliverySystem == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidDeliverySystem)
                .WithMessage(ValidationErrorMessages.InvalidDeliverySystem);
            RuleFor(i => i.ContainesDuplicateMerchandiseClass)
                .Equal(false)
                .WithErrorCode(ValidationErrorCodes.DuplicateMerchandiseHierarchyClass)
                .WithMessage(ValidationErrorMessages.DuplicateMerchandiseHierarchyClass);
            RuleFor(i => i.ContainesDuplicateNationalClass)
                .Equal(false)
                .WithErrorCode(ValidationErrorCodes.DuplicateNationalHierarchyClass)
                .WithMessage(ValidationErrorMessages.DuplicateNationalHierarchyClass);
            RuleFor(i => i.DrainedWeight)
                .Matches(TraitPatterns.DrainedWeight)
                .Unless(i => i.DrainedWeight == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidDrainedWeight)
                .WithMessage(ValidationErrorMessages.InvalidDrainedWeight);
            RuleFor(i => i.DrainedWeightUom)
                .Matches(TraitPatterns.DrainedWeightUom)
                .Unless(i => i.DrainedWeightUom == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidDrainedWeightUom)
                .WithMessage(ValidationErrorMessages.InvalidDrainedWeightUom);
            RuleFor(i => i.DryAged)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidDryAged)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.EcoScaleRating)
                .Matches(TraitPatterns.EcoScaleRating)
                .Unless(i => i.EcoScaleRating == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidEcoScaleRating)
                .WithMessage(ValidationErrorMessages.InvalidEcoScaleRating);
            RuleFor(i => i.FairTradeCertified)
                .Matches(TraitPatterns.FairTradeCertified)
                .Unless(i => i.FairTradeCertified == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidFairTradeCertified)
                .WithMessage(ValidationErrorMessages.InvalidFairTradeCertified);
            RuleFor(i => i.FinancialHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidFinancialHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidFinancialHierarchyClassId)
                .Matches(RegularExpressions.SubTeamNumber)
                .WithErrorCode(ValidationErrorCodes.InvalidFinancialHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidFinancialHierarchyClassId);
            RuleFor(i => i.FoodStampEligible)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidFoodStampEligible)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidFoodStampEligible)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.FreeRange)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidFreeRange)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.FreshOrFrozen)
                .Must(v => SeafoodFreshOrFrozenTypes.Descriptions.AsArray.Contains(v))
                .Unless(i => i.FreshOrFrozen == string.Empty)
                .WithErrorCode(ValidationErrorCodes.InvalidFreshOrFrozen)
                .WithMessage(ValidationErrorMessages.InvalidFreshOrFrozen);
            RuleFor(i => i.GlutenFree)
                .Length(0, 255)
                .WithErrorCode(ValidationErrorCodes.InvalidGlutenFree)
                .WithMessage(ValidationErrorMessages.InvalidCertificationAgency);
            RuleFor(i => i.GrassFed)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidGrassFed)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.Hemp)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidHemp)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.HiddenItem)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidHiddenItem)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.InforMessageId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidInforMessageId)
                .WithMessage(ValidationErrorMessages.InvalidInforMessageId);
            RuleFor(i => i.Kosher)
                .Length(0, 255)
                .WithErrorCode(ValidationErrorCodes.InvalidKosher)
                .WithMessage(ValidationErrorMessages.InvalidCertificationAgency);
            RuleFor(i => i.LocalLoanProducer)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidLocalLoanProducer)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.MadeInHouse)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidMadeInHouse)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.MainProductName)
                .Matches(TraitPatterns.MainProductName)
                .WithErrorCode(ValidationErrorCodes.InvalidMainProductName)
                .WithMessage(ValidationErrorMessages.InvalidMainProductName);
            RuleFor(i => i.MerchandiseHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidMerchandiseHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidMerchandiseHierarchyClassId)
                .Matches(RegularExpressions.NumericHierarchyClassId)
                .WithErrorCode(ValidationErrorCodes.InvalidMerchandiseHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidMerchandiseHierarchyClassId);
            RuleFor(i => i.ModifiedDate)
                .Must(v => IsDateTime(v))
                .WithErrorCode(ValidationErrorCodes.InvalidModifiedDate)
                .WithMessage(ValidationErrorMessages.InvalidModifiedDate);
            RuleFor(i => i.ModifiedUser)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidModifiedUser)
                .WithMessage(ValidationErrorMessages.InvalidModifiedUser);
            RuleFor(i => i.Msc)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidMsc)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.NationalHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidNationalHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidNationalHierarchyClassId)
                .Matches(RegularExpressions.NumericHierarchyClassId)
                .WithErrorCode(ValidationErrorCodes.InvalidNationalHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidNationalHierarchyClassId);
            RuleFor(i => i.NonGmo)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidNonGmo)
                .WithMessage(ValidationErrorMessages.InvalidCertificationAgency);
            RuleFor(i => i.Notes)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidNotes)
                .WithMessage(ValidationErrorMessages.InvalidNotes);
            RuleFor(i => i.NutritionRequired)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidNutritionRequired)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.Organic)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidOrganic)
                .WithMessage(ValidationErrorMessages.InvalidCertificationAgency);
            RuleFor(i => i.OrganicPersonalCare)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidOrganicPersonalCare)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.PackageUnit)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidPackageUnit)
                .WithMessage(ValidationErrorMessages.InvalidPackageUnit)
                .Matches(TraitPatterns.PackageUnit)
                .WithErrorCode(ValidationErrorCodes.InvalidPackageUnit)
                .WithMessage(ValidationErrorMessages.InvalidPackageUnit);
            RuleFor(i => i.Paleo)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidPaleo)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.PastureRaised)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidPastureRaised)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.PosDescription)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidPosDescription)
                .WithMessage(ValidationErrorMessages.InvalidPosDescription)
                .Matches(TraitPatterns.PosDescription)
                .WithErrorCode(ValidationErrorCodes.InvalidPosDescription)
                .WithMessage(ValidationErrorMessages.InvalidPosDescription);
            RuleFor(i => i.PosScaleTare)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidPosScaleTare)
                .WithMessage(ValidationErrorMessages.InvalidPosScaleTare)
                .Matches(TraitPatterns.PosScaleTare)
                .WithErrorCode(ValidationErrorCodes.InvalidPosScaleTare)
                .WithMessage(ValidationErrorMessages.InvalidPosScaleTare);
            RuleFor(i => i.PremiumBodyCare)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidPremiumBodyCare)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.ProductDescription)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidProductDescription)
                .WithMessage(ValidationErrorMessages.InvalidProductDescription)
                .Matches(TraitPatterns.ProductDescription)
                .WithErrorCode(ValidationErrorCodes.InvalidProductDescription)
                .WithMessage(ValidationErrorMessages.InvalidProductDescription);
            RuleFor(i => i.CustomerFriendlyDescription)
                .Matches(TraitPatterns.CustomerFriendlyDescription)
                .WithErrorCode(ValidationErrorCodes.InvalidCustomerFriendlyDescription)
                .WithMessage(ValidationErrorMessages.InvalidCustomerFriendlyDescription);
            RuleFor(i => i.ProductFlavorType)
                .Matches(TraitPatterns.ProductFlavorType)
                .WithErrorCode(ValidationErrorCodes.InvalidProductFlavorType)
                .WithMessage(ValidationErrorMessages.InvalidProductFlavorType);
            RuleFor(i => i.ProhibitDiscount)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidProhibitDiscount)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidProhibitDiscount)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.RetailSize)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidRetailSize)
                .WithMessage(ValidationErrorMessages.InvalidRetailSize)
                .Must(v => IsValidRetailSize(v))
                .WithErrorCode(ValidationErrorCodes.InvalidRetailSize)
                .WithMessage(ValidationErrorMessages.InvalidRetailSize);
            RuleFor(i => i.RetailUom)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidRetailUom)
                .WithMessage(ValidationErrorMessages.InvalidRetailUom)
                .Must(i => UomCodes.ByName.Values.Contains(i))
                .WithErrorCode(ValidationErrorCodes.InvalidRetailUom)
                .WithMessage(ValidationErrorMessages.InvalidRetailUom);
            RuleFor(i => i.ScanCode)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidScanCode)
                .WithMessage(ValidationErrorMessages.InvalidScanCode)
                .Must(v => !v.StartsWith("0"))
                .WithErrorCode(ValidationErrorCodes.InvalidScanCode)
                .WithMessage(ValidationErrorMessages.InvalidScanCode)
                .Matches(RegularExpressions.ScanCode)
                .WithErrorCode(ValidationErrorCodes.InvalidScanCode)
                .WithMessage(ValidationErrorMessages.InvalidScanCode);
            RuleFor(i => i.ScanCodeType)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidScanCodeType)
                .WithMessage(ValidationErrorMessages.InvalidScanCodeType)
                .Must(v => ScanCodeTypes.Descriptions.AsArray.Contains(v))
                .WithErrorCode(ValidationErrorCodes.InvalidScanCodeType)
                .WithMessage(ValidationErrorMessages.InvalidScanCodeType);
            RuleFor(i => i.SeafoodCatchType)
                .Must(v => SeafoodCatchTypes.Descriptions.AsArray.Contains(v))
                .Unless(i => string.IsNullOrEmpty(i.SeafoodCatchType))
                .WithErrorCode(ValidationErrorCodes.InvalidSeafoodCatchType)
                .WithMessage(ValidationErrorMessages.InvalidSeafoodCatchType);
            RuleFor(i => i.TaxHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCodes.InvalidTaxHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidTaxHierarchyClassId)
                .Matches(RegularExpressions.TaxCode)
                .WithErrorCode(ValidationErrorCodes.InvalidTaxHierarchyClassId)
                .WithMessage(ValidationErrorMessages.InvalidTaxHierarchyClassId);
            RuleFor(i => i.Vegan)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidVegan)
                .WithMessage(ValidationErrorMessages.InvalidCertificationAgency);
            RuleFor(i => i.Vegetarian)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidVegetarian)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.WholeTrade)
                .Matches(RegularExpressions.BooleanZeroOrOne)
                .WithErrorCode(ValidationErrorCodes.InvalidWholeTrade)
                .WithMessage(ValidationErrorMessages.InvalidBooleanStringZeroOrOne);
            RuleFor(i => i.GlobalPricingProgram)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidGlobalPricingProgram)
                .WithMessage(ValidationErrorMessages.InvalidGlobalPricingProgram)
                .Matches(TraitPatterns.GlobalPricingProgram)
                .WithErrorCode(ValidationErrorCodes.InvalidGlobalPricingProgram)
                .WithMessage(ValidationErrorMessages.InvalidGlobalPricingProgram);
            RuleFor(i => i.PercentageTareWeight)
                .Matches(TraitPatterns.PercentageTareWeight)
                .WithErrorCode(ValidationErrorCodes.InvalidPercentageTareWeight)
                .WithMessage(ValidationErrorMessages.InvalidPercentageTareWeight);
            RuleFor(i => i.WicEligible)
                .Matches(TraitPatterns.WicEligible)
                .WithErrorCode(ValidationErrorCodes.InvalidWicEligible)
                .WithMessage(ValidationErrorMessages.InvalidWicEligible);
            RuleFor(i => i.SmithsonianBirdFriendly)
                .Matches(TraitPatterns.SmithsonianBirdFriendly)
                .WithErrorCode(ValidationErrorCodes.InvalidSmithsonianBirdFriendly)
                .WithMessage(ValidationErrorMessages.InvalidSmithsonianBirdFriendly);
            RuleFor(i => i.RainforestAlliance)
                .Matches(TraitPatterns.RainforestAlliance)
                .WithErrorCode(ValidationErrorCodes.InvalidRainforestAlliance)
                .WithMessage(ValidationErrorMessages.InvalidRainforestAlliance);
            RuleFor(i => i.PrimeBeef)
                .Matches(TraitPatterns.PrimeBeef)
                .WithErrorCode(ValidationErrorCodes.InvalidPrimeBeef)
                .WithMessage(ValidationErrorMessages.InvalidPrimeBeef);
            RuleFor(i => i.Refrigerated)
                .Matches(TraitPatterns.Refrigerated)
                .WithErrorCode(ValidationErrorCodes.InvalidRefrigerated)
                .WithMessage(ValidationErrorMessages.InvalidRefrigerated);
            RuleFor(i => i.MadeWithOrganicGrapes)
                .Length(0, MaxLengths.StandardProperty255)
                .WithErrorCode(ValidationErrorCodes.InvalidMadeWithOrganicGrapes)
                .WithMessage(ValidationErrorMessages.InvalidMadeWithOrganicGrapes);
            RuleFor(i => i.ShelfLife)
                .Matches(TraitPatterns.ShelfLife)
                .WithErrorCode(ValidationErrorCodes.InvalidShelfLife)
                .WithMessage(ValidationErrorMessages.InvalidShelfLife);
            RuleFor(i => i.FlexibleText)
                .Length(0, MaxLengths.FlexibleText)
                .WithErrorCode(ValidationErrorCodes.InvalidFlexibleText)
                .WithMessage(ValidationErrorMessages.InvalidFlexibleTextLength)
                .Matches(TraitPatterns.FlexibleText)
                .WithErrorCode(ValidationErrorCodes.InvalidFlexibleText)
                .WithMessage(ValidationErrorMessages.InvalidFlexibleTextPattern);
            RuleFor(i => i.SelfCheckoutItemTareGroup)
                .Length(0, MaxLengths.SelfCheckoutItemTareGroup)
                .WithErrorCode(ValidationErrorCodes.InvalidSelfCheckoutItemTareGroup)
                .WithMessage(ValidationErrorMessages.InvalidSelfCheckoutItemTareGroupLength)
                .Matches(TraitPatterns.SelfCheckoutItemTareGroup)
                .WithErrorCode(ValidationErrorCodes.InvalidSelfCheckoutItemTareGroup)
                .WithMessage(ValidationErrorMessages.InvalidSelfCheckoutItemTareGroupPattern);

            this.settings = settings;
            this.getItemValidationPropertiesQueryHandler = getItemValidationPropertiesQueryHandler;
        }

        private bool IsValidRetailSize(string retailSize)
        {
            decimal decimalValue;
            bool isDecimal = decimal.TryParse(retailSize, NumberStyles.Float, CultureInfo.InvariantCulture, out decimalValue);

            if (isDecimal && decimalValue > 0 && decimalValue < MaxValues.RetailSize)
            {
                if (!retailSize.Contains('.') || (retailSize.Contains('.') && GetNumberOfDigitsPastTheDecimalPoint(retailSize) <= MaxValues.RetailSizeScale))
                {
                    return true;
                }
            }

            return false;
        }

        private int GetNumberOfDigitsPastTheDecimalPoint(string retailSize)
        {
            return retailSize.Split('.').Last().Length;
        }

        private bool IsDateTime(string value)
        {
            DateTime dateTime = new DateTime();

            return DateTime.TryParse(value, out dateTime);
        }

        private bool ValidateScaleTare(string posScaleTare)
        {
            decimal decimalValue;
            bool isDecimal = decimal.TryParse(posScaleTare, NumberStyles.Float, CultureInfo.InvariantCulture, out decimalValue);

            if (isDecimal)
            {
                if(decimalValue < 10 && posScaleTare.Length < 5)
                {
                    return true;
                }
            }

            return false;
        }

        public void ValidateCollection(IEnumerable<ItemModel> collection)
        {
            foreach (var item in collection)
            {
                var result = Validate(item);
                if (!result.IsValid)
                {
                    item.ErrorCode = result.Errors.First().ErrorCode;
                    item.ErrorDetails = result.Errors.First().ErrorMessage;
                }
            }

            ValidateCollectionAgainstDatabase(collection);
        }

        private void ValidateCollectionAgainstDatabase(IEnumerable<ItemModel> collection)
        {
            var itemValidationPropertiesResults = getItemValidationPropertiesQueryHandler.Search(new GetItemValidationPropertiesParameters { Items = collection });

            var joinedItems = collection
                .Where(i => i.ErrorCode == null)
                .Join(itemValidationPropertiesResults,
                    i => i.ItemId,
                    r => r.ItemId,
                    (i, r) => new { Item = i, ValidationProperties = r });

            foreach (var joinedItem in joinedItems)
            {
                if (!joinedItem.ValidationProperties.BrandId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.NonExistentBrand;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.NonExistentBrand, joinedItem.Item.BrandsHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.SubTeamId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.NonExistentSubTeam;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.NonExistentSubTeam, joinedItem.Item.FinancialHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.SubBrickId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.NonExistentSubBrick;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.NonExistentSubBrick, joinedItem.Item.MerchandiseHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.NationalClassId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.NonExistentNationalClass;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.NonExistentNationalClass, joinedItem.Item.NationalHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.TaxClassId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.NonExistentTax;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.NonExistentTax, joinedItem.Item.TaxHierarchyClassId);
                }
                else if (!string.IsNullOrWhiteSpace(joinedItem.ValidationProperties.ModifiedDate) && IsMessageModifiedDateLessThanCurrentModifiedDate(joinedItem.Item.ModifiedDate, joinedItem.ValidationProperties.ModifiedDate))
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.OutOfSyncItemUpdateErrorCode;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.OutOfSyncItemUpdateErrorCode, joinedItem.Item.ModifiedDate, joinedItem.ValidationProperties.ModifiedDate);
                }
                else if(!IsSequenceIdValid(joinedItem.Item.SequenceId, joinedItem.ValidationProperties.SequenceId))
                {
                    joinedItem.Item.ErrorCode = ValidationErrorCodes.OutOfSyncItemUpdateErrorCode;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrorMessages.OutOfSyncItemUpdateSequenceIdErrorCode, joinedItem.Item.SequenceId, joinedItem.ValidationProperties.SequenceId);
                }
            }
        }

        private bool IsMessageModifiedDateLessThanCurrentModifiedDate(string messageModifiedDate, string currentModifiedDate)
        {
            var parsedMessageModifiedDate = DateTime.Parse(messageModifiedDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var parsedMessageModifiedDateNanoseconds = parsedMessageModifiedDate.Ticks % TimeSpan.TicksPerMillisecond;

            var parsedCurrentModifiedDate = DateTime.Parse(currentModifiedDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var parsedCurrentModifiedDateNanoseconds = parsedCurrentModifiedDate.Ticks % TimeSpan.TicksPerMillisecond;

            //Removing the nanoseconds from modified date because we only want to compare up to the number of milliseconds
            parsedMessageModifiedDate = parsedMessageModifiedDate.AddTicks(-parsedMessageModifiedDateNanoseconds);
            parsedCurrentModifiedDate = parsedCurrentModifiedDate.AddTicks(-parsedCurrentModifiedDateNanoseconds);

            return parsedMessageModifiedDate < parsedCurrentModifiedDate;
        }

        private bool IsSequenceIdValid(decimal? newSequenceId, decimal? currentSequenceId)
        {
            if(settings.ValidateSequenceId)
            {
                return !currentSequenceId.HasValue || (newSequenceId.Value >= currentSequenceId.Value);
            }
            else
            {
                return true;
            }
        }
    }
}