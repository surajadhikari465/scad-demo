using GPMService.Producer.Model;

namespace GPMService.Producer.Message.Parser
{
    internal interface IMessageParser<T>
    {
        T ParseMessage(ReceivedMessage receivedMessage);
    }
}
