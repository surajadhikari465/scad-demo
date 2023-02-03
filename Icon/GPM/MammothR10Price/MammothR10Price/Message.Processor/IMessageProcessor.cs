using Icon.Dvs.Model;

namespace MammothR10Price.Message.Processor
{
    public interface IMessageProcessor
    {
        void ProcessReceivedMessage(DvsMessage message);
    }
}
