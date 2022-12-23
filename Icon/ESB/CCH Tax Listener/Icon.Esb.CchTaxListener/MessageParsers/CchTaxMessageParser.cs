using Icon.Esb.CchTax.Constants;
using Icon.Esb.CchTax.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Icon.Dvs.Model;


namespace Icon.Esb.CchTax.MessageParsers
{
    public class CchTaxMessageParser : IMessageParser<List<TaxHierarchyClassModel>>
    {
        public List<TaxHierarchyClassModel> ParseMessage(DvsMessage xmlMessage)
        {
            var xmlDocument = XDocument.Parse(xmlMessage.MessageContent);
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
