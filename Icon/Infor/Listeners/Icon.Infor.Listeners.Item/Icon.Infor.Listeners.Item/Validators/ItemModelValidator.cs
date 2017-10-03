using FluentValidation;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Constants;
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
                .WithErrorCode(ValidationErrors.Codes.InvalidItemId)
                .WithMessage(ValidationErrors.Messages.InvalidItemId);
            RuleFor(i => i.ItemTypeCode)
                .Must(t => ItemTypes.Ids.ContainsKey(t))
                .WithErrorCode(ValidationErrors.Codes.InvalidItemTypeCode)
                .WithMessage(ValidationErrors.Messages.InvalidItemTypeCode);
            RuleFor(i => i.AirChilled)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidAirChilled)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.AlcoholByVolume)
                .Matches(TraitPatterns.AlcoholByVolume)
                .Unless(i => i.AlcoholByVolume == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidAlcoholByVolume)
                .WithMessage(ValidationErrors.Messages.InvalidAlcoholByVolume);
            RuleFor(i => i.AnimalWelfareRating)
                .Must(r => animalWelfareRatingDescriptions.Contains(r))
                .Unless(i => i.AnimalWelfareRating == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidAnimalWelfareRating)
                .WithMessage(ValidationErrors.Messages.InvalidAnimalWelfareRating);
            RuleFor(i => i.Biodynamic)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidBiodynamic)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.BrandsHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidBrand)
                .WithMessage(ValidationErrors.Messages.InvalidBrand)
                .Matches(CustomRegexPatterns.NumericHierarchyClassId)
                .WithErrorCode(ValidationErrors.Codes.InvalidBrand)
                .WithMessage(ValidationErrors.Messages.InvalidBrand);
            RuleFor(i => i.CaseinFree)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidCaseinFree)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.CheeseMilkType)
                .Must(v => MilkTypes.Descriptions.AsArray.Contains(v))
                .Unless(i => i.CheeseMilkType == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidCheeseMilkType)
                .WithMessage(ValidationErrors.Messages.InvalidCheeseMilkType);
            RuleFor(i => i.CheeseRaw)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidCheeseRaw)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.DeliverySystem)
                .Must(v => DeliverySystems.Descriptions.AsArray.Contains(v))
                .Unless(i => i.DeliverySystem == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidDeliverySystem)
                .WithMessage(ValidationErrors.Messages.InvalidDeliverySystem);
            RuleFor(i => i.ContainesDuplicateMerchandiseClass)
                .Equal(false)
                .WithErrorCode(ValidationErrors.Codes.DuplicateMerchandiseHierarchyClass)
                .WithMessage(ValidationErrors.Messages.DuplicateMerchandiseHierarchyClass);
            RuleFor(i => i.ContainesDuplicateNationalClass)
                .Equal(false)
                .WithErrorCode(ValidationErrors.Codes.DuplicateNationalHierarchyClass)
                .WithMessage(ValidationErrors.Messages.DuplicateNationalHierarchyClass);
            RuleFor(i => i.DrainedWeight)
                .Matches(TraitPatterns.DrainedWeight)
                .Unless(i => i.DrainedWeight == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidDrainedWeight)
                .WithMessage(ValidationErrors.Messages.InvalidDrainedWeight);
            RuleFor(i => i.DrainedWeightUom)
                .Matches(TraitPatterns.DrainedWeightUom)
                .Unless(i => i.DrainedWeightUom == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidDrainedWeightUom)
                .WithMessage(ValidationErrors.Messages.InvalidDrainedWeightUom);
            RuleFor(i => i.DryAged)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidDryAged)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.EcoScaleRating)
                .Matches(TraitPatterns.EcoScaleRating)
                .Unless(i => i.EcoScaleRating == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidEcoScaleRating)
                .WithMessage(ValidationErrors.Messages.InvalidEcoScaleRating);
            RuleFor(i => i.FairTradeCertified)
                .Matches(TraitPatterns.FairTradeCertified)
                .Unless(i => i.FairTradeCertified == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidFairTradeCertified)
                .WithMessage(ValidationErrors.Messages.InvalidFairTradeCertified);
            RuleFor(i => i.FinancialHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidFinancialHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidFinancialHierarchyClassId)
                .Matches(CustomRegexPatterns.SubTeamNumber)
                .WithErrorCode(ValidationErrors.Codes.InvalidFinancialHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidFinancialHierarchyClassId);
            RuleFor(i => i.FoodStampEligible)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidFoodStampEligible)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidFoodStampEligible)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.FreeRange)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidFreeRange)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.FreshOrFrozen)
                .Must(v => SeafoodFreshOrFrozenTypes.Descriptions.AsArray.Contains(v))
                .Unless(i => i.FreshOrFrozen == string.Empty)
                .WithErrorCode(ValidationErrors.Codes.InvalidFreshOrFrozen)
                .WithMessage(ValidationErrors.Messages.InvalidFreshOrFrozen);
            RuleFor(i => i.GlutenFree)
                .Length(0, 255)
                .WithErrorCode(ValidationErrors.Codes.InvalidGlutenFree)
                .WithMessage(ValidationErrors.Messages.InvalidCertificationAgency);
            RuleFor(i => i.GrassFed)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidGrassFed)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.Hemp)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidHemp)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.HiddenItem)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidHiddenItem)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.InforMessageId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidInforMessageId)
                .WithMessage(ValidationErrors.Messages.InvalidInforMessageId);
            RuleFor(i => i.Kosher)
                .Length(0, 255)
                .WithErrorCode(ValidationErrors.Codes.InvalidKosher)
                .WithMessage(ValidationErrors.Messages.InvalidCertificationAgency);
            RuleFor(i => i.LocalLoanProducer)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidLocalLoanProducer)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.MadeInHouse)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidMadeInHouse)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.MainProductName)
                .Matches(TraitPatterns.MainProductName)
                .WithErrorCode(ValidationErrors.Codes.InvalidMainProductName)
                .WithMessage(ValidationErrors.Messages.InvalidMainProductName);
            RuleFor(i => i.MerchandiseHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidMerchandiseHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidMerchandiseHierarchyClassId)
                .Matches(CustomRegexPatterns.NumericHierarchyClassId)
                .WithErrorCode(ValidationErrors.Codes.InvalidMerchandiseHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidMerchandiseHierarchyClassId);
            RuleFor(i => i.ModifiedDate)
                .Must(v => IsDateTime(v))
                .WithErrorCode(ValidationErrors.Codes.InvalidModifiedDate)
                .WithMessage(ValidationErrors.Messages.InvalidModifiedDate);
            RuleFor(i => i.ModifiedUser)
                .Length(0, ItemValues.MaxPropertyStringLength)
                .WithErrorCode(ValidationErrors.Codes.InvalidModifiedUser)
                .WithMessage(ValidationErrors.Messages.InvalidModifiedUser);
            RuleFor(i => i.Msc)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidMsc)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.NationalHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidNationalHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidNationalHierarchyClassId)
                .Matches(CustomRegexPatterns.NumericHierarchyClassId)
                .WithErrorCode(ValidationErrors.Codes.InvalidNationalHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidNationalHierarchyClassId);
            RuleFor(i => i.NonGmo)
                .Length(0, ItemValues.MaxPropertyStringLength)
                .WithErrorCode(ValidationErrors.Codes.InvalidNonGmo)
                .WithMessage(ValidationErrors.Messages.InvalidCertificationAgency);
            RuleFor(i => i.Notes)
                .Length(0, ItemValues.MaxPropertyStringLength)
                .WithErrorCode(ValidationErrors.Codes.InvalidNotes)
                .WithMessage(ValidationErrors.Messages.InvalidNotes);
            RuleFor(i => i.NutritionRequired)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidNutritionRequired)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.Organic)
                .Length(0, ItemValues.MaxPropertyStringLength)
                .WithErrorCode(ValidationErrors.Codes.InvalidOrganic)
                .WithMessage(ValidationErrors.Messages.InvalidCertificationAgency);
            RuleFor(i => i.OrganicPersonalCare)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidOrganicPersonalCare)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.PackageUnit)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidPackageUnit)
                .WithMessage(ValidationErrors.Messages.InvalidPackageUnit)
                .Matches(TraitPatterns.PackageUnit)
                .WithErrorCode(ValidationErrors.Codes.InvalidPackageUnit)
                .WithMessage(ValidationErrors.Messages.InvalidPackageUnit);
            RuleFor(i => i.Paleo)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidPaleo)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.PastureRaised)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidPastureRaised)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.PosDescription)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidPosDescription)
                .WithMessage(ValidationErrors.Messages.InvalidPosDescription)
                .Matches(TraitPatterns.PosDescription)
                .WithErrorCode(ValidationErrors.Codes.InvalidPosDescription)
                .WithMessage(ValidationErrors.Messages.InvalidPosDescription);
            RuleFor(i => i.PosScaleTare)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidPosScaleTare)
                .WithMessage(ValidationErrors.Messages.InvalidPosScaleTare)
                .Matches(TraitPatterns.PosScaleTare)
                .WithErrorCode(ValidationErrors.Codes.InvalidPosScaleTare)
                .WithMessage(ValidationErrors.Messages.InvalidPosScaleTare);
            RuleFor(i => i.PremiumBodyCare)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidPremiumBodyCare)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.ProductDescription)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidProductDescription)
                .WithMessage(ValidationErrors.Messages.InvalidProductDescription)
                .Matches(TraitPatterns.ProductDescription)
                .WithErrorCode(ValidationErrors.Codes.InvalidProductDescription)
                .WithMessage(ValidationErrors.Messages.InvalidProductDescription);
            RuleFor(i => i.CustomerFriendlyDescription)
                .Matches(TraitPatterns.CustomerFriendlyDescription)
                .WithErrorCode(ValidationErrors.Codes.InvalidCustomerFriendlyDescription)
                .WithMessage(ValidationErrors.Messages.InvalidCustomerFriendlyDescription);
            RuleFor(i => i.ProductFlavorType)
                .Matches(TraitPatterns.ProductFlavorType)
                .WithErrorCode(ValidationErrors.Codes.InvalidProductFlavorType)
                .WithMessage(ValidationErrors.Messages.InvalidProductFlavorType);
            RuleFor(i => i.ProhibitDiscount)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidProhibitDiscount)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidProhibitDiscount)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.RetailSize)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidRetailSize)
                .WithMessage(ValidationErrors.Messages.InvalidRetailSize)
                .Must(v => IsValidRetailSize(v))
                .WithErrorCode(ValidationErrors.Codes.InvalidRetailSize)
                .WithMessage(ValidationErrors.Messages.InvalidRetailSize);
            RuleFor(i => i.RetailUom)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidRetailUom)
                .WithMessage(ValidationErrors.Messages.InvalidRetailUom)
                .Must(i => UomCodes.ByName.Values.Contains(i))
                .WithErrorCode(ValidationErrors.Codes.InvalidRetailUom)
                .WithMessage(ValidationErrors.Messages.InvalidRetailUom);
            RuleFor(i => i.ScanCode)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidScanCode)
                .WithMessage(ValidationErrors.Messages.InvalidScanCode)
                .Must(v => !v.StartsWith("0"))
                .WithErrorCode(ValidationErrors.Codes.InvalidScanCode)
                .WithMessage(ValidationErrors.Messages.InvalidScanCode)
                .Matches(CustomRegexPatterns.ScanCode)
                .WithErrorCode(ValidationErrors.Codes.InvalidScanCode)
                .WithMessage(ValidationErrors.Messages.InvalidScanCode);
            RuleFor(i => i.ScanCodeType)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidScanCodeType)
                .WithMessage(ValidationErrors.Messages.InvalidScanCodeType)
                .Must(v => ScanCodeTypes.Descriptions.AsArray.Contains(v))
                .WithErrorCode(ValidationErrors.Codes.InvalidScanCodeType)
                .WithMessage(ValidationErrors.Messages.InvalidScanCodeType);
            RuleFor(i => i.SeafoodCatchType)
                .Must(v => SeafoodCatchTypes.Descriptions.AsArray.Contains(v))
                .Unless(i => string.IsNullOrEmpty(i.SeafoodCatchType))
                .WithErrorCode(ValidationErrors.Codes.InvalidSeafoodCatchType)
                .WithMessage(ValidationErrors.Messages.InvalidSeafoodCatchType);
            RuleFor(i => i.TaxHierarchyClassId)
                .NotEmpty()
                .WithErrorCode(ValidationErrors.Codes.InvalidTaxHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidTaxHierarchyClassId)
                .Matches(CustomRegexPatterns.TaxCode)
                .WithErrorCode(ValidationErrors.Codes.InvalidTaxHierarchyClassId)
                .WithMessage(ValidationErrors.Messages.InvalidTaxHierarchyClassId);
            RuleFor(i => i.Vegan)
                .Length(0, ItemValues.MaxPropertyStringLength)
                .WithErrorCode(ValidationErrors.Codes.InvalidVegan)
                .WithMessage(ValidationErrors.Messages.InvalidCertificationAgency);
            RuleFor(i => i.Vegetarian)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidVegetarian)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);
            RuleFor(i => i.WholeTrade)
                .Matches(CustomRegexPatterns.BooleanValue)
                .WithErrorCode(ValidationErrors.Codes.InvalidWholeTrade)
                .WithMessage(ValidationErrors.Messages.InvalidBooleanString);

            this.settings = settings;
            this.getItemValidationPropertiesQueryHandler = getItemValidationPropertiesQueryHandler;
        }

        private bool IsValidRetailSize(string retailSize)
        {
            decimal decimalValue;
            bool isDecimal = decimal.TryParse(retailSize, NumberStyles.Float, CultureInfo.InvariantCulture, out decimalValue);

            if (isDecimal && decimalValue > 0 && decimalValue < ItemValues.RetailSizeMax)
            {
                if (!retailSize.Contains('.') || (retailSize.Contains('.') && GetNumberOfDigitsPastTheDecimalPoint(retailSize) <= ItemValues.RetailSizeScaleMax))
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
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.NonExistentBrand;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.NonExistentBrand, joinedItem.Item.BrandsHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.SubTeamId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.NonExistentSubTeam;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.NonExistentSubTeam, joinedItem.Item.FinancialHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.SubBrickId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.NonExistentSubBrick;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.NonExistentSubBrick, joinedItem.Item.MerchandiseHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.NationalClassId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.NonExistentNationalClass;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.NonExistentNationalClass, joinedItem.Item.NationalHierarchyClassId);
                }
                else if (!joinedItem.ValidationProperties.TaxClassId.HasValue)
                {
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.NonExistentTax;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.NonExistentTax, joinedItem.Item.TaxHierarchyClassId);
                }
                else if (!string.IsNullOrWhiteSpace(joinedItem.ValidationProperties.ModifiedDate) && IsMessageModifiedDateLessThanCurrentModifiedDate(joinedItem.Item.ModifiedDate, joinedItem.ValidationProperties.ModifiedDate))
                {
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.OutOfSyncItemUpdateErrorCode;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.OutOfSyncItemUpdateErrorCode, joinedItem.Item.ModifiedDate, joinedItem.ValidationProperties.ModifiedDate);
                }
                else if(!IsSequenceIdValid(joinedItem.Item.SequenceId, joinedItem.ValidationProperties.SequenceId))
                {
                    joinedItem.Item.ErrorCode = ValidationErrors.Codes.OutOfSyncItemUpdateErrorCode;
                    joinedItem.Item.ErrorDetails = string.Format(ValidationErrors.Messages.OutOfSyncItemUpdateSequenceIdErrorCode, joinedItem.Item.SequenceId, joinedItem.ValidationProperties.SequenceId);
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
