﻿using System.IO;
using System.Xml;
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

        public string Serialize(T t)
        {
            using (var writer = new Utf8StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                {
                    return Serialize(t, writer, xmlWriter);
                }
            }
        }

        public string Serialize(T t, TextWriter writer)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
            {
                return Serialize(t, writer, xmlWriter);
            }
        }

        private string Serialize(T t, TextWriter writer, XmlWriter xmlWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(xmlWriter, t, namespaces);

            return writer.ToString();
        }
    }
}
