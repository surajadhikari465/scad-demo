using System.Xml.Serialization;
using System.IO;
using Icon.Dvs.Model;

namespace Icon.Dvs.MessageParser
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

        protected TContract DeserializeMessage(DvsMessage message)
        {
            TContract contract;
            using (textReader = new StringReader(RemoveUnusableCharactersFromXml(message.MessageContent)))
            {
                contract = serializer.Deserialize(textReader) as TContract;
            }
            return contract;
        }

        public abstract TModel ParseMessage(DvsMessage message);

        private static string RemoveUnusableCharactersFromXml(string xml)
        {
            const int bomCharacter = 65279;
            return xml.Replace("" + (char)bomCharacter, "");
        }
    }
}
