using GPMService.Producer.Model;

namespace GPMService.Producer.Message.Processor
{
    internal interface IMessageProcessor
    {
        void ProcessMessage(ReceivedMessage receivedMessage);
    }
}
