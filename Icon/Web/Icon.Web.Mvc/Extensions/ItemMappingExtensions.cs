﻿using Icon.Framework;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Extensions
{
    public static class ItemMappingExtensions
    {
        public static BulkImportItemModel ToBulkImportModel(this ItemViewModel viewModel)
        {
            return new BulkImportItemModel
            {
                ItemId = viewModel.ItemId,
                ScanCode = viewModel.ScanCode,
                BrandId = viewModel.BrandHierarchyClassId.ToString(),
                ProductDescription = viewModel.ProductDescription?.Trim(),
                PosDescription = viewModel.PosDescription?.Trim(),
                PackageUnit = viewModel.PackageUnit,
                RetailSize = viewModel.RetailSize,
                RetailUom = viewModel.RetailUom,
                DeliverySystem = viewModel.DeliverySystem,
                FoodStampEligible = ConversionUtility.ConvertToItemTraitDbValue(viewModel.FoodStampEligible),
                PosScaleTare = viewModel.PosScaleTare,
                MerchandiseId = viewModel.MerchandiseHierarchyClassId?.ToString(),
                TaxId = viewModel.TaxHierarchyClassId?.ToString(),
                BrowsingId = viewModel.BrowsingHierarchyClassId?.ToString(),
                IsValidated = ConversionUtility.ConvertToItemTraitDbValue(viewModel.IsValidated),
                DepartmentSale = ConversionUtility.ConvertToItemTraitDbValue(viewModel.DepartmentSale),
                HiddenItem = ConversionUtility.ConvertToItemTraitDbValue(viewModel.HiddenItem),
                NationalId = viewModel.NationalHierarchyClassId?.ToString(),
                Notes = viewModel.Notes,
                AnimalWelfareRatingId = viewModel.AnimalWelfareRatingId?.ToString(),
                Biodynamic = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Biodynamic),
                CheeseAttributeMilkTypeId = viewModel.CheeseMilkTypeId?.ToString(),
                CheeseAttributeRaw = ConversionUtility.ConvertToItemTraitDbValue(viewModel.CheeseRaw),
                EcoScaleRatingId = viewModel.EcoScaleRatingId?.ToString(),
                GlutenFreeAgency = viewModel.GlutenFreeAgency,
                KosherAgency = viewModel.KosherAgency,
                Msc = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Msc),
                NonGmoAgency = viewModel.NonGmoAgency,
                OrganicAgency = viewModel.OrganicAgency,
                PremiumBodyCare = ConversionUtility.ConvertToItemTraitDbValue(viewModel.PremiumBodyCare),
                SeafoodFreshOrFrozenId = viewModel.SeafoodFreshOrFrozenId?.ToString(),
                SeafoodWildOrFarmRaisedId = viewModel.SeafoodCatchTypeId?.ToString(),
                VeganAgency = viewModel.VeganAgency,
                Vegetarian = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Vegetarian),
                WholeTrade = ConversionUtility.ConvertToItemTraitDbValue(viewModel.WholeTrade),
                GrassFed = ConversionUtility.ConvertToItemTraitDbValue(viewModel.GrassFed),
                PastureRaised = ConversionUtility.ConvertToItemTraitDbValue(viewModel.PastureRaised),
                FreeRange = ConversionUtility.ConvertToItemTraitDbValue(viewModel.FreeRange),
                DryAged = ConversionUtility.ConvertToItemTraitDbValue(viewModel.DryAged),
                AirChilled = ConversionUtility.ConvertToItemTraitDbValue(viewModel.AirChilled),
                MadeInHouse = ConversionUtility.ConvertToItemTraitDbValue(viewModel.MadeInHouse),
                AlcoholByVolume = ConversionUtility.ConvertToItemTraitDbValue(viewModel.AlcoholByVolume),
                CaseinFree = ConversionUtility.ConvertToItemTraitDbValue(viewModel.CaseinFree),
                DrainedWeight = ConversionUtility.ConvertToItemTraitDbValue(viewModel.DrainedWeight),
                DrainedWeightUom = viewModel.DrainedWeightUom,
                FairTradeCertified = viewModel.FairTradeCertified,
                Hemp = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Hemp),
                LocalLoanProducer = ConversionUtility.ConvertToItemTraitDbValue(viewModel.LocalLoanProducer),
                MainProductName = string.IsNullOrWhiteSpace(viewModel.MainProductName) ? null : viewModel.MainProductName,
                NutritionRequired = ConversionUtility.ConvertToItemTraitDbValue(viewModel.NutritionRequired),
                OrganicPersonalCare = ConversionUtility.ConvertToItemTraitDbValue(viewModel.OrganicPersonalCare),
                Paleo = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Paleo),
                ProductFlavorType = string.IsNullOrWhiteSpace(viewModel.ProductFlavorType) ? null : viewModel.ProductFlavorType
            };
        }

        public static BulkImportItemModel ToBulkImportModel(this ItemEditViewModel viewModel, int brandId)
        {
            return new BulkImportItemModel
            {
                ItemId = viewModel.ItemId,
                ScanCode = viewModel.ScanCode,
                BrandId = brandId.ToString(),
                ProductDescription = viewModel.ProductDescription?.Trim(),
                PosDescription = viewModel.PosDescription?.Trim(),
                PackageUnit = viewModel.PackageUnit,
                RetailSize = viewModel.RetailSize,
                RetailUom = viewModel.RetailUom,
                DeliverySystem = viewModel.DeliverySystem,
                FoodStampEligible = ConversionUtility.ConvertToItemTraitDbValue(viewModel.FoodStampEligible),
                PosScaleTare = viewModel.PosScaleTare,
                MerchandiseId = viewModel.MerchandiseHierarchyClassSelectedId?.ToString(),
                TaxId = viewModel.TaxHierarchyClassSelectedId?.ToString(),
                BrowsingId = viewModel.BrowsingHierarchyClassSelectedId?.ToString(),
                IsValidated = ConversionUtility.ConvertToItemTraitDbValue(viewModel.IsValidated),
                DepartmentSale = ConversionUtility.ConvertToItemTraitDbValue(viewModel.DepartmentSale),
                HiddenItem = ConversionUtility.ConvertToItemTraitDbValue(viewModel.HiddenItem),
                NationalId = viewModel.NationalHierarchyClassSelectedId?.ToString(),
                Notes = viewModel.Notes,
                AnimalWelfareRatingId = viewModel.SelectedAnimalWelfareRatingId?.ToString(),
                Biodynamic = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedBiodynamicOption),
                CheeseAttributeMilkTypeId = viewModel.SelectedCheeseMilkTypeId?.ToString(),
                CheeseAttributeRaw = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedCheeseRawOption),
                EcoScaleRatingId = viewModel.SelectedEcoScaleRatingId?.ToString(),
                GlutenFreeAgency = viewModel.GlutenFreeAgency,
                KosherAgency = viewModel.KosherAgency,
                Msc = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedMscOption),
                NonGmoAgency = viewModel.NonGmoAgency,
                OrganicAgency = viewModel.OrganicAgency,
                PremiumBodyCare = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedPremiumBodyCareOption),
                SeafoodFreshOrFrozenId = viewModel.SelectedSeafoodFreshOrFrozenId?.ToString(),
                SeafoodWildOrFarmRaisedId = viewModel.SelectedSeafoodCatchTypeId?.ToString(),
                VeganAgency = viewModel.VeganAgency,
                Vegetarian = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedVegetarianOption),
                WholeTrade = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedWholeTradeOption),
                GrassFed = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedGrassFedOption),
                PastureRaised = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedPastureRaisedOption),
                FreeRange = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedFreeRangeOption),
                DryAged = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedDryAgedOption),
                AirChilled = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedAirChilledOption),
                MadeInHouse = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedMadeInHouseOption),
                AlcoholByVolume = viewModel.AlcoholByVolume,
                CaseinFree = ConversionUtility.ConvertToItemTraitDbValue(viewModel.CaseinFree),
                DrainedWeight = viewModel.DrainedWeight,
                DrainedWeightUom = viewModel.DrainedWeightUom,
                FairTradeCertified = viewModel.FairTradeCertified,
                Hemp = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Hemp),
                LocalLoanProducer = ConversionUtility.ConvertToItemTraitDbValue(viewModel.LocalLoanProducer),
                MainProductName = string.IsNullOrWhiteSpace(viewModel.MainProductName) ? null : viewModel.MainProductName,
                NutritionRequired = ConversionUtility.ConvertToItemTraitDbValue(viewModel.NutritionRequired),
                OrganicPersonalCare = ConversionUtility.ConvertToItemTraitDbValue(viewModel.OrganicPersonalCare),
                Paleo = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Paleo),
                ProductFlavorType = string.IsNullOrWhiteSpace(viewModel.ProductFlavorType) ? null : viewModel.ProductFlavorType
            };
        }

        public static BulkImportNewItemModel ToBulkImportNewItemModel(this ItemCreateViewModel viewModel)
        {
            return new BulkImportNewItemModel
            {
                ScanCode = viewModel.ScanCode.Trim().TrimStart('0'),
                BrandId = "0",
                BrandName = viewModel.BrandName,
                ProductDescription = viewModel.ProductDescription.Trim(),
                PosDescription = viewModel.PosDescription.TrimStart(),
                PackageUnit = viewModel.PackageUnit,
                RetailSize = viewModel.RetailSize,
                RetailUom = viewModel.RetailUom,
                DeliverySystem = viewModel.DeliverySystem,
                FoodStampEligible = viewModel.FoodStampEligible ? "1" : "0",
                PosScaleTare = viewModel.PosScaleTare,
                MerchandiseId = viewModel.MerchandiseHierarchyClassId == null ? String.Empty : viewModel.MerchandiseHierarchyClassId.ToString(),
                TaxId = viewModel.TaxHierarchyClassId == null ? String.Empty : viewModel.TaxHierarchyClassId.ToString(),
                TaxLineage = viewModel.TaxHierarchyClassName,
                BrowsingId = viewModel.BrowsingHierarchyClassId == null ? String.Empty : viewModel.BrowsingHierarchyClassId.ToString(),
                IsValidated = "0",
                NationalId = viewModel.NationalHierarchyClassId == null ? String.Empty : viewModel.NationalHierarchyClassId.ToString(),
                AnimalWelfareRatingId = viewModel.SelectedAnimalWelfareRatingId?.ToString(),
                Biodynamic = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedBiodynamicOption),
                CheeseAttributeMilkTypeId = viewModel.SelectedCheeseMilkTypeId?.ToString(),
                CheeseAttributeRaw = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedCheeseRawOption),
                EcoScaleRatingId = viewModel.SelectedEcoScaleRatingId?.ToString(),
                GlutenFreeAgency = viewModel.GlutenFreeAgency,
                KosherAgency = viewModel.KosherAgency,
                Msc = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedMscOption),
                NonGmoAgency = viewModel.NonGmoAgency,
                OrganicAgency = viewModel.OrganicAgency,
                PremiumBodyCare = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedPremiumBodyCareOption),
                SeafoodFreshOrFrozenId = viewModel.SelectedSeafoodFreshOrFrozenId?.ToString(),
                SeafoodWildOrFarmRaisedId = viewModel.SelectedSeafoodCatchTypeId?.ToString(),
                VeganAgency = viewModel.VeganAgency,
                Vegetarian = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedVegetarianOption),
                WholeTrade = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedWholeTradeOption),
                GrassFed = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedGrassFedOption),
                PastureRaised = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedPastureRaisedOption),
                FreeRange = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedFreeRangeOption),
                DryAged = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedDryAgedOption),
                AirChilled = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedAirChilledOption),
                MadeInHouse = ConversionUtility.ConvertYesNoToDatabaseValue(viewModel.SelectedMadeInHouseOption),
                AlcoholByVolume = viewModel.AlcoholByVolume,
                CaseinFree = ConversionUtility.ConvertToItemTraitDbValue(viewModel.CaseinFree),
                DrainedWeight = viewModel.DrainedWeight,
                DrainedWeightUom = viewModel.DrainedWeightUom,
                FairTradeCertified = viewModel.FairTradeCertified,
                Hemp = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Hemp),
                LocalLoanProducer = ConversionUtility.ConvertToItemTraitDbValue(viewModel.LocalLoanProducer),
                MainProductName = viewModel.MainProductName,
                NutritionRequired = ConversionUtility.ConvertToItemTraitDbValue(viewModel.NutritionRequired),
                OrganicPersonalCare = ConversionUtility.ConvertToItemTraitDbValue(viewModel.OrganicPersonalCare),
                Paleo = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Paleo),
                ProductFlavorType = viewModel.ProductFlavorType
            };
        }

        public static BulkImportNewItemModel ToBulkImportNewItemModel(this IrmaItemViewModel viewModel, bool isValidated)
        {
            return new BulkImportNewItemModel
            {
                ScanCode = viewModel.Identifier.Trim().TrimStart('0'),
                BrandId = "0",
                BrandName = viewModel.BrandName,
                ProductDescription = viewModel.ItemDescription.Trim(),
                PosDescription = viewModel.PosDescription.TrimStart(),
                PackageUnit = viewModel.PackageUnit.ToString(),
                RetailSize = viewModel.RetailSize?.ToString(),
                RetailUom = viewModel.RetailUom,
                DeliverySystem = viewModel.DeliverySystem,
                FoodStampEligible = viewModel.FoodStamp ? "1" : "0",
                PosScaleTare = viewModel.PosScaleTare.ToString(),
                MerchandiseId = viewModel.MerchandiseHierarchyClassId == null ? String.Empty : viewModel.MerchandiseHierarchyClassId.ToString(),
                TaxId = viewModel.TaxHierarchyClassId == null ? String.Empty : viewModel.TaxHierarchyClassId.ToString(),
                TaxLineage = viewModel.TaxHierarchyClassName,
                BrowsingId = String.Empty,
                IsValidated = isValidated ? "1" : "0",
                NationalId = viewModel.NationalHierarchyClassId == null ? String.Empty : viewModel.NationalHierarchyClassId.ToString(),
                AnimalWelfareRatingId = viewModel.AnimalWelfareRatingId?.ToString(),
                Biodynamic = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Biodynamic),
                CheeseAttributeMilkTypeId = viewModel.CheeseMilkTypeId?.ToString(),
                CheeseAttributeRaw = ConversionUtility.ConvertToItemTraitDbValue(viewModel.CheeseRaw),
                EcoScaleRatingId = viewModel.EcoScaleRatingId?.ToString(),
                GlutenFreeAgency = viewModel.GlutenFreeAgency,
                KosherAgency = viewModel.KosherAgency,
                Msc = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Msc),
                NonGmoAgency = viewModel.NonGmoAgency,
                OrganicAgency = viewModel.OrganicAgency,
                PremiumBodyCare = ConversionUtility.ConvertToItemTraitDbValue(viewModel.PremiumBodyCare),
                SeafoodFreshOrFrozenId = viewModel.SeafoodFreshOrFrozenId?.ToString(),
                SeafoodWildOrFarmRaisedId = viewModel.SeafoodCatchTypeId?.ToString(),
                VeganAgency = viewModel.VeganAgency,
                Vegetarian = ConversionUtility.ConvertToItemTraitDbValue(viewModel.Vegetarian),
                WholeTrade = ConversionUtility.ConvertToItemTraitDbValue(viewModel.WholeTrade),
                GrassFed = ConversionUtility.ConvertToItemTraitDbValue(viewModel.GrassFed),
                PastureRaised = ConversionUtility.ConvertToItemTraitDbValue(viewModel.PastureRaised),
                FreeRange = ConversionUtility.ConvertToItemTraitDbValue(viewModel.FreeRange),
                DryAged = ConversionUtility.ConvertToItemTraitDbValue(viewModel.DryAged),
                AirChilled = ConversionUtility.ConvertToItemTraitDbValue(viewModel.AirChilled),
                MadeInHouse = ConversionUtility.ConvertToItemTraitDbValue(viewModel.MadeInHouse),
                AlcoholByVolume = viewModel.AlcoholByVolume?.ToString()
            };
        }
    }
}