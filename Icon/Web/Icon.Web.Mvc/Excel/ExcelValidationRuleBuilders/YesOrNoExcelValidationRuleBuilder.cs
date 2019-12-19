using System.Collections.Generic;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders
{
    public class YesOrNoExcelValidationRuleBuilder : CollectionExcelValidationRuleBuilder
    {
        private readonly static IEnumerable<string> YesOrNoCollection = new List<string> { null, "Y", "N" };

        public YesOrNoExcelValidationRuleBuilder(int columnIndex) : base(YesOrNoCollection, columnIndex) { }
    }
}