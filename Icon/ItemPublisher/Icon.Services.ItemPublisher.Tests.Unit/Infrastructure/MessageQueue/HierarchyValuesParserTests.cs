using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass]
    public class HierarchyValuesParserTests
    {
        [TestMethod]
        public void ParseHierarchyClassIdForContract_Tax_ReturnIsInCorrectFormat()
        {
            // Given.
            var parser = new HierarchyValueParser();

            // When.
            string result = parser.ParseHierarchyClassIdForContract("0020001 PERIODICALS - NEWSPAPER NEWSSTAND SALES DAILY", 40219, Framework.Hierarchies.Tax);

            // Then.
            Assert.AreEqual("0020001", result);
        }

        [TestMethod]
        public void ParseHierarchyClassIdForContract_Financial_ReturnIsInCorrectFormat()
        {
            // Given.
            var parser = new HierarchyValueParser();

            // When.
            string result = parser.ParseHierarchyClassIdForContract("Grocery (1000)", 40219, Framework.Hierarchies.Financial);

            // Then.
            Assert.AreEqual("1000", result);
        }

        [TestMethod]
        public void ParseHierarchyClassIdForContract_Generic_ReturnIsInCorrectFormat()
        {
            // Given.
            var parser = new HierarchyValueParser();

            // When.
            string result = parser.ParseHierarchyClassIdForContract("BREADSMITH", 40219, Framework.Hierarchies.Brands);

            // Then.
            Assert.AreEqual("40219", result);
        }

        [TestMethod]
        public void ParseHierarchyClassNameForContract_FinancialIsNot0000_ReturnIsTheHierarchyClassName()
        {
            // Given.
            var parser = new HierarchyValueParser();

            // When.
            string result = parser.ParseHierarchyNameForContract("Grocery (1000)", 40219, Framework.Hierarchies.Financial);

            // Then.
            Assert.AreEqual("Grocery (1000)", result);
        }

        [TestMethod]
        public void ParseHierarchyClassNameForContract_FinancialSubteamNumberIs0000_ReturnShouldBeNa()
        {
            // Given.
            var parser = new HierarchyValueParser();

            // When.
            string result = parser.ParseHierarchyNameForContract("NoSubteam (0000)", 40219, Framework.Hierarchies.Financial);

            // Then.
            Assert.AreEqual("na", result);
        }

        [TestMethod]
        public void ParseHierarchyClassNameForContract_AnythingNotFinancial_ReturnShouldBeHierarchyClassName()
        {
            // Given.
            var parser = new HierarchyValueParser();

            // When.
            string result = parser.ParseHierarchyNameForContract("0000000 GENERAL MERCHANDISE!", 40219, Framework.Hierarchies.Tax);

            // Then.
            Assert.AreEqual("0000000 GENERAL MERCHANDISE!", result);
        }
    }
}