using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders.Factories
{
    public interface IExcelValidationRuleBuilderFactory<T>
    {
        IEnumerable<IExcelValidationRuleBuilder> CreateBuilders();
    }
}
