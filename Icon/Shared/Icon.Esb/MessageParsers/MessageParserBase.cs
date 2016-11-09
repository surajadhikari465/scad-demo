using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb.Subscriber;
using System.Xml.Serialization;
using System.IO;

namespace Icon.Esb.MessageParsers
{
    public abstract class MessageParserBase<TContract, TModel> : IMessageParser<TModel> 
        where TContract : class
    {
        protected XmlSerializer serializer;
        protected TextReader textReader;

        public MessageParserBase()
        {
            serializer = new XmlSerializer(typeof(TContract));
        }

        protected TContract DeserializeMessage(IEsbMessage message)
        {
            TContract contract;
            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(message.MessageText)))
            {
                contract = serializer.Deserialize(textReader) as TContract;
            }
            return contract;
        }

        public abstract TModel ParseMessage(IEsbMessage message);
    }
}
