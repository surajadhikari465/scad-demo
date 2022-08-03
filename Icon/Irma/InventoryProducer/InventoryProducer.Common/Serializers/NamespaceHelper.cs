using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Xml.Serialization;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Schemas;

namespace InventoryProducer.Common.Serializers
{
    public static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            if (t == typeof(inventoryAdjustments))
            {
                AddInventorySpoilageNamespaces(namespaces);
            }
            else if (t == typeof(EventTypes))
            {
                AddEventTypesNamespaces(namespaces);
            }
            else if (t == typeof(ErrorMessage))
            {
                AddErrorMessageNamespaces(namespaces);
            }
            else
            {
                throw new ArgumentException(string.Format("No namespaces set for type {0}", t.ToString()));
            }

            return namespaces;
        }
        private static void AddInventorySpoilageNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.EnterpriseInventoryMgmtCommonRefTypes);
            namespaces.Add("ns1", Constants.XmlNamespaces.EnterpriseUnitOfMeasureMgmtUnitOfMeasure);
            namespaces.Add("ns2", Constants.XmlNamespaces.EnterpriseTransactionMgmtCommonRefTypes);
        }
        private static void AddEventTypesNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.IrmaHoneyCrispEvents);
        }
        private static void AddErrorMessageNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ns0", Constants.XmlNamespaces.MammothErrorMessage);
        }
    }
}
