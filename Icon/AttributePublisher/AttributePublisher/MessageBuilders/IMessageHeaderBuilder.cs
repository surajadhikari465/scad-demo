using System.Collections.Generic;

namespace AttributePublisher.MessageBuilders
{
    public interface IMessageHeaderBuilder
    {
        Dictionary<string, string> BuildHeader(string messageID);
    }
}