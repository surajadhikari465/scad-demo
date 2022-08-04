using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using System.Collections.Generic;

namespace InventoryProducer.Common.InstockDequeue.Model
{
    public class InstockDequeueResult
    {
        public InstockDequeueModel InstockDequeueModel { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }

        public InstockDequeueResult(InstockDequeueModel instockDequeueModel, Dictionary<string, string> headers)
        {
            this.InstockDequeueModel = instockDequeueModel;
            this.Headers = headers;
        }
    }
}
