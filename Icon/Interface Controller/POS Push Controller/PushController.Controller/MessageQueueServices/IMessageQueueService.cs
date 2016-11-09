using System.Collections.Generic;

namespace PushController.Controller.MessageQueueServices
{
    public interface IMessageQueueService<T>
    {
        void SaveMessagesBulk(List<T> messagesToSave);
        void SaveMessagesRowByRow(List<T> messagesToSave);
    }
}
