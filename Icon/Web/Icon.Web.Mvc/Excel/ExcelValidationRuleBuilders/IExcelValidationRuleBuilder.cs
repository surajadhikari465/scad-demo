using Infragistics.Documents.Excel;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders
{
    public interface IExcelValidationRuleBuilder
    {
        void AddValidationRule(Workbook workbook, int numberOfRows);
    }
}
