using Icon.Services.ItemPublisher.Repositories.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Models.Mappers
{
    public class ItemMapper : IItemMapper
    {
        /// <summary>
        /// Maps the item entity to the item model
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ItemModel MapEntityToModel(Item entity)
        {
            ItemModel item = new ItemModel
            {
                ItemTypeId = entity.ItemTypeId,
                ItemTypeDescription = entity.ItemTypeDescription,
                ItemTypeCode = entity.ItemTypeCode,
                ScanCode = entity.ScanCode,
                ScanCodeId = entity.ScanCodeId,
                ScanCodeTypeDesc = entity.ScanCodeTypeDesc,
                ScanCodeTypeId = entity.ScanCodeTypeId,
                IsHospitalityItemSpecified = false,
                IsKitchenItemSpecified = false,
                ItemId = entity.ItemId,
                ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(entity.ItemAttributesJson),
                SysEndTimeUtc = entity.SysEndTimeUtc,
                SysStartTimeUtc = entity.SysStartTimeUtc
            };

            if (item.ItemAttributes.TryGetValue(ItemPublisherConstants.Attributes.HospitalityItem, out string hospitalityItemValue))
            {
                item.IsHospitalityItem = hospitalityItemValue.ToBooleanFromYesOrNo();
                item.IsHospitalityItemSpecified = true;
                item.ItemAttributes.Remove(ItemPublisherConstants.Attributes.HospitalityItem);
            }

            if (item.ItemAttributes.TryGetValue(ItemPublisherConstants.Attributes.KitchenItem, out string kitchenItemValue))
            {
                item.IsKitchenItem = kitchenItemValue.ToBooleanFromYesOrNo();
                item.IsKitchenItemSpecified = true;
                item.ItemAttributes.Remove(ItemPublisherConstants.Attributes.KitchenItem);
            }

            if (item.ItemAttributes.TryGetValue(ItemPublisherConstants.Attributes.Url1, out string imageUrlValue))
            {
                item.ImageUrl = imageUrlValue;
                item.ItemAttributes.Remove(ItemPublisherConstants.Attributes.Url1);
            }

            if (item.ItemAttributes.TryGetValue(ItemPublisherConstants.Attributes.KitchenDescription, out string kitchenDescriptionValue))
            {
                item.KitchenDescription = kitchenDescriptionValue;
                item.ItemAttributes.Remove(ItemPublisherConstants.Attributes.KitchenDescription);
            }

            return item;
        }
    }
}