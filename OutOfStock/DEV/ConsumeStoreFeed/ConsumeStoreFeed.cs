using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;
using OOS.Model.Feed;
using StructureMap;

namespace ConsumeStoreFeed
{
    public class Program
    {
        static void Main(string[] args)
        {
            Bootstrap();
            var logger = ObjectFactory.GetInstance<ILogService>().GetLogger();
            logger.Info("Main() Enter");
            var service = ObjectFactory.GetInstance<StoreFeedConsumerService>();
            service.Consume();
            logger.Info("Main() Exit");
        }

        private static void Bootstrap()
        {
            StructuremapBootstrapper.Bootstrap();
        }
    }
}
