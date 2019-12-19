using System.Collections.Generic;
using System.Linq;
using Icon.Common;

namespace Icon.Web.Mvc.Excel.ExcelValidationRuleBuilders
{
    public class RemovableValueCollectionExcelValidationRuleBuilder : CollectionExcelValidationRuleBuilder
    {
        public RemovableValueCollectionExcelValidationRuleBuilder(IEnumerable<string> values, int columnIndex)
            : base(AddRemovableValues(values), columnIndex)
        {

        }

        private static IEnumerable<string> AddRemovableValues(IEnumerable<string> values)
        {
            var valuesList = new List<string> { null }
                .Concat(values)
                .ToList();
            valuesList.Add(Constants.ExcelImportRemoveValueKeyword);

            return valuesList;
        }
    }
}