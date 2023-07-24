using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.DataAccess
{
    public interface IDAL<InventoryModel>
    {
        IList<InventoryModel> Get(string eventType, int keyId, int? secondaryKeyId);

        void Insert(List<InstockDequeueModel> instockDequeueModelList);
    }
}
