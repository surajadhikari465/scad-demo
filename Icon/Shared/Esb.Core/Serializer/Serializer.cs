using System.Data.SqlTypes;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Esb.Core.Serializer
{
    public class Serializer<T> : ISerializer<T>
    {
        private XmlWriterSettings settings;
        private XmlSerializerNamespaces namespaces;

        public Serializer()
        {
            settings = new XmlWriterSettings();

            // These settings prevent things like tab and newline characters from appearing in the serialized string.
            settings.NewLineHandling = NewLineHandling.None;
            settings.Indent = false;

            // UTF-8 is the desired format for ESB.
            settings.Encoding = System.Text.Encoding.UTF8;

            namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
        }

        public string Serialize(T miniBulk, TextWriter writer)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, miniBulk, namespaces);

                stream.Position = 0;
                return XDocument.Load(stream).ToString();
            }
        }
    }
}
