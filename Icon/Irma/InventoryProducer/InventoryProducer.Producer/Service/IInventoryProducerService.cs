using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryProducer.Producer.Service
{
    interface IInventoryProducerService
    {
        void Start();
        void Stop();
    }
}
