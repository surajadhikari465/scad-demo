using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using OOS.Model.Feed;

namespace OutOfStock.StoreConsumerWebService
{
    [ServiceContract]
    public interface IStoreConsumer
    {
        [OperationContract]
        void Consume(StoreFeed[] feeds);
    }
}
