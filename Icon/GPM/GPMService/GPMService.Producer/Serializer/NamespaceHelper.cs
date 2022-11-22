using GPMService.Producer.Helpers;
using System;
using System.Xml.Serialization;

namespace GPMService.Producer.Serializer
{
    public static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            if (t == typeof(ConfirmBODType))
            {
                AddConfirmBODTypeNamespaces(namespaces);
            }
            else if (t == typeof(PriceMessageArchiveType))
            {
                AddPriceMessageArchiveTypeNamespaces(namespaces);
            }
            else if (t == typeof(MammothPricesType) || t == typeof(MammothPriceType))
            {
                AddMammothPricesTypeNamespaces(namespaces);
            }
            return namespaces;
        }

        private static void AddMammothPricesTypeNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.MammothMammothPrices);
        }

        private static void AddConfirmBODTypeNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.InforOAGIS);
        }
        private static void AddPriceMessageArchiveTypeNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.MammothPriceMessageArchive);
        }
    }
}
