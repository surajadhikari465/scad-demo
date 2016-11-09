using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetIrmaItemSubscriptionsQuery : IQuery<List<IRMAItemSubscription>>
    {
        public List<IRMAPush> IrmaPushData { get; set; }
    }
}
