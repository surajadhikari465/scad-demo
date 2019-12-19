using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Esb.Core.Serializer;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Esb.Core.Mappers;
using Services.NewItem.Cache;
using Services.NewItem.MessageBuilders;
using Services.NewItem.Models;
using System.Collections.Generic;
using System.IO;
using Icon.Common.DataAccess;
using Services.NewItem.Queries;
using System.Linq;
using System.Xml.Linq;
using Icon.Framework;

namespace Services.NewItem.Tests.Helpers
{
    public static class XmlDeepEquals
    {
        public static void AssertDeepEqualXmlDocs(XDocument xmlDocA, XDocument xmlDocB, string customFailMessage = null)
        {
            if (customFailMessage == null)
            {
                var standardFailMessage = String.Format(
                    "XDocument comparison failed XNode.DeepEquals().{0}EXPECTED{1}{0}{2}{0}{1}(end of EXPECTED){0}{0}ACTUAL:{1}{0}{3}{0}{1}(end of ACTUAL)",
                    Environment.NewLine, "--------", xmlDocA, xmlDocB);
                var areEquivalent = AreXmlDocsDeepEqual(xmlDocA, xmlDocB);
                Assert.IsTrue(areEquivalent, standardFailMessage);
            }
            else
            {
                Assert.IsTrue(XNode.DeepEquals(xmlDocA, xmlDocB), customFailMessage);
            }
        }
        public static bool AreXmlDocsDeepEqual(XDocument xmlDocA, XDocument xmlDocB)
        {
            return XNode.DeepEquals(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentXmlDocAndXmlString(XDocument xmlDocA, string xmlStringB, string customFailMessage = null)
        {
            var xmlDocB = XDocument.Parse(xmlStringB);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentXmlStringAndXmlDoc(string xmlStringA, XDocument xmlDocB, string customFailMessage = null)
        {
            var xmlDocA = XDocument.Parse(xmlStringA);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentXmlStrings(string xmlStringA, string xmlStringB, string customFailMessage = null)
        {
            var xmlDocA = XDocument.Parse(xmlStringA);
            var xmlDocB = XDocument.Parse(xmlStringB);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentXmlDocAndFileToLoad(XDocument xmlDocA, string xmlFilePathB, string customFailMessage = null)
        {
            var xmlDocB = XDocument.Load(xmlFilePathB);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentXmlStringAndFileToLoad(string xmlStringA, string xmlFilePathB, string customFailMessage = null)
        {
            var xmlDocA = XDocument.Parse(xmlStringA);
            var xmlDocB = XDocument.Load(xmlFilePathB);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentFileToLoadAndXmlDoc(string xmlFilePathA, XDocument xmlDocB, string customFailMessage = null)
        {
            var xmlDocA = XDocument.Load(xmlFilePathA);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentFileToLoadAndXmlString(string xmlFilePathA, string xmlStringB, string customFailMessage = null)
        {
            var xmlDocA = XDocument.Load(xmlFilePathA);
            var xmlDocB = XDocument.Parse(xmlStringB);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }

        public static void AssertEquivalentFilesToLoad(string xmlFilePathA, string xmlFilePathB, string customFailMessage = null)
        {
            var xmlDocA = XDocument.Load(xmlFilePathA);
            var xmlDocB = XDocument.Load(xmlFilePathB);
            AssertDeepEqualXmlDocs(xmlDocA, xmlDocB);
        }
    }
}
