using GPMService.Producer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb;

namespace GPMService.Producer.Message.Parser
{
    internal class NearRealTimeMessageParser : IMessageParser<items>
    {
        private XmlSerializer serializer;
        private TextReader textReader;

        public NearRealTimeMessageParser()
        {
            serializer = new XmlSerializer(typeof(items));
        }
        public items ParseMessage(ReceivedMessage receivedMessage)
        {
            items gpmParsed;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(receivedMessage.esbMessage.MessageText)))
            {
                gpmParsed = serializer.Deserialize(textReader) as items;
            }
            return gpmParsed;
        }
    }
}
