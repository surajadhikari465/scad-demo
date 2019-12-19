using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOS.Model
{
    public class StoreValidator : IStoreValidator
    {
        private IRegionRepository regionRepository;
        private IStoreRepository storeRepository;

        public StoreValidator(IRegionRepository regionRepository, IStoreRepository storeRepository)
        {
            this.regionRepository = regionRepository;
            this.storeRepository = storeRepository;
        }


        public void Validate(string storeAbbrev)
        {
            var storeRegion = storeRepository.ForAbbrev(storeAbbrev);
            AssertStoreValid(storeRegion, storeAbbrev);
        }

        public void ValidateStoreInRegion(string regionAbbrev, string storeAbbrev)
        {
            var region = regionRepository.ForAbbrev(regionAbbrev);
            AssertRegionValid(region, regionAbbrev);
            var store = region.OpenStores().FirstOrDefault(p => p.Abbrev.Equals(storeAbbrev, StringComparison.OrdinalIgnoreCase));
            AssertStoreValid(store, storeAbbrev);
        }

        private static void AssertRegionValid(Region region, string regionAbbrev)
        {
            if (region == null)
            {
                throw new InvalidStoreException(string.Format("Invalid Region Abbreviation='{0}'", regionAbbrev));
            }
        }

        private static void AssertStoreValid(Store store, string storeAbbrev)
        {
            if (store == null)
            {
                throw new InvalidStoreException(string.Format("Invalid Store Abbreviation='{0}' in Region", storeAbbrev));
            }
        }
    }
}
