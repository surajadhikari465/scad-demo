using Icon.Esb.Subscriber;

namespace MammothR10Price.Message.Processor
{
    public interface IMessageProcessor
    {
        void ProcessReceivedMessage(IEsbMessage message);
    }
}
