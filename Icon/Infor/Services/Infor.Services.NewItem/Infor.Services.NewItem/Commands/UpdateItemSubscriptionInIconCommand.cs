using Services.NewItem.Models;
using System.Collections.Generic;

namespace Services.NewItem.Commands
{
    public class UpdateItemSubscriptionInIconCommand
    {
        public IEnumerable<NewItemModel> NewItems { get; set; }
    }
}
