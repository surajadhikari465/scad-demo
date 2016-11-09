using System.Collections.Generic;

namespace Vim.Common.ControllerApplication
{
    public interface IQueueManager<T>
    {
        List<T> Get(List<int> eventTypeIds);
        void Finalize(List<T> queueRecords);
    }
}
