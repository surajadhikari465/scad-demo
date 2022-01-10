using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface IMessageHeaderBuilder
    {
        Dictionary<string, string> BuildMessageHeader(List<string> nonReceivingSystemsProduct, string messageId);
    }
}