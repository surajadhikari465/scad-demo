namespace OOS.Model
{
    public class SummaryRepositoryFactory : ISummaryRepositoryFactory
    {
        private IUserProfile userProfile;
        private IRegionRepository regionRepository;
        private IStoreRepository storeRepository;
        private IOOSEntitiesFactory dbFactory;
        private IConfigurator config;

        public SummaryRepositoryFactory(IConfigurator config, IUserProfile userProfile, 
            IRegionRepository regionRepository, IStoreRepository storeRepository, IOOSEntitiesFactory dbFactory)
        {
            this.config = config;
            this.userProfile = userProfile;
            this.regionRepository = regionRepository;
            this.storeRepository = storeRepository;
            this.dbFactory = dbFactory;
        }

        public ISummaryRepository New(string regionAbbrev)
        {
            if (userProfile.IsRegionBuyer()) 
                return RegionSummaryRepository(regionAbbrev);

            var userRegion = userProfile.UserRegion();
            return RegionSummaryRepository(userRegion);

            //var storeAbbrev = userProfile.UserStoreAbbreviation();
            //return StoreSummaryRepository(storeAbbrev);
        }

        private ISummaryRepository RegionSummaryRepository(string regionAbbrev)
        {
            var region = regionRepository.ForAbbrev(regionAbbrev);
            return region == null ? null : new RegionSummaryRepository(region, dbFactory, config);
        }

        private ISummaryRepository StoreSummaryRepository(string storeAbbrev)
        {
            var store = storeRepository.ForAbbrev(storeAbbrev);
            return store == null ? null : new StoreSummaryRepository(store, dbFactory);
        }

    }
}