﻿using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace IrmaPriceListenerService.Serializer
{
    public class Serializer<T>: ISerializer<T>
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
        }

        public string Serialize(T canonicalObject, TextWriter writer)
        {
            XmlWriter xmlWriter = XmlWriter.Create(writer, this.settings);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(xmlWriter, canonicalObject, this.namespaces);
            return writer.ToString();
        }
    }
}
