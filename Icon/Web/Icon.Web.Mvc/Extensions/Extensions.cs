﻿using Icon.Framework;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Extensions
{
    public static class Extensions
    {
        public static List<ItemViewModel> ToViewModels(this List<ItemSearchModel> items)
        {
            return items.Select(item => new ItemViewModel(item)).ToList();
        }

        public static List<BrandViewModel> ToViewModels(this List<BrandModel> brands)
        {
            return brands.Select(b => new BrandViewModel(b)).ToList();
        }

        public static List<CertificationAgencyViewModel> ToViewModels(this List<CertificationAgencyModel> agencies)
        {
            return agencies.Select(b => new CertificationAgencyViewModel(b)).ToList();
        }

        public static List<PluRequestViewModel> ToViewModels(this List<PLURequest> pluRequests)
        {
            return pluRequests.Select(pluRequest => new PluRequestViewModel(pluRequest)).ToList();
        }

        public static List<PluRequestChangeHistoryViewModel> ToViewModels(this List<PLURequestChangeHistory> pluRequests)
        {
            return pluRequests.Select(pluRequest => new PluRequestChangeHistoryViewModel(pluRequest)).ToList();
        }
        /// <summary>
        /// Builds a string representation of the HierarchyClass that is passed in. The HierarchyClass is expected to be the leaf node of
        /// the hierarchy.
        /// </summary>
        /// <param name="leafHierarchyClass">The leaf node of a HierarchyClass hierarchy</param>
        /// <returns></returns>
        public static string ToFlattenedHierarchyClassString(this HierarchyClass leafHierarchyClass)
        {
            string flattenedHierarchyString = leafHierarchyClass.hierarchyClassName;

            // Append Financial Subteam to the end of it if it's a Merch Sub-brick
            if (leafHierarchyClass.hierarchyID == Hierarchies.Merchandise)
            {
                var subteam = leafHierarchyClass.HierarchyClassTrait.FirstOrDefault(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping);
                if (subteam != null)
                {
                    flattenedHierarchyString = flattenedHierarchyString + ": " + subteam.traitValue.Replace("(", "").Replace(")", ""); ;
                }
            }

            flattenedHierarchyString = flattenedHierarchyString + "|" + leafHierarchyClass.hierarchyClassID;
            var parentHierarchyClass = leafHierarchyClass.HierarchyClass2;
            string currentParentHierarchyClassName = String.Empty;

            while (parentHierarchyClass != null)
            {
                //flattenedHierarchyString = parentHierarchyClass.hierarchyClassName + "|" + flattenedHierarchyString;
                currentParentHierarchyClassName = parentHierarchyClass.hierarchyClassName;
                parentHierarchyClass = parentHierarchyClass.HierarchyClass2;
            }

            if (!String.IsNullOrEmpty(currentParentHierarchyClassName))
                flattenedHierarchyString = currentParentHierarchyClassName + "|" + flattenedHierarchyString;

            return flattenedHierarchyString;
        }

        /// <summary>
        /// Builds a string representation of the Merchandise HierarchyClass that is passed in. The HierarchyClass is expected to be the leaf node of
        /// the hierarchy.
        /// </summary>
        /// <param name="leafHierarchyClass">Leaf node of the Hierarchy</param>
        /// <param name="includeSubTeam">To include the Subteam suffixed to end of string before the hierarchyClassId</param>
        /// <param name="includeSubBrickCode">To include the SubBrickCode suffixed to the end of the string before the hierarchyClassId</param>
        /// <returns>The full lineage of a hierarchy class separated by the '|' char</returns>
        public static string ToFlattenedMerchandiseHierarchyClassString(this HierarchyClass leafHierarchyClass, bool includeSubTeam, bool includeSubBrickCode)
        {
            string flattenedHierarchyString = leafHierarchyClass.hierarchyClassName;

            // Append Financial Subteam to the end of it if it's a Merch Sub-brick
            if (includeSubTeam)
            {
                var subteam = leafHierarchyClass.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.MerchFinMapping);
                if (subteam != null)
                {
                    flattenedHierarchyString = flattenedHierarchyString + ": " + subteam.traitValue.Replace("(", "").Replace(")", ""); ;
                }    
            }

            if (includeSubBrickCode)
            {
                var subBrickCode = leafHierarchyClass.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.SubBrickCode);
                if (subBrickCode != null)
                {
                    flattenedHierarchyString = flattenedHierarchyString + "|" + subBrickCode.traitValue;
                }    
            }

            flattenedHierarchyString = flattenedHierarchyString + "|" + leafHierarchyClass.hierarchyClassID;
            var parentHierarchyClass = leafHierarchyClass.HierarchyClass2;
            string currentParentHierarchyClassName = String.Empty;

            while (parentHierarchyClass != null)
            {
                //flattenedHierarchyString = parentHierarchyClass.hierarchyClassName + "|" + flattenedHierarchyString;
                currentParentHierarchyClassName = parentHierarchyClass.hierarchyClassName;
                parentHierarchyClass = parentHierarchyClass.HierarchyClass2;
            }

            if (!String.IsNullOrEmpty(currentParentHierarchyClassName))
                flattenedHierarchyString = currentParentHierarchyClassName + "|" + flattenedHierarchyString;

            return flattenedHierarchyString;

        }

        /// <summary>
        /// Builds a string representation of the Tax HierarchyClass that is passed in.
        /// the hierarchy.
        /// </summary>
        /// <param name="taxHierarchyClass">Leaf node of the Hierarchy</param>
        /// <param name="useTaxRomance">should should TaxRomacne instead</param>
        /// <returns>The full lineage of a hierarchy class separated by the '|' char</returns>
        public static string ToFlattenedTaxHierarchyClassString(this HierarchyClass taxHierarchyClass, bool useTaxRomance)
        {
            string flattenedHierarchyString = taxHierarchyClass.hierarchyClassName;

            // Use TaxRomance if not null otherwise use Tax name.
            if (useTaxRomance)
            {
                var taxRomance = taxHierarchyClass.HierarchyClassTrait.FirstOrDefault(hct => hct.Trait.traitCode == TraitCodes.TaxRomance);
                if (taxRomance != null)
                {
                    flattenedHierarchyString = taxRomance.traitValue;
                }
            }

            flattenedHierarchyString = flattenedHierarchyString + "|" + taxHierarchyClass.hierarchyClassID;
            return flattenedHierarchyString;

        }

        public static List<HierarchyClassViewModel> HierarchyClassForCombo(this Hierarchy hierarchy, bool excludeAffinitySubBricks = true)
        {
            return hierarchy.HierarchyClass.HierarchyClassForCombo(excludeAffinitySubBricks);
        }
        
        public static List<ConfigurationViewModel> ToConfigurationViewModel(this List<RegionalSettingsModel> regionalSettingsist)
        {
            return regionalSettingsist.Select(item => new ConfigurationViewModel(item)).ToList();        
        }

        public static List<HierarchyClassViewModel> HierarchyClassForCombo(this List<HierarchyClassModel> hierarchyList)
        {
            return hierarchyList.Select(item => new HierarchyClassViewModel(item)).ToList();
        }

        public static List<HierarchyClassViewModel> HierarchyClassForCombo(this IEnumerable<HierarchyClass> hierarchyClasses, bool excludeAffinitySubBricks = true)
        {
            int maxLevel = hierarchyClasses.Select(hc => hc.hierarchyLevel).Max().GetValueOrDefault();

            if (excludeAffinitySubBricks)
            {
                hierarchyClasses = hierarchyClasses.ExcludeAffinitySubBricks();
            }

            List<HierarchyClassViewModel> hierarchyClassViewModels = hierarchyClasses
                .Where(hc => hc.hierarchyLevel == maxLevel)
                .Select(hc => new HierarchyClassViewModel { HierarchyClassLineage = hc.ToFlattenedHierarchyClassString() })
                .ToList();

            int hierarchyClassId;
            foreach (var hierarchyClass in hierarchyClassViewModels)
            {
                string[] splitHierarchyLineage = hierarchyClass.HierarchyClassLineage.Split('|');

                if (Int32.TryParse(splitHierarchyLineage.Last(), out hierarchyClassId))
                {
                    hierarchyClass.HierarchyClassId = hierarchyClassId;
                }

                hierarchyClass.HierarchyClassName = String.Join("|", splitHierarchyLineage.Take(splitHierarchyLineage.Length - 1)).Trim();
            }

            List<HierarchyClassViewModel> hierarchyCombo = hierarchyClassViewModels.OrderBy(hc => hc.HierarchyClassName).ToList();
            return hierarchyCombo;
        }

        /// <summary>
        /// Parses string into Enum of type T.  If Parsing does not work an argument exception will be thrown.
        /// </summary>
        /// <typeparam name="T">Specified Enum</typeparam>
        /// <param name="enumString">the string that needs to be converted to Enum of type T</param>
        /// <returns>Enum of type T</returns>
        public static TEnum ToEnum<TEnum>(this string enumString) where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            TEnum status;
            if (Enum.TryParse<TEnum>(enumString, true, out status))
            {
                return status;
            }
            else
            {
                throw new ArgumentException("String cannot be parsed into the enum");
            }
        }

        public static IEnumerable<SelectListItem> ToSelectListItem(this IEnumerable<DropDownViewModel> dropDownList)
        {
            return dropDownList.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
        }

        /// <summary>
        /// This will remove the sub-bricks that are associated to the Affinity node.
        /// The affinity node cannot be associated to items and this removes it from a Hierarchy.HierarchyClass list
        /// </summary>
        /// <param name="hierarchy">Hierarchy with all navigation properties loaded</param>
        /// <returns></returns>
        public static IEnumerable<HierarchyClass> ExcludeAffinitySubBricks(this Hierarchy hierarchy)
        {
            return hierarchy.HierarchyClass.ExcludeAffinitySubBricks();
        }

        /// <summary>
        /// This will remove the sub-bricks that are associated to the Affinity node.
        /// The affinity node cannot be associated to items and this removes it from a HierarchyClass list.
        /// </summary>
        /// <param name="hierarchyClasses">HierarchyClass list with navigation properties loaded.</param>
        /// <returns></returns>
        public static IEnumerable<HierarchyClass> ExcludeAffinitySubBricks(this IEnumerable<HierarchyClass> hierarchyClasses)
        {
            hierarchyClasses = hierarchyClasses.Where(hc => !hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.Affinity));
            return hierarchyClasses;
        }

        public static string[] ParseByLine(this string lineDelimitedString)
        {
            return lineDelimitedString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(scanCode => scanCode.Trim()).ToArray();
        }

        public static bool? YesNoStringToBool(this string stringValue)
        {
            bool? boolValue = null;
            if (String.Equals(stringValue, "Y", StringComparison.InvariantCultureIgnoreCase) || String.Equals(stringValue, "YES", StringComparison.InvariantCultureIgnoreCase))
            {
                boolValue = true;
            }
            else if (String.Equals(stringValue, "N", StringComparison.InvariantCultureIgnoreCase) || String.Equals(stringValue, "NO", StringComparison.InvariantCultureIgnoreCase))
            {
                boolValue = false;
            }
            return boolValue;
        }

        public static string BoolToYesNoStringValue(this bool? boolValue)
        {
            return boolValue.HasValue ? (boolValue.Value == true ? "Yes" : "No") : string.Empty;
        }

        public static string BoolToYesNoStringValue(this bool boolValue)
        {
            return boolValue ? "Yes" : "No";
        }

        public static bool IsPosOrScalePlu(this string scanCode)
        {
            if (string.IsNullOrWhiteSpace(scanCode))
            {
                throw new ArgumentNullException("scanCode");
            }
            else if(scanCode.Length < 7)
            {
                return true;
            }
            else if(scanCode.Length == 11 && scanCode.StartsWith("2") && scanCode.EndsWith("00000"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ConvertDateFormat(string stringDateValue)
        {
            if (string.IsNullOrEmpty(stringDateValue))
                return stringDateValue;

            DateTime dateValue = DateTime.Parse(stringDateValue);
            if (dateValue != null)
            {
                return dateValue.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        public static GetItemsBySearchParameters GetSearchParameters(this ItemSearchViewModel viewModel, bool getCountOnly = false, int? pageSize = null)
        {
            return new GetItemsBySearchParameters
            {
                ScanCode = viewModel.ScanCode,
                BrandName = viewModel.BrandName,
                PartialBrandName = viewModel.PartialBrandName,
                ProductDescription = viewModel.ProductDescription,
                RetailSize = viewModel.RetailSize,
                RetailUom = viewModel.SelectedRetailUom,
                DeliverySystem = viewModel.SelectedDeliverySystem,
                MerchandiseHierarchy = viewModel.MerchandiseHierarchy,
                TaxRomance = viewModel.TaxHierarchy,
                PosDescription = viewModel.PosDescription,
                SearchStatus = viewModel.Status.Single(s => s.Value == viewModel.SelectedStatusId.ToString()).Text.ToEnum<SearchStatus>(),
                HiddenItemStatus = viewModel.HiddenStatus.Single(s => s.Value == viewModel.SelectedHiddenItemStatusId.ToString()).Text.ToEnum<HiddenStatus>(),
                FoodStampEligible = viewModel.SelectedFoodStampId,
                DepartmentSale = viewModel.SelectedDepartmentSaleId,
                PosScaleTare = viewModel.PosScaleTare,
                PackageUnit = viewModel.PackageUnit,
                PartialScanCode = viewModel.PartialScanCode,
                NationalClass = viewModel.NationalHierarchy,
                AlcoholByVolume = viewModel.AlcoholByVolume,
                CaseinFree = viewModel.CaseinFree.ConvertYesNoToDatabaseValue(),
                DrainedWeight = viewModel.DrainedWeight,
                DrainedWeightUom = viewModel.DrainedWeightUom,
                FairTradeCertified = viewModel.FairTradeCertified,
                Hemp = viewModel.Hemp.ConvertYesNoToDatabaseValue(),
                LocalLoanProducer = viewModel.LocalLoanProducer.ConvertYesNoToDatabaseValue(),
                MainProductName = viewModel.MainProductName,
                NutritionRequired = viewModel.NutritionRequired.ConvertYesNoToDatabaseValue(),
                OrganicPersonalCare = viewModel.OrganicPersonalCare.ConvertYesNoToDatabaseValue(),
                Paleo = viewModel.Paleo.ConvertYesNoToDatabaseValue(),
                ProductFlavorType = viewModel.ProductFlavorType,
                AnimalWelfareRatingId = viewModel.ItemSignAttributes.SelectedAnimalWelfareRatingId,
                Biodynamic = viewModel.ItemSignAttributes.SelectedBiodynamicOption,
                MilkTypeId = viewModel.ItemSignAttributes.SelectedCheeseMilkTypeId,
                CheeseRaw = viewModel.ItemSignAttributes.SelectedCheeseRawOption,
                EcoScaleRatingId = viewModel.ItemSignAttributes.SelectedEcoScaleRatingId,
                GlutenFreeAgency = viewModel.ItemSignAttributes.GlutenFreeAgency,
                KosherAgency = viewModel.ItemSignAttributes.KosherAgency,
                Msc = viewModel.ItemSignAttributes.SelectedMscOption,
                NonGmoAgency = viewModel.ItemSignAttributes.NonGmoAgency,
                OrganicAgency = viewModel.ItemSignAttributes.OrganicAgency,
                PremiumBodyCare = viewModel.ItemSignAttributes.SelectedPremiumBodyCareOption,
                SeafoodFreshOrFrozenId = viewModel.ItemSignAttributes.SelectedSeafoodFreshOrFrozenId,
                SeafoodCatchTypeId = viewModel.ItemSignAttributes.SelectedSeafoodCatchTypeId,
                VeganAgency = viewModel.ItemSignAttributes.VeganAgency,
                Vegetarian = viewModel.ItemSignAttributes.SelectedVegetarianOption,
                WholeTrade = viewModel.ItemSignAttributes.SelectedWholeTradeOption,
                Notes = viewModel.Notes,
                GrassFed = viewModel.ItemSignAttributes.SelectedGrassFedOption,
                PastureRaised = viewModel.ItemSignAttributes.SelectedPastureRaisedOption,
                FreeRange = viewModel.ItemSignAttributes.SelectedFreeRangeOption,
                DryAged = viewModel.ItemSignAttributes.SelectedDryAgedOption,
                AirChilled = viewModel.ItemSignAttributes.SelectedAirChilledOption,
                MadeInHouse = viewModel.ItemSignAttributes.SelectedMadeInHouseOption,
                CreatedDate = ConvertDateFormat(viewModel.CreatedDate),
                LastModifiedDate = ConvertDateFormat(viewModel.LastModifiedDate),
                LastModifiedUser = viewModel.LastModifiedUser,
                PageIndex = viewModel.Page,
                PageSize = pageSize == null ? viewModel.PageSize : pageSize,
                GetCountOnly = getCountOnly
            };
        }
    }
}