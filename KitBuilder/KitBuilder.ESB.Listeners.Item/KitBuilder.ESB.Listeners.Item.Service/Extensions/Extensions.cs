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

            if (enterpriseAttributes == null) return null;

            var itemModel = new ItemModel
            {
                ItemId = item.id,
                ScanCode = enterpriseAttributes.scanCodes.First().code,
                MerchandiseHierarchyClassId = GetHierarchyClassId(enterpriseAttributes, HierarchyNames.Merchandise),
                BrandsHierarchyName = GetHierarchyClassName(enterpriseAttributes, HierarchyNames.Brands),
                CustomerFriendlyDescription = GetTraitValue(enterpriseAttributes, TraitCodes.CustomerFriendlyDescription),
                ProductDescription = GetTraitValue(enterpriseAttributes, TraitCodes.ProductDescription),
                FlexibleText = GetTraitValue(enterpriseAttributes, TraitCodes.FlexibleText),
                KitchenDescription = enterpriseAttributes.kitchenDescription,
                ImageUrl = enterpriseAttributes.imageUrl,
                KitchenItem = enterpriseAttributes.isKitchenItemSpecified  ? enterpriseAttributes.isKitchenItem : (bool?) null,
                HospitalityItem = enterpriseAttributes.isHospitalityItemSpecified ?  enterpriseAttributes.isHospitalityItem : (bool?) null,
				PosDescription = GetTraitValue(enterpriseAttributes, TraitCodes.PosDescription)
			};

            return itemModel;
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

        public static bool ToBool(this string boolString)
        {
            return boolString == "1";
        }

       
    }
}
