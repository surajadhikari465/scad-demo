using Esb.Core.Constants;
using Icon.Esb.Schemas.Infor.ContractTypes;
using Icon.Esb.Schemas.Mammoth.ContractTypes;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Schemas.Wfm.PreGpm.Contracts;
using System;
using System.Xml.Serialization;
using GpmContracts = Icon.Esb.Schemas.Wfm.Contracts;
using PreGpmContracts= Icon.Esb.Schemas.Wfm.PreGpm.Contracts;

namespace Esb.Core.Serializer
{
    public static class NamespaceHelper
    {
        public static XmlSerializerNamespaces SetupNamespaces(Type t)
        {
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            if (t == typeof(GpmContracts.items) || t == typeof(PreGpmContracts.items))
            {
                AddItemNamespaces(namespaces);
            }
            else if (t == typeof(GpmContracts.HierarchyType) || t == typeof(PreGpmContracts.HierarchyType))
            {
                AddHierarchyNamespaces(namespaces);
            }
            else if (t == typeof(GpmContracts.LocaleType) || t == typeof(PreGpmContracts.LocaleType))
            {
                AddLocaleNamespaces(namespaces);
            }
            else if (t == typeof(GpmContracts.SelectionGroupsType) || t == typeof(PreGpmContracts.SelectionGroupsType))
            {
                AddSelectionGroupsNameSpaces(namespaces);
            }
            else if (t == typeof(ConfirmBODType))
            {
                AddConfirmBodNameSpaces(namespaces);
            }
            else if (t == typeof(JobSchedule))
            {
                AddJobScheduleNameSpaces(namespaces);
            }
            else
            {
                throw new ArgumentException(String.Format("No namespaces set for type {0}", t.ToString()));
            }

            return namespaces;
        }

        private static void AddJobScheduleNameSpaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add(string.Empty, NamespaceConstants.XmlNamespaces.MammothSchema);
        }

        private static void AddConfirmBodNameSpaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add(string.Empty, NamespaceConstants.XmlNamespaces.InforSchema);
        }

        private static void AddSelectionGroupsNameSpaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("crf", NamespaceConstants.XmlNamespaces.EnterpriseRetailMgmtCommonRefTypes);
            namespaces.Add("grp", NamespaceConstants.XmlNamespaces.EnterpriseRetailMgmtGroupType);
        }

        private static void AddLocaleNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("lcl", NamespaceConstants.XmlNamespaces.EnterpriseLocaleMgmtLocale);
            namespaces.Add("lrf", NamespaceConstants.XmlNamespaces.EnterpriseLocaleMgmtCommonRefTypes);
            namespaces.Add("adr", NamespaceConstants.XmlNamespaces.EnterpriseAddressMgmtAddress);
            namespaces.Add("ltp", NamespaceConstants.XmlNamespaces.EnterpriseLocaleMgmtLocaleType);
            namespaces.Add("tra", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTrait);
            namespaces.Add("trt", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTraitType);
            namespaces.Add("trv", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTraitValue);
            namespaces.Add("eau", NamespaceConstants.XmlNamespaces.EnterpriseAddressMgmtAddressUsage);
            namespaces.Add("eat", NamespaceConstants.XmlNamespaces.EnterpriseAddressMgmtAddressType);
            namespaces.Add("eac", NamespaceConstants.XmlNamespaces.EnterpriseAddressMgmtCommonRefType);
            namespaces.Add("phs", NamespaceConstants.XmlNamespaces.EnterpriseAddressMgmtPhysicalAddress);
            namespaces.Add("ett", NamespaceConstants.XmlNamespaces.EnterpriseTimezoneMgmtTimezone);
            namespaces.Add("cry", NamespaceConstants.XmlNamespaces.EnterpriseAddressMgmtCountry);
        }

        private static void AddHierarchyNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("hcl", NamespaceConstants.XmlNamespaces.EnterpriceHierarchyMgmtHierarchyClass);
            namespaces.Add("hpt", NamespaceConstants.XmlNamespaces.EnterpriseHierarchyMgmtHierarchyPrototype);
            namespaces.Add("tra", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTrait);
            namespaces.Add("trt", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTraitType);
            namespaces.Add("trv", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTraitValue);
            namespaces.Add("crf", NamespaceConstants.XmlNamespaces.EnterpriseRetailMgmtCommonRefTypes);
        }

        private static void AddItemNamespaces(XmlSerializerNamespaces namespaces)
        {
            namespaces.Add("ibi", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtBaseItem);
            namespaces.Add("itp", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtItemType);
            namespaces.Add("sis", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtScanCode);
            namespaces.Add("tra", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTrait);
            namespaces.Add("trt", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTraitType);
            namespaces.Add("ihy", NamespaceConstants.XmlNamespaces.EnterpriseHierarchyMgmtHierarchy);
            namespaces.Add("hcl", NamespaceConstants.XmlNamespaces.EnterpriceHierarchyMgmtHierarchyClass);
            namespaces.Add("crf", NamespaceConstants.XmlNamespaces.EnterpriseRetailMgmtCommonRefTypes);
            namespaces.Add("lcl", NamespaceConstants.XmlNamespaces.EnterpriseLocaleMgmtLocale);
            namespaces.Add("lcy", NamespaceConstants.XmlNamespaces.EnterpriseLocaleMgmtLocaleType);
            namespaces.Add("iea", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtItemAttributesEnterprise);
            namespaces.Add("trv", NamespaceConstants.XmlNamespaces.EnterpriseTraitMgmtTraitValue);
            namespaces.Add("isa", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtItemAttributesStore);
            namespaces.Add("prc", NamespaceConstants.XmlNamespaces.EnterprisePriceMgmtPrice);
            namespaces.Add("pry", NamespaceConstants.XmlNamespaces.EnterprisePriceMgmtPriceType);
            namespaces.Add("rwd", NamespaceConstants.XmlNamespaces.EnterpriseRewardMgmtReward);
            namespaces.Add("rwt", NamespaceConstants.XmlNamespaces.EnterpriseRewardMgmtRewardType);
            namespaces.Add("uom", NamespaceConstants.XmlNamespaces.EnterpriseUnitOfMeasureMgmtUnitOfMeasure);
            namespaces.Add("ama", NamespaceConstants.XmlNamespaces.EnterpriseAmountMgmtAmount);
            namespaces.Add("grp", NamespaceConstants.XmlNamespaces.EnterpriseRetailMgmtGroupType);
            namespaces.Add("psg", NamespaceConstants.XmlNamespaces.EnterpriseSelectionGrpMgmtSelectionGrp);
            namespaces.Add("lnk", NamespaceConstants.XmlNamespaces.EnterpriseRetailMgmtLinkType);
            namespaces.Add("icr", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtCommonRefTypes);
            namespaces.Add("ici", NamespaceConstants.XmlNamespaces.EnterpriseItemMgmtConsumerInformation);
        }
    }
}