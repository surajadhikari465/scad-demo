using System.Collections.Generic;
using InventoryProducer.Producer.Model.DBModel;

namespace InventoryProducer.Producer.DataAccess
{
    public interface ITransferOrdersDAL
    {
        IList<TransferOrdersModel> GetTransferOrders(string eventType, int keyId, int? secondaryKeyId);
    }
}
