using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.Excel.Validators
{
    [TestClass]
    public class RegexItemModelValidatorTests
    {
        public RegexItemModelValidator<ItemExcelModel> validator;

        [TestMethod]
        public void RegexItemModel_3ItemPropertiesDoNotMatchRegexExpression_ShouldSetErrorOn3ItemsThatDontMatchRegexExpression()
        {
            //Given
            string error = "Test Error";
            validator = new RegexItemModelValidator<ItemExcelModel>(i => i.FoodStampEligible, @"\bMatchingValue\b", error);

            List<ItemExcelModel> models = new List<ItemExcelModel>
            {
                new ItemExcelModel { FoodStampEligible = "NotMatchingValue" },
                new ItemExcelModel { FoodStampEligible = "    " },
                new ItemExcelModel { FoodStampEligible = "MatchingValue" },
                new ItemExcelModel { FoodStampEligible = "MatchingValue" },
                new ItemExcelModel { FoodStampEligible = "NotMatchingValue" },
            };

            //When
            validator.Validate(models);

            //Then 
            Assert.AreEqual(3, models.Count(m => m.Error == error));
            Assert.AreEqual(2, models.Count(m => m.Error == null));
        }
    }
}
