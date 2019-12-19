using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Excel.Validators
{
    [TestClass] [Ignore]
    public class DuplicateScanCodeValidatorTests
    {
        private DuplicateScanCodeValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            validator = new DuplicateScanCodeValidator();
        }

        [TestMethod]
        public void DuplicateScanCodeValidator_DuplicateScanCodesExist_ShouldSetErrorRows()
        {
            //Given
            string scanCodeErrorMessage = "Scan Code appears multiple times on the spreadsheet.";
            List<ItemExcelModel> models = new List<ItemExcelModel>
            {
                new ItemExcelModel { ScanCode = "1234" },
                new ItemExcelModel { ScanCode = "4321" },
                new ItemExcelModel { ScanCode = "1234" },
                new ItemExcelModel { ScanCode = "4321" },
                new ItemExcelModel { ScanCode = "12345" }
            };

            //When
            validator.Validate(models);

            //Then
            Assert.AreEqual(scanCodeErrorMessage, models[0].Error);
            Assert.AreEqual(scanCodeErrorMessage, models[1].Error);
            Assert.AreEqual(scanCodeErrorMessage, models[2].Error);
            Assert.AreEqual(scanCodeErrorMessage, models[3].Error);
            Assert.IsNull(models[4].Error);
        }

        [TestMethod]
        public void DuplicateScanCodeValidator_DuplicateScanCodesDontExist_ShouldNotSetErrorRows()
        {
            //Given
            List<ItemExcelModel> models = new List<ItemExcelModel>
            {
                new ItemExcelModel { ScanCode = "12341" },
                new ItemExcelModel { ScanCode = "12342" },
                new ItemExcelModel { ScanCode = "12343" },
                new ItemExcelModel { ScanCode = "12344" },
                new ItemExcelModel { ScanCode = "12345" }
            };

            //When
            validator.Validate(models);

            //Then
            foreach (var model in models)
            {
                Assert.IsNull(model.Error);
            }
        }
    }
}
