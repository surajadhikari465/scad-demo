using Icon.Esb.Schemas.Mammoth.ContractTypes;
using JobScheduler.Service.Helper;
using System;
using System.Xml.Serialization;

namespace JobScheduler.Service.Serializer
{
    internal static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            if (t == typeof(JobSchedule))
            {
                AddJobScheduleNamespaces(namespaces);
            }
            return namespaces;
        }

        private static void AddJobScheduleNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.MammothJobSchedule);
        }
    }
}
