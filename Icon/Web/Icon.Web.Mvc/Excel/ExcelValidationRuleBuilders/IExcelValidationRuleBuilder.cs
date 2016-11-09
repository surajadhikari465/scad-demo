using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders
{
    public interface IExcelValidationRuleBuilder
    {
        void AddValidationRule(Workbook workbook, int numberOfRows);
    }
}
