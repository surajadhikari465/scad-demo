using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Models.Builders
{
    public interface IMessageQueueItemModelBuilder
    {
        MessageQueueItemModel Build(Item item,
            List<Hierarchy> hierarchy,
            Nutrition nutrition);
    }
}