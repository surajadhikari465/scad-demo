using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Icon.Esb;

namespace GPMService.Producer.Serializer
{
    public class Serializer<T> : ISerializer<T>
    {
        private readonly XmlSerializer serializer;
        private readonly XmlWriterSettings settings;
        private readonly XmlSerializerNamespaces namespaces;

        public Serializer()
        {
            this.serializer = new XmlSerializer(typeof(T));
            this.settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.None, //prevent newline character from appearing in the serialized string.
                Indent = false,                         //prevent tab from appearing in the serialized string.
                Encoding = System.Text.Encoding.UTF8    // UTF-8 is the desired format for ESB.
            };

            this.namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
        }

        public T Deserialize(TextReader reader)
        {
            return (T)serializer.Deserialize(reader);
        }

        public string Serialize(T canonicalObject, TextWriter writer)
        {
            XmlWriter xmlWriter = XmlWriter.Create(writer, this.settings);
            serializer.Serialize(xmlWriter, canonicalObject, this.namespaces);
            return writer.ToString();
        }
    }
}
