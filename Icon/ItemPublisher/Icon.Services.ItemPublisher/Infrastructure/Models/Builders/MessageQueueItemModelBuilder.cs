using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Models.Builders
{
    /// <summary>
    /// Builder for MessageQueueItemModel
    /// </summary>
    public class MessageQueueItemModelBuilder : IMessageQueueItemModelBuilder
    {
        private IItemMapper itemMapper;

        public MessageQueueItemModelBuilder(IItemMapper itemMapper)
        {
            this.itemMapper = itemMapper;
        }

        /// <summary>
        /// Builds a MessageQueueItemModel from the supplied arguments.
        /// Translates attributes to item properties and vice versa. There is an issue
        /// where typically attributes in Icon are sent as traits in the DVS message but
        /// there are some exceptions. Some Item properties are traits in the DVS message and
        /// some attributes are actual properties in the DVS message. This method handles the translation
        /// and constructs a MessageQueueItemModel that is correct.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemExtended"></param>
        /// <param name="hierarchy"></param>
        /// <param name="nutrition"></param>
        /// <returns></returns>
        public MessageQueueItemModel Build(Item item,
          List<Hierarchy> hierarchy,
          Nutrition nutrition)
        {
            MessageQueueItemModel response = new MessageQueueItemModel();
            ItemModel itemModel = itemMapper.MapEntityToModel(item);
            response.Item = itemModel;
            response.Hierarchy = hierarchy == null ? new List<Hierarchy>() : hierarchy;
            response.Nutrition = nutrition;

            // translate attributes to the property on the item model
            if (response.Item.ItemAttributes.ContainsKey("IsKitchenItem"))
            {
                response.Item.IsKitchenItemSpecified = true;
                if (response.Item.ItemAttributes["IsKitchenItem"] == "1")
                {
                    response.Item.IsKitchenItem = true;
                }
            }

            if (response.Item.ItemAttributes.ContainsKey("IsHospitalityItem"))
            {
                response.Item.IsHospitalityItemSpecified = true;
                if (response.Item.ItemAttributes["IsHospitalityItem"] == "1")
                {
                    response.Item.IsHospitalityItem = true;
                }
            }

            // remove kitchen and hospitality attributes because they should not be sent as attributes
            response.Item.ItemAttributes.Remove("IsKitchenItemSpecified");
            response.Item.ItemAttributes.Remove("IsKitchenItem");
            response.Item.ItemAttributes.Remove("IsHospitalityItemSpecified");
            response.Item.ItemAttributes.Remove("IsHospitalityItem");

            return response;
        }
    }
}