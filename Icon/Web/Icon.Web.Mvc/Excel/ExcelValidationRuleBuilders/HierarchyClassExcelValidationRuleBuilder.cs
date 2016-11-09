using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders
{
    public class HierarchyClassExcelValidationRuleBuilder : IExcelValidationRuleBuilder
    {
        private readonly string hierarchyClassWorksheetName;
        private readonly int hierarchyClassColumnIndex;

        public HierarchyClassExcelValidationRuleBuilder(string hierarchyClassWorksheetName, int hierarchyClassColumnIndex)
        {
            this.hierarchyClassWorksheetName = hierarchyClassWorksheetName;
            this.hierarchyClassColumnIndex = hierarchyClassColumnIndex;
        }

        public void AddValidationRule(Workbook workbook, int numberOfRows)
        {
            var hierarchyClassWorksheet = workbook.Worksheets[hierarchyClassWorksheetName];
            var hierarchyClasses = hierarchyClassWorksheet.Rows.Select(r => r.Cells[0]);

            if(hierarchyClasses.Any())
            {
                var itemsWorksheet = workbook.Worksheets[0];
                var listRule = new ListDataValidationRule();

                listRule.AllowNull = true;
                listRule.ShowDropdown = true;
                listRule.ErrorMessageDescription = "Invalid value entered.";
                listRule.ErrorMessageTitle = "Validation Error";
                listRule.ErrorStyle = DataValidationErrorStyle.Stop;
                listRule.ShowErrorMessageForInvalidValue = true;

                string valuesFormula = string.Format("={0}!$A$1:$A${1}", hierarchyClassWorksheetName, hierarchyClasses.Count());

                listRule.SetValuesFormula(valuesFormula, null);

                var cellRegion = new WorksheetRegion(itemsWorksheet, 1, hierarchyClassColumnIndex, numberOfRows, hierarchyClassColumnIndex);
                var cellCollection = new WorksheetReferenceCollection(cellRegion);

                itemsWorksheet.DataValidationRules.Add(listRule, cellCollection);
            }
        }
    }
}