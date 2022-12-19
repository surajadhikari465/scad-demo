using System.Collections.Generic;

namespace MammothR10Price.Publish
{
    public interface IMessagePublisher
    {
        void Publish(string message, Dictionary<string, string> messageProperties);
    }
}
