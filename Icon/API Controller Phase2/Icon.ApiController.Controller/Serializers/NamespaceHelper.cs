using Icon.ApiController.Common;
using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Xml.Serialization;

namespace Icon.ApiController.Controller.Serializers
{
    public static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            if (t == typeof(items))
            {
                AddItemNamespaces(namespaces);
            }
            else if (t == typeof(HierarchyType))
            {
                AddHierarchyNamespaces(namespaces);
            }
            else if (t == typeof(LocaleType))
            {
                AddLocaleNamespaces(namespaces);
            }
            else if (t == typeof(SelectionGroupsType))
            {
                AddSelectionGroupsNameSpaces(namespaces);
            }
            else
            {
                throw new ArgumentException(String.Format("No namespaces set for type {0}", t.ToString()));
            }

            return namespaces;
        }

        private static void AddSelectionGroupsNameSpaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("crf", Constants.XmlNamespaces.EnterpriseRetailMgmtCommonRefTypes);
            namespaces.Add("grp", Constants.XmlNamespaces.EnterpriseRetailMgmtGroupType);
        }

        private static void AddLocaleNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("lcl", Constants.XmlNamespaces.EnterpriseLocaleMgmtLocale);
            namespaces.Add("lrf", Constants.XmlNamespaces.EnterpriseLocaleMgmtCommonRefTypes);
            namespaces.Add("adr", Constants.XmlNamespaces.EnterpriseAddressMgmtAddress);
            namespaces.Add("ltp", Constants.XmlNamespaces.EnterpriseLocaleMgmtLocaleType);
            namespaces.Add("tra", Constants.XmlNamespaces.EnterpriseTraitMgmtTrait);
            namespaces.Add("trt", Constants.XmlNamespaces.EnterpriseTraitMgmtTraitType);
            namespaces.Add("trv", Constants.XmlNamespaces.EnterpriseTraitMgmtTraitValue);
            namespaces.Add("eau", Constants.XmlNamespaces.EnterpriseAddressMgmtAddressUsage);
            namespaces.Add("eat", Constants.XmlNamespaces.EnterpriseAddressMgmtAddressType);
            namespaces.Add("eac", Constants.XmlNamespaces.EnterpriseAddressMgmtCommonRefType);
            namespaces.Add("phs", Constants.XmlNamespaces.EnterpriseAddressMgmtPhysicalAddress);
            namespaces.Add("ett", Constants.XmlNamespaces.EnterpriseTimezoneMgmtTimezone);
            namespaces.Add("cry", Constants.XmlNamespaces.EnterpriseAddressMgmtCountry);
        }

        private static void AddHierarchyNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("hcl", Constants.XmlNamespaces.EnterpriceHierarchyMgmtHierarchyClass);
            namespaces.Add("hpt", Constants.XmlNamespaces.EnterpriseHierarchyMgmtHierarchyPrototype);
        }

        private static void AddItemNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ibi", Constants.XmlNamespaces.EnterpriseItemMgmtBaseItem);
            namespaces.Add("itp", Constants.XmlNamespaces.EnterpriseItemMgmtItemType);
            namespaces.Add("sis", Constants.XmlNamespaces.EnterpriseItemMgmtScanCode);
            namespaces.Add("tra", Constants.XmlNamespaces.EnterpriseTraitMgmtTrait);
            namespaces.Add("trt", Constants.XmlNamespaces.EnterpriseTraitMgmtTraitType);
            namespaces.Add("ihy", Constants.XmlNamespaces.EnterpriseHierarchyMgmtHierarchy);
            namespaces.Add("hcl", Constants.XmlNamespaces.EnterpriceHierarchyMgmtHierarchyClass);
            namespaces.Add("crf", Constants.XmlNamespaces.EnterpriseRetailMgmtCommonRefTypes);
            namespaces.Add("lcl", Constants.XmlNamespaces.EnterpriseLocaleMgmtLocale);
            namespaces.Add("lcy", Constants.XmlNamespaces.EnterpriseLocaleMgmtLocaleType);
            namespaces.Add("iea", Constants.XmlNamespaces.EnterpriseItemMgmtItemAttributesEnterprise);
            namespaces.Add("trv", Constants.XmlNamespaces.EnterpriseTraitMgmtTraitValue);
            namespaces.Add("isa", Constants.XmlNamespaces.EnterpriseItemMgmtItemAttributesStore);
            namespaces.Add("prc", Constants.XmlNamespaces.EnterprisePriceMgmtPrice);
            namespaces.Add("pry", Constants.XmlNamespaces.EnterprisePriceMgmtPriceType);
            namespaces.Add("uom", Constants.XmlNamespaces.EnterpriseUnitOfMeasureMgmtUnitOfMeasure);
            namespaces.Add("ama", Constants.XmlNamespaces.EnterpriseAmountMgmtAmount);
            namespaces.Add("grp", Constants.XmlNamespaces.EnterpriseRetailMgmtGroupType);
            namespaces.Add("psg", Constants.XmlNamespaces.EnterpriseSelectionGrpMgmtSelectionGrp);
            namespaces.Add("lnk", Constants.XmlNamespaces.EnterpriseRetailMgmtLinkType);
            namespaces.Add("icr", Constants.XmlNamespaces.EnterpriseItemMgmtCommonRefTypes);
            namespaces.Add("ici", Constants.XmlNamespaces.EnterpriseItemMgmtConsumerInformation);            
        }
    }
}