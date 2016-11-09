using Mammoth.Common.ControllerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication
{
    public interface IQueueManager<T>
    {
        ChangeQueueEvents<T> Get();
        void Finalize(ChangeQueueEvents<T> queueRecords);
    }
}
