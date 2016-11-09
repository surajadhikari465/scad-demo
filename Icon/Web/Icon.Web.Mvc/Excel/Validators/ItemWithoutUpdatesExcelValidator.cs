using Icon.Web.Common.Utility;
using Icon.Web.Mvc.Excel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class ItemWithoutUpdatesExcelValidator<T> : IExcelValidator<T> where T : ExcelModel, new()
    {
        public void Validate(IEnumerable<T> excelModels)
        {
            var scanCodePropertyName = "ScanCode";
            var properties = typeof(T).GetProperties()
                .Where(p => p.IsDefined(typeof(ExcelColumnAttribute), false) && p.Name != scanCodePropertyName);

            Parallel.ForEach(excelModels,
                i =>
                {
                    if (properties.All(p => p.GetValue(i)?.ToString() == string.Empty))
                    {
                        i.Error = "No fields are specified to update.";
                    }
                });
        }
    }
}