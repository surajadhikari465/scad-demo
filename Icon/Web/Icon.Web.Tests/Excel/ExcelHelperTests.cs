using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Mvc.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Excel
{
    [TestClass]
    public class ExcelHelperTests
    {
        [TestMethod]
        public void GetExcelValidationValues_TrueOrFalseFields_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var trueOrFalseValidationSequence = new string[] { null, "Y", "N" };

            var fieldsForValiation = new List<string>
            {
               "Food Stamp Eligible",
               "Department Sale",
               "Hidden Item",
               "Validated",
               "Biodynamic",
               "Cheese Attribute: Raw",
               "Premium Body Care",
               "Vegetarian",
               "Whole Trade",
               "Grass Fed",
               "Pasture Raised",
               "Free Range",
               "Dry Aged",
               "Air Chilled",
               "Made In House"
            };

            // When & Then.
            foreach (var field in fieldsForValiation)
            {
                var validSelections = ExcelHelper.GetExcelValidationValues(field, includeRemoveOption: false);

                bool validationOptionsMatch = validSelections.SequenceEqual(trueOrFalseValidationSequence);

                Assert.IsTrue(validationOptionsMatch);
            }
        }

        [TestMethod]
        public void GetExcelValidationValues_AnimalWelfareRatingWithoutRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[AnimalWelfareRatings.Descriptions.AsArray.Length + 1];
            validationSequence[0] = null;
            AnimalWelfareRatings.Descriptions.AsArray.CopyTo(validationSequence, 1);

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Animal Welfare Rating", includeRemoveOption: false);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_MilkTypeWithoutRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[MilkTypes.Descriptions.AsArray.Length + 1];
            validationSequence[0] = null;
            MilkTypes.Descriptions.AsArray.CopyTo(validationSequence, 1);

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Cheese Attribute: Milk Type", includeRemoveOption: false);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_EcoScaleRatingWithoutRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[EcoScaleRatings.Descriptions.AsArray.Length + 1];
            validationSequence[0] = null;
            EcoScaleRatings.Descriptions.AsArray.CopyTo(validationSequence, 1);

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Eco-Scale Rating", includeRemoveOption: false);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_SeafoodFreshOrFrozenWithoutRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[SeafoodFreshOrFrozenTypes.Descriptions.AsArray.Length + 1];
            validationSequence[0] = null;
            SeafoodFreshOrFrozenTypes.Descriptions.AsArray.CopyTo(validationSequence, 1);

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Fresh Or Frozen", includeRemoveOption: false);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_SeafoodWildOrFarmRaisedWithoutRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[SeafoodCatchTypes.Descriptions.AsArray.Length + 1];
            validationSequence[0] = null;
            SeafoodCatchTypes.Descriptions.AsArray.CopyTo(validationSequence, 1);

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Seafood: Wild Or Farm Raised", includeRemoveOption: false);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_AnimalWelfareRatingWithRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[AnimalWelfareRatings.Descriptions.AsArray.Length + 2];
            validationSequence[0] = null;
            AnimalWelfareRatings.Descriptions.AsArray.CopyTo(validationSequence, 1);
            validationSequence[validationSequence.Length - 1] = Constants.ExcelImportRemoveValueKeyword;

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Animal Welfare Rating", includeRemoveOption: true);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_MilkTypeWithRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[MilkTypes.Descriptions.AsArray.Length + 2];
            validationSequence[0] = null;
            MilkTypes.Descriptions.AsArray.CopyTo(validationSequence, 1);
            validationSequence[validationSequence.Length - 1] = Constants.ExcelImportRemoveValueKeyword;

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Cheese Attribute: Milk Type", includeRemoveOption: true);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_EcoScaleRatingWithRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[EcoScaleRatings.Descriptions.AsArray.Length + 2];
            validationSequence[0] = null;
            EcoScaleRatings.Descriptions.AsArray.CopyTo(validationSequence, 1);
            validationSequence[validationSequence.Length - 1] = Constants.ExcelImportRemoveValueKeyword;

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Eco-Scale Rating", includeRemoveOption: true);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

          [TestMethod]
        public void GetExcelValidationValues_SeafoodFreshOrFrozenWithRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[SeafoodFreshOrFrozenTypes.Descriptions.AsArray.Length + 2];
            validationSequence[0] = null;
            SeafoodFreshOrFrozenTypes.Descriptions.AsArray.CopyTo(validationSequence, 1);
            validationSequence[validationSequence.Length - 1] = Constants.ExcelImportRemoveValueKeyword;

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Fresh Or Frozen", includeRemoveOption: true);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        public void GetExcelValidationValues_SeafoodWildOrFarmRaisedWithRemovalOption_CorrectValidationOptionsShouldBeReturned()
        {
            // Given.
            var validationSequence = new string[SeafoodCatchTypes.Descriptions.AsArray.Length + 2];
            validationSequence[0] = null;
            SeafoodCatchTypes.Descriptions.AsArray.CopyTo(validationSequence, 1);
            validationSequence[validationSequence.Length - 1] = Constants.ExcelImportRemoveValueKeyword;

            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Seafood: Wild Or Farm Raised", includeRemoveOption: true);

            // Then.
            bool validationOptionsMatch = validSelections.SequenceEqual(validationSequence);

            Assert.IsTrue(validationOptionsMatch);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetExcelValidationValues_UnknownFieldName_ExcecptionShouldBeThrown()
        {
            // Given.
            
            // When.
            var validSelections = ExcelHelper.GetExcelValidationValues("Seafood: Fresh Or Rotten", includeRemoveOption: true);

            // Then.
            // Expected excecption.
        }

        [TestMethod]
        public void GetCellStringValue_NullCell_EmptyStringShouldBeReturned()
        {
            // Given.
            object cell = null;

            // When.
            string value = ExcelHelper.GetCellStringValue(cell);

            // Then.
            Assert.AreEqual(String.Empty, value);
        }

        [TestMethod]
        public void GetCellStringValue_NonNullCell_CellValueShouldBeReturned()
        {
            // Given.
            string testValue = "test";
            object cell = testValue;

            // When.
            string value = ExcelHelper.GetCellStringValue(cell);

            // Then.
            Assert.AreEqual(testValue, value);
        }

        [TestMethod]
        public void GetBoolStringFromCellText_CellTextIsLetterY_String1ShouldBeReturned()
        {
            // Given.
            string lowercaseCellText = "y";
            string uppercaseCellText = "Y";

            // When.
            string lowercaseResult = lowercaseCellText.GetBoolStringFromCellText();
            string uppercaseResult = uppercaseCellText.GetBoolStringFromCellText();

            // Then.
            Assert.AreEqual("1", lowercaseResult);
            Assert.AreEqual("1", uppercaseResult);
        }

        [TestMethod]
        public void GetBoolStringFromCellText_CellTextIsLetterN_String0ShouldBeReturned()
        {
            // Given.
            string lowercaseCellText = "n";
            string uppercaseCellText = "N";

            // When.
            string lowercaseResult = lowercaseCellText.GetBoolStringFromCellText();
            string uppercaseResult = uppercaseCellText.GetBoolStringFromCellText();

            // Then.
            Assert.AreEqual("0", lowercaseResult);
            Assert.AreEqual("0", uppercaseResult);
        }

        [TestMethod]
        public void GetBoolStringFromCellText_CellTextIsSomethingOtherThanYOrN_CellTextShouldBeReturned()
        {
            // Given.
            string cellText = "test";

            // When.
            string result = cellText.GetBoolStringFromCellText();
            
            // Then.
            Assert.AreEqual(cellText, result);
        }

        [TestMethod]
        public void ToSpreadsheetBoolean_ValueIsString1_StringYShouldBeReturned()
        {
            // Given.
            string value = "1";

            // When.
            string result = value.ToSpreadsheetBoolean();

            // Then.
            Assert.AreEqual("Y", result);
        }

        [TestMethod]
        public void ToSpreadsheetBoolean_ValueIsString0_StringNShouldBeReturned()
        {
            // Given.
            string value = "0";

            // When.
            string result = value.ToSpreadsheetBoolean();

            // Then.
            Assert.AreEqual("N", result);
        }

        [TestMethod]
        public void ToSpreadsheetBoolean_ValueIsSomethingOtherThanString0Or1_ValueShouldBeReturned()
        {
            // Given.
            string value = "test";

            // When.
            string result = value.ToSpreadsheetBoolean();

            // Then.
            Assert.AreEqual(value, result);
        }

        [TestMethod]
        public void ToSpreadsheetBoolean_ValueIsBooleanTrue_StringYShouldBeReturned()
        {
            // Given.
            bool? value = true;

            // When.
            string result = value.ToSpreadsheetBoolean();

            // Then.
            Assert.AreEqual("Y", result);
        }

        [TestMethod]
        public void ToSpreadsheetBoolean_ValueIsBooleanFalse_StringNShouldBeReturned()
        {
            // Given.
            bool? value = false;

            // When.
            string result = value.ToSpreadsheetBoolean();

            // Then.
            Assert.AreEqual("N", result);
        }

        [TestMethod]
        public void ToSpreadsheetBoolean_ValueIsNull_EmptyStringShouldBeReturned()
        {
            // Given.
            bool? value = null;

            // When.
            string result = value.ToSpreadsheetBoolean();

            // Then.
            Assert.AreEqual(String.Empty, result);
        }

        [TestMethod]
        public void GetIdFromCellText_LineageText_IdPortionShouldBeReturned()
        {
            // Given.
            string lineage = "Lineage|2222";
            string id = lineage.Split('|').Last().Trim();

            // When.
            string result = lineage.GetIdFromCellText();

            // Then.
            Assert.AreEqual(id, result);
        }

        [TestMethod]
        public void IsDigitsOnly_ValueIsNumeric_ResultShouldBeTrue()
        {
            // Given.
            string value = "222222";

            // When.
            bool result = value.IsDigitsOnly();

            // Then.
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDigitsOnly_ValueIsAlphanumeric_ResultShouldBeFalse()
        {
            // Given.
            string value = "22222a2";

            // When.
            bool result = value.IsDigitsOnly();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDigitsOnly_ValueIsNull_ResultShouldBeFalse()
        {
            // Given.
            string value = null;

            // When.
            bool result = value.IsDigitsOnly();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDigitsOnly_ValueIsWhitespace_ResultShouldBeFalse()
        {
            // Given.
            string value = "     ";

            // When.
            bool result = value.IsDigitsOnly();

            // Then.
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsDigitsOnly_ValueHasDecimalPoint_ResultShouldBeFalse()
        {
            // Given.
            string value = "1.2";

            // When.
            bool result = value.IsDigitsOnly();

            // Then.
            Assert.IsFalse(result);
        }
    }
}
