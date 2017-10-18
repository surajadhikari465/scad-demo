using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Esb.Core.Serializer
{
    public class SerializerWithoutEncodingType<T> : ISerializer<T>
    {
        private XmlSerializerNamespaces namespaces;

        public SerializerWithoutEncodingType()
        {
            namespaces = NamespaceHelper.SetupNamespaces(typeof(T));
        }

        public string Serialize(T t)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stream, t, namespaces);

                stream.Position = 0;
                return XDocument.Load(stream).ToString();
            }
        }

        public string Serialize(T t, TextWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, t, namespaces);

            return XDocument.Parse(writer.ToString()).ToString();
        }
    }
}
