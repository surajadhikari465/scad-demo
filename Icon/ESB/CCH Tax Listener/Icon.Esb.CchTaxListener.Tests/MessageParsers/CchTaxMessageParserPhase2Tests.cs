using Icon.Esb.CchTax.MessageParsers;
using Icon.Dvs.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.CchTax.Tests.MessageParsers
{
    [TestClass]
    public class CchTaxMessageParserPhase2Tests
    {
        private CchTaxMessageParser messageParser;

        [TestInitialize]
        public void Initialize()
        {
            messageParser = new CchTaxMessageParser();
        }

        [TestMethod]
        public void ParseMessage_ValidPhase2Message_ShouldParseMessageSuccessfully()
        {
            //Given
            var message = new DvsMessage(null, File.ReadAllText(@"TestMessages\test_tax_message_phase_2.xml"));
            

            //When
            var taxHierarchyClasses = messageParser.ParseMessage(message);

            //Then
            Assert.AreEqual(225, taxHierarchyClasses.Count);
            Assert.AreEqual("0000000", taxHierarchyClasses.First().TaxCode);
            Assert.AreEqual("0000000 GENERAL MERCHANDISE", taxHierarchyClasses.First().HierarchyClassName);
            Assert.AreEqual("9999000", taxHierarchyClasses.Last().TaxCode);
            Assert.AreEqual("9999000 TAXABLE PRODUCTS AND SVCS", taxHierarchyClasses.Last().HierarchyClassName);
        }
    }
}
