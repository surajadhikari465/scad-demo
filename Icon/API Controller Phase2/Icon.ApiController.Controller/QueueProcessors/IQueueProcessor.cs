using System;

namespace Icon.ApiController.Controller.QueueProcessors
{
    public interface IQueueProcessor
    {
        void ProcessMessageQueue();
    }
}
