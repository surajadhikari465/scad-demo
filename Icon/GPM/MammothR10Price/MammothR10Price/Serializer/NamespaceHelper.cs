using System;
using System.Xml.Serialization;

namespace MammothR10Price.Serializer
{
    public static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            return namespaces;
        }
    }
}
