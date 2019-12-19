using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Infrastructure
{
    [TestClass]
    public class ItemSearchCriteriaTests
    {

        [TestMethod]
        public void SearchCriteria_ParseTwoValuesWithSpace_SearchCriteriaIsSet()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @"A B");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
            Assert.AreEqual("B", criteria.Values[1]);
        }

        [TestMethod]
        public void SearchCriteria_ParseTwoValuesWithExtraSpaceAtEnd_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @"A B ");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
            Assert.AreEqual("B", criteria.Values[1]);
        }

        [TestMethod]
        public void SearchCriteria_ParseTwoValuesWithExtraSpaceAtBeginning_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @" A B");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
            Assert.AreEqual("B", criteria.Values[1]);
        }


        [TestMethod]
        public void SearchCriteria_ParseTwoValuesWithExtraSpaceInBetweenValues_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @" A     B    ");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
            Assert.AreEqual("B", criteria.Values[1]);
        }

        [TestMethod]
        public void SearchCriteria_SingleValue_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @" A ");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_SingleValueWithQuotes_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @"""A""");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_SingleValueWithQuotesAlongWithNoQuoted_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @"""A"" B");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
            Assert.AreEqual("B", criteria.Values[1]);
        }

        [TestMethod]
        public void SearchCriteria_SingleValueWithQuotesWithNoQuoted_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, @"A ""B"" ");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual("A", criteria.Values[0]);
            Assert.AreEqual("B", criteria.Values[1]);
        }

        [TestMethod]
        public void SearchCriteria_ComplexTest1_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"""Texas"" California ""New York"" Arkansas");

            // Then
            Assert.AreEqual(4, criteria.Values.Count);
            Assert.AreEqual("Texas", criteria.Values[0]);
            Assert.AreEqual("California", criteria.Values[1]);
            Assert.AreEqual("New York", criteria.Values[2]);
            Assert.AreEqual("Arkansas", criteria.Values[3]);
        }

        [TestMethod]
        public void SearchCriteria_ComplexTest2_SearchCriteriaIsSetWithValuesTrimmed()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"""Texas"" California ""New York"" ""Arkansas""");

            // Then
            Assert.AreEqual(4, criteria.Values.Count);
            Assert.AreEqual("Texas", criteria.Values[0]);
            Assert.AreEqual("California", criteria.Values[1]);
            Assert.AreEqual("New York", criteria.Values[2]);
            Assert.AreEqual("Arkansas", criteria.Values[3]);
        }

        [TestMethod]
        public void SearchCriteria_EmptyString_ReturnsNoValues()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"");

            // Then
            Assert.AreEqual(0, criteria.Values.Count);
        }

        [TestMethod]
        public void SearchCriteria_EmptyQuotes_ReturnsNoValues()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"""");

            // Then
            Assert.AreEqual(0, criteria.Values.Count);
        }

        [TestMethod]
        public void SearchCriteria_MismatchedQuotes_ReturnsValuesWithQuotesStripped()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"""Texas");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual(@"Texas", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_MismatchedQuotesMultipleValues_ReturnsValuesWithQuotesStripped()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"""Texas"" ""New York");

            // Then
            Assert.AreEqual(2, criteria.Values.Count);
            Assert.AreEqual(@"Texas", criteria.Values[0]);
            Assert.AreEqual(@"New York", criteria.Values[1]);
        }


        [TestMethod]
        public void SearchCriteria_WhenSearchingExactlyAll_ReturnsValuesWithQuotesStripped()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ContainsAll, $@"""Texas""");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual(@"Texas", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_WhenSearchingExactlyAllWithMultipleValues_ReturnsValuesWithQuotesStripped()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ExactlyMatchesAll, $@"""Texas"" ""Oklahoma""");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual(@"Texas Oklahoma", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_WhenSearchingExactlyAllNoQoutes_ReturnsValuesWithQuotesStripped()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria(string.Empty, AttributeSearchOperator.ExactlyMatchesAll, $@"Texas Oklahoma");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual(@"Texas Oklahoma", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_SingleItemIdValuesExactlyAll_ReturnsValues()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAll, $@"1");

            // Then
            Assert.AreEqual(1, criteria.Values.Count);
            Assert.AreEqual(@"1", criteria.Values[0]);
        }

        [TestMethod]
        public void SearchCriteria_MultipleItemIdValuesExactlyAll_ReturnsNoValues()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAll, $@"1 2");

            // Then
            Assert.AreEqual(0, criteria.Values.Count);
        }

        [TestMethod]
        public void SearchCriteria_NonNumberForItemId_ReturnsNoValues()
        {
            // When 
            ItemSearchCriteria criteria = new ItemSearchCriteria("ItemId", AttributeSearchOperator.ExactlyMatchesAll, $@"Test");

            // Then
            Assert.AreEqual(0, criteria.Values.Count);
        }

    }
}
