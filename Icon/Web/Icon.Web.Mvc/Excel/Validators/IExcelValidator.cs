namespace Icon.Web.Mvc.Excel.Validators
{
    using Icon.Web.Mvc.Excel.Models;
    using System.Collections.Generic;

    public interface IExcelValidator<T> where T : ExcelModel, new()
    {
        void Validate(IEnumerable<T> excelModels);
    }
}
