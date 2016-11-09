using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders
{
    public class CollectionExcelValidationRuleBuilder : IExcelValidationRuleBuilder
    {
        private int columnIndex;
        private IEnumerable<string> values;

        public CollectionExcelValidationRuleBuilder(IEnumerable<string> values, int columnIndex)
        {
            this.values = values;
            this.columnIndex = columnIndex;
        }

        public void AddValidationRule(Workbook workbook, int numberOfRows)
        {
            var itemsWorksheet = workbook.Worksheets[0];
            var listRule = new ListDataValidationRule();

            listRule.AllowNull = true;
            listRule.ShowDropdown = true;
            listRule.ErrorMessageDescription = "Invalid value entered.";
            listRule.ErrorMessageTitle = "Validation Error";
            listRule.ErrorStyle = DataValidationErrorStyle.Stop;
            listRule.ShowErrorMessageForInvalidValue = true;

            listRule.SetValues(values.ToArray());

            var cellRegion = new WorksheetRegion(itemsWorksheet, 1, columnIndex, numberOfRows, columnIndex);
            var cellCollection = new WorksheetReferenceCollection(cellRegion);

            itemsWorksheet.DataValidationRules.Add(listRule, cellCollection);
        }
    }
}