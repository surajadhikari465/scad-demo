using System.Collections.Generic;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders.Factories
{
    public interface IExcelValidationRuleBuilderFactory<T>
    {
        IEnumerable<IExcelValidationRuleBuilder> CreateBuilders();
    }
}
