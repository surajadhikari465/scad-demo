using Icon.Dvs.Model;

namespace Icon.Esb.CchTax.MessageParsers
{
    public interface IMessageParser<T>
    {
        T ParseMessage(DvsMessage message);
    }
}
