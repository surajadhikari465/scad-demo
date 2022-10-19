using InventoryProducer.Producer.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.DataAccess
{
    public interface IPurchaseOrdersDAL
    {
        IList<PurchaseOrdersModel> GetPurchaseOrders(string eventType, int keyId, int? secondaryKeyId);
    }
}
