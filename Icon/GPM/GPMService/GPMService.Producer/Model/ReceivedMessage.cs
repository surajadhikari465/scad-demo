using Icon.Esb.Subscriber;
using System.Collections.Generic;

namespace GPMService.Producer.Model
{
    internal class ReceivedMessage
    {
        public IEsbMessage esbMessage { get; set; }
    }
}
