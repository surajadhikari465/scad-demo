using System.Collections.Generic;
using System.Linq;
using Icon.Dvs.MessageParser;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Dvs.Model;

namespace Mammoth.Esb.AttributeListener.MessageParsers
{
    public class AttributeMessageParser : MessageParserBase<AttributesType, AttributesType>
    {
        public override AttributesType ParseMessage(DvsMessage message)
        {
            AttributesType attributes = DeserializeMessage(message);

            return attributes;
        }
    }
}
