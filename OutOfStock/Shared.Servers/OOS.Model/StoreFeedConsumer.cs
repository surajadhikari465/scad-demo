using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
//using AaronPowell.Dynamics.Collections;
using Newtonsoft.Json;
using OOS.Model.Infrastructure;
using OOSCommon;

namespace OOS.Model.Feed
{
    public class StoreFeedConsumer : JsonRestService<IStoreFeedConsumer>, IStoreFeedConsumer
    {
        private IOOSLog logger;

        public StoreFeedConsumer(ILogService logService, string serviceUrl) : base(serviceUrl)
        {
            logger = logService.GetLogger();
        }

        public dynamic Consume()
        {
            try
            {
                return Send(x => x.Consume());
            }
            catch(Exception ex)
            {
                logger.Info(string.Format("Consume() Failed: {0}", ex.Message));
                return new Dictionary<string, StoreFeed>(); //.AsDynamic();
            }
        }

        protected override dynamic Deserialize(JsonReader jReader)
        {
            var json = jsonSerializer.Deserialize<List<StoreFeed>>(jReader);
            var jsonDic = json.ToDictionary(p => p.tlc, q => q);
            return jsonDic; //.AsDynamic();
        }
    }
}
