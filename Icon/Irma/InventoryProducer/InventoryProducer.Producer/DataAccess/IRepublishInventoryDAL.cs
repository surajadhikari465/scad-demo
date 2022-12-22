using InventoryProducer.Producer.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.DataAccess
{
    internal interface IRepublishInventoryDAL
    {
        IList<ArchivedMessageModel> GetUnsentMessages();
        void UpdateMessageArchiveWithError(int messageArchiveId, int processTimes, string errorDescription);
    }
}
