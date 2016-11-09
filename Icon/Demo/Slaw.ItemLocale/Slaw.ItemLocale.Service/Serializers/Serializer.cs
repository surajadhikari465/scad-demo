using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Slaw.ItemLocale.Serializers
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
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            serializer.Serialize(xmlWriter, miniBulk, namespaces);            
            string xml = writer.ToString();

            return xml;
        }
    }
}
