using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Mvc.Excel.Validators;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Excel.Validators
{
    [TestClass]
    public class PredicateExcelValidatorTests
    {
        private PredicateExcelValidator<ItemExcelModel> validator;
        
        [TestMethod]
        public void PredicateExcelValidator_AllItemsPassPredicate_ShouldNotSetErrorOnItems()
        {
            //Given
            IEnumerable<ItemExcelModel> items = new List<ItemExcelModel>
            {
                new ItemExcelModel { ProductDescription = "Test" },
                new ItemExcelModel { ProductDescription = "Test" },
                new ItemExcelModel { ProductDescription = "Test" },
                new ItemExcelModel { ProductDescription = "Test" },
                new ItemExcelModel { ProductDescription = "Test" }
            };
            validator = new PredicateExcelValidator<ItemExcelModel>(i => !string.IsNullOrWhiteSpace(i.ProductDescription), "Test Error");

            //When
            validator.Validate(items);

            //Then
            Assert.IsTrue(items.All(i => i.Error == null));
        }

        [TestMethod]
        public void PredicateExcelValidator_AllItemsDoNotPassPredicate_ShouldSetErrorRowOnItems()
        {
            //Given
            IEnumerable<ItemExcelModel> items = new List<ItemExcelModel>
            {
                new ItemExcelModel { ProductDescription = string.Empty },
                new ItemExcelModel { ProductDescription = string.Empty },
                new ItemExcelModel { ProductDescription = string.Empty },
                new ItemExcelModel { ProductDescription = string.Empty },
                new ItemExcelModel { ProductDescription = string.Empty }
            };
            validator = new PredicateExcelValidator<ItemExcelModel>(i => !string.IsNullOrWhiteSpace(i.ProductDescription), "Test Error");

            //When
            validator.Validate(items);

            //Then
            Assert.IsTrue(items.All(i => i.Error == "Test Error"));
        }

        [TestMethod]
        public void PredicateExcelValidator_SomeItemsDoNotPassPredicateAndSomeDo_ShouldSetErrorRowsForItemsThatDontPassPredicate()
        {
            //Given
            IEnumerable<ItemExcelModel> items = new List<ItemExcelModel>
            {
                new ItemExcelModel { ProductDescription = "Test" },
                new ItemExcelModel { ProductDescription = string.Empty },
                new ItemExcelModel { ProductDescription = "Test" },
                new ItemExcelModel { ProductDescription = string.Empty },
                new ItemExcelModel { ProductDescription = "Test" }
            };
            validator = new PredicateExcelValidator<ItemExcelModel>(i => !string.IsNullOrWhiteSpace(i.ProductDescription), "Test Error");

            //When
            validator.Validate(items);

            //Then
            Assert.IsTrue(items.Where(i => i.ProductDescription == "Test").All(i => i.Error == null));
            Assert.IsTrue(items.Count(i => i.ProductDescription == "Test") == 3);
            Assert.IsTrue(items.Where(i => i.ProductDescription == string.Empty).All(i => i.Error == "Test Error"));
            Assert.IsTrue(items.Count(i => i.ProductDescription == string.Empty) == 2);
        }
    }
}
