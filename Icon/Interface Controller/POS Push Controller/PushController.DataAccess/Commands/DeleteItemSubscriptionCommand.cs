using Icon.Framework;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class DeleteItemSubscriptionCommand
    {
        public List<IRMAItemSubscription> Subscriptions { get; set; }
    }
}
