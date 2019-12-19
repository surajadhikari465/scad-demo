using System.Collections.Generic;
using System.Linq;
using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Esb.Subscriber;

namespace Mammoth.Esb.AttributeListener.MessageParsers
{
    public class AttributeMessageParser : MessageParserBase<AttributesType, AttributesType>
    {
        public override AttributesType ParseMessage(IEsbMessage message)
        {
            AttributesType attributes = DeserializeMessage(message);

            return attributes;
        }
    }
}
