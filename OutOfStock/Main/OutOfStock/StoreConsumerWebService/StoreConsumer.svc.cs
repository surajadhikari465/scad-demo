using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OOS.Model.Feed;
using StructureMap;

namespace OutOfStock.StoreConsumerWebService
{
    public class StoreConsumer : IStoreConsumer
    {
        public void Consume(StoreFeed[] feeds)
        {
            var consumer = ObjectFactory.GetInstance<StoreFeedConsumerService>();
            consumer.Consume(feeds);
        }
    }
}
