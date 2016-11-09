namespace Icon.Web.Mvc.Excel.Validators
{
    using Icon.Web.Mvc.Excel.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class PredicateExcelValidator<T> : IExcelValidator<T>
        where T : ExcelModel, new()
    {
        private string error;
        private Predicate<T> predicate;

        public PredicateExcelValidator(
            Predicate<T> predicate,
            string error)
        {
            this.predicate = predicate;
            this.error = error;
        }

        public void Validate(IEnumerable<T> excelModels)
        {
            Parallel.ForEach(excelModels, i =>
            {
                if (!predicate(i))
                    i.Error = error;
            });
        }
    }
}