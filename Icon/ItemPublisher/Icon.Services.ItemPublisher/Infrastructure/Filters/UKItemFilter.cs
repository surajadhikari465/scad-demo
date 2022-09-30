using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Models;

namespace Icon.Services.ItemPublisher.Infrastructure.Filters
{
    public class UKItemFilter: IFilter
    {
        private ILogger<UKItemFilter> logger;

        public UKItemFilter(ILogger<UKItemFilter> logger)
        {
            this.logger = logger;
        }

        public bool Filter(ItemModel item)
        {
            if(item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.UKItem) && 
                item.ItemAttributes[ItemPublisherConstants.Attributes.UKItem] != null &&
                item.ItemAttributes[ItemPublisherConstants.Attributes.UKItem].ToLower().Trim().Equals("yes")
            )
            {
                logger.Warn($"Item {item.ItemId} is UK specific and will be filtered out");
                return true;
            }
            return false;
        }
    }
}
