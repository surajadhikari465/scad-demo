using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using KitBuilder.ESB.Listeners.Item.Service.Constants.ItemValidation;
using KitBuilder.ESB.Listeners.Item.Service.Models;

namespace KitBuilder.ESB.Listeners.Item.Service.Extensions
{
    public static class Extensions
    {
        public static ItemModel ToItemModel(
           this Icon.Esb.Schemas.Wfm.Contracts.ItemType item,
           string inforMessageId,
           DateTime messageParseTime,
           decimal? sequenceId)
        {
            var enterpriseAttributes = item.locale.First().Item as EnterpriseItemAttributesType;
            var storeAttributes = item.locale.First().Item as StoreItemAttributesType;

            if (enterpriseAttributes == null) return null;

            return new ItemModel
            {
                ItemId = item.id,
                ScanCode = enterpriseAttributes.scanCodes.First().code,
                MerchandiseHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.Merchandise),
                BrandsHierarchyName = GetHierarchyClassName(enterpriseAttributes, HierarchyNames.Brands),
                CustomerFriendlyDescription = GetTraitValue(enterpriseAttributes, TraitCodes.CustomerFriendlyDescription),
                FlexibleText = GetTraitValue(enterpriseAttributes, TraitCodes.FlexibleText),
                KitchenDescription = enterpriseAttributes.kitchenDescription,
                ImageUrl = enterpriseAttributes.imageUrl,
                KitchenItem = enterpriseAttributes.isKitchenItemSpecified ? enterpriseAttributes.isKitchenItem : false,
                HospitalityItem = enterpriseAttributes.isHospitalityItemSpecified ? enterpriseAttributes.isHospitalityItem : false,
            };
        }

     

        private static string GetScanCodeTypeCode(string scanCode)
        {
            if (IsScalePlu(scanCode))
            {
                return ScanCodeTypes.Descriptions.ScalePlu;
            }
            else if (scanCode.Length < 7)
            {
                return ScanCodeTypes.Descriptions.PosPlu;
            }
            else
            {
                return ScanCodeTypes.Descriptions.Upc;
            }
        }

        private static bool IsScalePlu(string scanCode)
        {
            return Regex.IsMatch(scanCode, RegularExpressions.ScalePlu)
                || Regex.IsMatch(scanCode, RegularExpressions.IngredientPlu46)
                || Regex.IsMatch(scanCode, RegularExpressions.IngredientPlu48);
        }

        private static int GetItemTypeCodeId(string typeCode)
        {
            if (ItemTypes.Ids.ContainsKey(typeCode))
            {
                return ItemTypes.Ids[typeCode];
            }
            else
            {
                return -1;
            }
        }

        private static int GetScanCodeTypeId(string scanCode)
        {
            if (scanCode.Length == 11 && scanCode[0] == '2' && scanCode.EndsWith("00000"))
            {
                return ScanCodeTypes.ScalePlu;
            }
            else if (scanCode.Length < 7)
            {
                return ScanCodeTypes.PosPlu;
            }
            else
            {
                return ScanCodeTypes.Upc;
            }
        }

        private static string GetTraitValue(EnterpriseItemAttributesType enterpriseAttributes, string traitCode, string defaultValue = "")
        {
            var traitValue = enterpriseAttributes.traits.FirstOrDefault(i => i.code == traitCode);

            if (traitValue == null || string.IsNullOrWhiteSpace(traitValue.type.value.First().value))
                return defaultValue;
            else
                return traitValue.type.value.First().value;
        }

        private static string GetHierarchyClassId(EnterpriseItemAttributesType enterpriseAttributes, string hierarchyName)
        {
            try
            {
                var hierarchyClassId = enterpriseAttributes.hierarchies.FirstOrDefault(i => i.name == hierarchyName);
                if (hierarchyClassId == null)
                {
                    return string.Empty;
                }
                else
                {
                    return hierarchyClassId.@class.FirstOrDefault()?.id; 
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetHierarchyClassName(EnterpriseItemAttributesType enterpriseAttributes, string hierarchyName)
        {
            try
            {
                var hierarchyClassName = enterpriseAttributes.hierarchies.FirstOrDefault(i => i.name == hierarchyName).@class.FirstOrDefault().name;
                return string.IsNullOrEmpty(hierarchyClassName) ? string.Empty : hierarchyClassName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static int? GetIdFromDescription(this Dictionary<string, int> descriptionToIdDictionary, string description)
        {
            if (description == null || !descriptionToIdDictionary.ContainsKey(description))
            {
                return null;
            }
            else
            {
                return descriptionToIdDictionary[description];
            }
        }

        public static bool ToBool(this string boolString)
        {
            return boolString == "1";
        }

        private static bool ContainsDuplicateHierarchyClass(EnterpriseItemAttributesType enterpriseAttributes, string hierarchyName)
        {
            var hierarchyClassCount = enterpriseAttributes.hierarchies.Count(i => i.name == hierarchyName);

            return hierarchyClassCount > 1;
        }
    }
}
