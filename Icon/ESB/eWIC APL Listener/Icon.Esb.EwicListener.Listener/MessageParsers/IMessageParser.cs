using Icon.Dvs.Model;

namespace Icon.Esb.EwicAplListener.MessageParsers
{
    public interface IMessageParser<T>
    {
        T ParseMessage(DvsMessage message);
    }
}
