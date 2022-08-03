using Icon.Logging;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace InventoryProducer.Common.Serializers
{
    public class Serializer<T> : ISerializer<T>
    {
        private readonly XmlWriterSettings settings;
        private readonly XmlSerializerNamespaces namespaces;

        public Serializer()
        {
            this.settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.None, //prevent newline character from appearing in the serialized string.
                Indent = false,                         //prevent tab from appearing in the serialized string.
                Encoding = System.Text.Encoding.UTF8    // UTF-8 is the desired format for ESB.
            };

            this.namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
        }

        public string Serialize(T canonicalObject, TextWriter writer)
        {
            XmlWriter xmlWriter = XmlWriter.Create(writer, this.settings);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(xmlWriter, canonicalObject, this.namespaces);
            string xml = writer.ToString();
            return xml;
        }
    }
}
