using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model.Feed
{
    public class StoreFactory : IStoreFactory
    {
        private IRegionRepository regionRepository;

        public StoreFactory(IRegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }

        public List<Store> ConstituteFrom(IEnumerable<StoreFeed> storeFeeds)
        {
            return (from feed in storeFeeds
                    let r = regionRepository.ForAbbrev(feed.region)
                    let s = (r == null) ? null : r.GetStores().FirstOrDefault(p => feed.tlc == p.Abbrev)                   
                    where r != null
                    select new Store(0) { Name = feed.name, Abbrev = feed.tlc, Status = feed.status, RegionId = r.Id, BusinessUnitNo = (s == null) ? string.Empty : s.BusinessUnitNo }).ToList();
        }

    }
}
