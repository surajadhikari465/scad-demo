using PushController.Common;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class DeleteItemLinksCommand
    {
        public List<ItemLinkModel> ItemLinks { get; set; }
    }
}