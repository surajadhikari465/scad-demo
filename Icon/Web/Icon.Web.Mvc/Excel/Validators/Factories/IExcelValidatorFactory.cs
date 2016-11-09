namespace Icon.Web.Mvc.Excel.Validators.Factories
{
    using Models;
    using System.Collections.Generic;

    public interface IExcelValidatorFactory<T> where T : ExcelModel, new()
    {
        IEnumerable<IExcelValidator<T>> CreateValidators();
    }   
}