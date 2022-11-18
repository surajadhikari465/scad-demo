using System;
using System.Collections.Generic;

namespace GPMService.Producer.Publish
{
    internal interface IMessagePublisher
    {
        void PublishMessage(string message, Dictionary<string, string> messageProperties);
    }
}
