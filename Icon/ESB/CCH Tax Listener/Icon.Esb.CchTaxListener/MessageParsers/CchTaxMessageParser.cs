using Icon.Esb.CchTax.Constants;
using Icon.Esb.CchTax.Models;
using Icon.Esb.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Icon.Esb.CchTax.MessageParsers
{
    public class CchTaxMessageParser : IMessageParser<List<TaxHierarchyClassModel>>
    {
        public List<TaxHierarchyClassModel> ParseMessage(IEsbMessage xmlMessage)
        {
            var xmlDocument = XDocument.Parse(xmlMessage.MessageText);
            var taxHierarchyClasses = xmlDocument.Descendants().Elements((XNamespace)XmlNamespaceConstants.TaxHierarchyClassNamespace + "taxHierarchyClass")
                        .Select(element => new TaxHierarchyClassModel
                        {
                            TaxCode = element.Element((XNamespace)XmlNamespaceConstants.HierarchyClassNamespaceV2 + "id").Value,
                            HierarchyClassName = element.Element((XNamespace)XmlNamespaceConstants.HierarchyClassNamespaceV2 + "name").Value
                        }).ToList();

            return taxHierarchyClasses;
        }
    }
}
