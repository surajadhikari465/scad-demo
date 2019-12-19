using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model.Feed
{
    public interface IStoreFeedConsumerService
    {
        void Consume();
        void Consume(IEnumerable<StoreFeed> storeFeeds);
    }
}
