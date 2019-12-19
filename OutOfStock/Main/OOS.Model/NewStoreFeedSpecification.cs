using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model.Feed;

namespace OOS.Model
{
    public class NewStoreFeedSpecification
    {
        public static bool IsSatisfiedBy(StoreFeed storeFeed)
        {
            if (string.IsNullOrWhiteSpace(storeFeed.facility)) return false;
            return storeFeed.facility.Equals("Whole Foods Market", StringComparison.OrdinalIgnoreCase) ? true : false;
        }
    }
}
