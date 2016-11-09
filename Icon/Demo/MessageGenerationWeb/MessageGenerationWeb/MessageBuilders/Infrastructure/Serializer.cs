using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MessageGenerationWeb.MessageBuilders.Infrastructure
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
            settings.OmitXmlDeclaration = true;

            namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
        }

        public string Serialize(T miniBulk, TextWriter writer)
        {
            XmlWriter xmlWriter = XmlWriter.Create(writer, settings);
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                serializer.Serialize(xmlWriter, miniBulk, namespaces);
            }
            catch (Exception)
            {
                return String.Empty;
            }

            string xml = writer.ToString();

            return xml;
        }
    }
}