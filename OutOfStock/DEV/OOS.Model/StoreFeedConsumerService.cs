using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOSCommon;

namespace OOS.Model.Feed
{
    public class StoreFeedConsumerService : IStoreFeedConsumerService
    {
        private IOOSLog logger;
        private IStoreRepository storeRepository;
        private IStoreFeedConsumer consumer;
        private IStoreFactory storeFactory;

        public StoreFeedConsumerService(ILogService logService, IStoreFeedConsumer consumer, IStoreRepository storeRepository, IStoreFactory storeFactory)
        {
            logger = logService.GetLogger();
            this.consumer = consumer;
            this.storeRepository = storeRepository;
            this.storeFactory = storeFactory;
        }

        public void Consume()
        {
            logger.Debug("Consume() Enter");
            Consume(StoreFeeds());
            logger.Debug("Consume() Exit");
        }

        public void Consume(IEnumerable<StoreFeed> feeds)
        {
            if (feeds == null)
            {
                logger.Info("Consume() Invalid feeds: feeds cannot be null");
                return;               
            }

            var decodedFeeds = StoreFeedInfoDecoder.Decode(feeds);
            AddNewStores(decodedFeeds);
            UpdateExistingStores(decodedFeeds);
        }


        private List<StoreFeed> StoreFeeds()
        {
            var feeds = consumer.Consume();
            if (feeds == null)
            {
                logger.Debug("StoreFeeds() No Store Feeds were consumed.");
                return new List<StoreFeed>();
            }

            var storeFeeds = new List<StoreFeed>();
            foreach (StoreFeed feed in feeds.Values)
            {
                storeFeeds.Add(feed);
            }
            return storeFeeds;
        }

        internal void AddNewStores(IEnumerable<StoreFeed> feeds)
        {
            var storeFeeds = new List<StoreFeed>();
            foreach (StoreFeed feed in feeds)
            {
                if (!NewStoreFeedSpecification.IsSatisfiedBy(feed)) continue;

                var store = storeRepository.ForAbbrev(feed.tlc);
                if (store != null) continue;
                storeFeeds.Add(feed);
                logger.Debug(string.Format("AddNewStores() Store='{0}' Name='{1}' Region='{2}' Status='{3}'", feed.tlc, feed.name, feed.region, feed.status));
            }
            logger.Debug(string.Format("AddNewStores() Consumed {0} NEW store feeds", storeFeeds.Count));
            var newStores = storeFactory.ConstituteFrom(storeFeeds);
            newStores.ForEach(storeRepository.Add);
        }

        internal void UpdateExistingStores(IEnumerable<StoreFeed> feeds)
        {
            var storeFeeds = new List<StoreFeed>();
            foreach (StoreFeed feed in feeds)
            {
                var store = storeRepository.ForAbbrev(feed.tlc);
                if (store == null || (feed.status == store.Status && feed.name == store.Name)) continue;

                storeFeeds.Add(feed);
                logger.Debug(string.Format("UpdateExistingStores() Store='{0}' Name='{1}' Region='{2}' Status='{3}'", feed.tlc, feed.name, feed.region, feed.status));
            }
            logger.Debug(string.Format("UpdateExistingStores() Consumed {0} UPDATED store feeds", storeFeeds.Count));
            var stores = storeFactory.ConstituteFrom(storeFeeds);
            stores.ForEach(storeRepository.Update);
        }
    }
}
