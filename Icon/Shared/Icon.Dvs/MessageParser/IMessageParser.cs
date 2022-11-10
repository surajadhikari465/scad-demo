using System;
using Icon.Dvs.Model;

namespace Icon.Dvs.MessageParser
{
    public interface IMessageParser<T>
    {
        T ParseMessage(DvsMessage message);
    }
}
