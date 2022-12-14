using System;
using System.Xml.Serialization;

namespace MammothR10Price.Serializer
{
    public static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add("v1", "http://schemas.wfm.com/Enterprise/ItemMgmt/Item/V1");
            namespaces.Add("rwd", "http://schemas.wfm.com/Enterprise/RewardMgmt/Reward/V2");
            namespaces.Add("ibi", "http://schemas.wfm.com/Enterprise/ItemMgmt/BaseItem/V1");
            namespaces.Add("isa", "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesStore/V2");
            namespaces.Add("ltp", "http://schemas.wfm.com/Enterprise/LocaleMgmt/LocaleType/V2");
            namespaces.Add("sis", "http://schemas.wfm.com/Enterprise/ItemMgmt/ScanCode/V2");
            namespaces.Add("crf", "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1");
            namespaces.Add("lcl", "http://schemas.wfm.com/Enterprise/LocaleMgmt/Locale/V2");
            namespaces.Add("amt", "http://schemas.wfm.com/Enterprise/AmountMgmt/Amount/V1");
            namespaces.Add("trv", "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitValue/V2");
            namespaces.Add("trt", "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitType/V2");
            namespaces.Add("rrt", "http://schemas.wfm.com/Enterprise/RewardMgmt/RewardType/V1");
            namespaces.Add("prt", "http://schemas.wfm.com/Enterprise/PriceMgmt/PriceType/V1");
            namespaces.Add("itp", "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemType/V1");
            namespaces.Add("prc", "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2");
            namespaces.Add("ns0", "http://schemas.wfm.com/Enterprise/TraitMgmt/Trait/V2");
            namespaces.Add("uom", "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2");
            return namespaces;
        }
    }
}
