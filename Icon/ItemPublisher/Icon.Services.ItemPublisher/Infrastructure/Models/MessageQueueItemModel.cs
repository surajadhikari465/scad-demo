using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Models
{
    /// <summary>
    /// This model represents everything we need to load from the database
    /// to create a DVS message
    /// </summary>
    public class MessageQueueItemModel
    {
        public ItemModel Item { get; set; }
        public List<Hierarchy> Hierarchy { get; set; }
        public Nutrition Nutrition { get; set; }
    }
}