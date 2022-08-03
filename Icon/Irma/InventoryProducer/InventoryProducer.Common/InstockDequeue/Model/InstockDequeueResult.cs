using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using System.Collections.Generic;

namespace InventoryProducer.Common.InstockDequeue.Model
{
    public class InstockDequeueResult
    {
        public InstockDequeueModel instockDequeueModel;
        public Dictionary<string, string> headers;

        public InstockDequeueResult(InstockDequeueModel instockDequeueModel, Dictionary<string, string> headers)
        {
            this.instockDequeueModel = instockDequeueModel;
            this.headers = headers;
        }
    }
}
