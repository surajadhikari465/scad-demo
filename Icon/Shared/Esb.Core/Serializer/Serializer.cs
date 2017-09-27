using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Esb.Core.Serializer
{
    public class Serializer<T> : ISerializer<T>
    {
        private XmlWriterSettings settings;
        private XmlSerializerNamespaces namespaces;
        private bool setNamespaceAliases;

        public Serializer() : this(true)
        {

        }

        public Serializer(bool setNamespaceAliases)
        {
            settings = new XmlWriterSettings();
            // These settings prevent things like tab and newline characters from appearing in the serialized string.
            settings.NewLineHandling = NewLineHandling.None;
            settings.Indent = false;
            // UTF-8 is the desired format for ESB.
            settings.Encoding = System.Text.Encoding.UTF8;

            if (setNamespaceAliases)
            {
                namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
            }
            this.setNamespaceAliases = setNamespaceAliases;
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
            if (setNamespaceAliases)
            {
                serializer.Serialize(xmlWriter, t, namespaces);
            }
            else
            {
                serializer.Serialize(xmlWriter, t);
            }

            return writer.ToString();
        }
    }
}
