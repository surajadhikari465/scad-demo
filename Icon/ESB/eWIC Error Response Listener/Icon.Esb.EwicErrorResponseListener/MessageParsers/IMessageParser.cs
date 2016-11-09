using Icon.Esb.Subscriber;

namespace Icon.Esb.EwicAplListener.MessageParsers
{
    public interface IMessageParser<T>
    {
        T ParseMessage(IEsbMessage message);
    }
}
