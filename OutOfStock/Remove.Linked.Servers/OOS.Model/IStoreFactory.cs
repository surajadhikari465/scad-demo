using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Feed;

namespace OOS.Model
{
    public interface IStoreFactory
    {
        List<Store> ConstituteFrom(IEnumerable<StoreFeed> storeFeeds);
    }
}
