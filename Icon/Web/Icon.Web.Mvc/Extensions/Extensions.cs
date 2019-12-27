using Icon.Framework;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Icon.Common.Models;

namespace Icon.Web.Mvc.Extensions
{
    public static class Extensions
    {
        public static ItemViewModel ToViewModel(this ItemDbModel item)
        {
            return new ItemViewModel(item);
        }

        public static ItemHistoryModel ToViewModel(this ItemHistoryDbModel itemHistory)
        {
            return new ItemHistoryModel(itemHistory);
        }

        public static IEnumerable<HierarchyClassViewModel> ToViewModels(this IEnumerable<HierarchyClassModel> models)
        {
            return models.Select(m => m.ToViewModel());
        }

        public static HierarchyClassViewModel ToViewModel(this HierarchyClassModel model)
        {
            return new HierarchyClassViewModel(model);
        }

        public static List<BrandViewModel> ToViewModels(this List<BrandModel> brands)
        {
            return brands.Select(b => new BrandViewModel(b)).ToList();
        }

        public static List<ManufacturerViewModel> ToViewModels(this List<ManufacturerModel> manufacturer)
        {
            return manufacturer.Select(b => new ManufacturerViewModel(b)).ToList();
        }

        public static IEnumerable<AttributeViewModel> ToViewModels(this IEnumerable<AttributeModel> attributes)
        {
            return attributes.Select(a => new AttributeViewModel(a)).ToList();
        }

        public static List<CertificationAgencyViewModel> ToViewModels(this List<CertificationAgencyModel> agencies)
        {
            return agencies.Select(b => new CertificationAgencyViewModel(b)).ToList();
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
            if(dropDownList == null)
            {
                return new List<SelectListItem>();
            }
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
                DeliverySystem = viewModel.DeliverySystem,
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
                AnimalWelfareRating = viewModel.ItemSignAttributes.AnimalWelfareRating,
                Biodynamic = viewModel.ItemSignAttributes.SelectedBiodynamicOption,
                MilkType = viewModel.ItemSignAttributes.CheeseMilkType,
                CheeseRaw = viewModel.ItemSignAttributes.SelectedCheeseRawOption,
                EcoScaleRating = viewModel.ItemSignAttributes.EcoScaleRating,
                GlutenFreeAgency = viewModel.ItemSignAttributes.GlutenFreeAgency,
                KosherAgency = viewModel.ItemSignAttributes.KosherAgency,
                Msc = viewModel.ItemSignAttributes.SelectedMscOption,
                NonGmoAgency = viewModel.ItemSignAttributes.NonGmoAgency,
                OrganicAgency = viewModel.ItemSignAttributes.OrganicAgency,
                PremiumBodyCare = viewModel.ItemSignAttributes.SelectedPremiumBodyCareOption,
                SeafoodFreshOrFrozen = viewModel.ItemSignAttributes.SeafoodFreshOrFrozen,
                SeafoodCatchType = viewModel.ItemSignAttributes.SeafoodCatchType,
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

        public static string ToPascalCase(this string str)
        {
            if (str.Length > 1)
                return str[0].ToString().ToLower() + str.Substring(1);
            else
                return str.ToLower();
        }
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> source,
            string fields, 
            bool toPascal = false)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            // create a list to hold our ExpandoObjects
            var expandoObjectList = new List<ExpandoObject>();

            // create a list with PropertyInfo objects on TSource.  Reflection is
            // expensive, so rather than doing it for each object in the list, we do 
            // it once and reuse the results.  After all, part of the reflection is on the 
            // type of the object (TSource), not on the instance
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject
                var propertyInfos = typeof(TSource)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                // only the public properties that match the fields should be
                // in the ExpandoObject

                // the field are separated by ",", so we split it.
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    // trim each field, as it might contain leading 
                    // or trailing spaces. Can't trim the var in foreach,
                    // so use another var.
                    var propertyName = field.Trim();

                    // use reflection to get the property on the source object
                    // we need to include public and instance, b/c specifying a binding flag overwrites the
                    // already-existing binding flags.
                    var propertyInfo = typeof(TSource)
                        .GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    }

                    // add propertyInfo to list 
                    propertyInfoList.Add(propertyInfo);
                }
            }

            // run through the source objects
            foreach (TSource sourceObject in source)
            {
                // create an ExpandoObject that will hold the 
                // selected properties & values
                var dataShapedObject = new ExpandoObject();

                // Get the value of each property we have to return.  For that,
                // we run through the list
                foreach (var propertyInfo in propertyInfoList)
                {
                    // GetValue returns the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    var propertyName = toPascal ? propertyInfo.Name.ToPascalCase() : propertyInfo.Name;

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyName, propertyValue);
                }

                // add the ExpandoObject to the list
                expandoObjectList.Add(dataShapedObject);
            }

            // return the list

            return expandoObjectList;
        }
    }

}