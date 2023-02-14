using Services.Extract.Models;

namespace Services.Extract.Message.Parser
{
    public interface IMessageParser<T>
    {
        T ParseMessage(string receivedMessage);
    }
}
