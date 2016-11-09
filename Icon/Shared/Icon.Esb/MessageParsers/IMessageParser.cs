using Icon.Esb.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.MessageParsers
{
    public interface IMessageParser<T>
    {
        T ParseMessage(IEsbMessage message);
    }
}
