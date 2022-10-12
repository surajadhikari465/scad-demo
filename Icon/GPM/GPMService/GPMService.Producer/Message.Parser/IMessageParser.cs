using GPMService.Producer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMService.Producer.Message.Parser
{
    internal interface IMessageParser
    {
        // TODO: Modify the return type.
        void ParseMessage(ReceivedMessage receivedMessage);
    }
}
