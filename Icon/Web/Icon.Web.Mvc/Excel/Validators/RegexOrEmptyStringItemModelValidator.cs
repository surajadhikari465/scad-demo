namespace Icon.Web.Mvc.Excel.Validators
{
    using Icon.Web.Mvc.Excel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class RegexOrEmptyStringItemModelValidator<T> : RegexItemModelValidator<T> where T : ExcelModel, new()
    {
        public RegexOrEmptyStringItemModelValidator (
            Expression<Func<ItemExcelModel, string>> propertyExpression,
            string regexPattern,
            string errorMessage) : base (propertyExpression, regexPattern, errorMessage)
        {      
        }

        public override void Validate(IEnumerable<T> excelModels)
        {
            var propertyInfo = typeof(T)
                    .GetProperty(this.PropertyName);

            Parallel.ForEach(excelModels, i =>
            {
                var propertyValue = propertyInfo.GetValue(i) as string ?? string.Empty;
                if (!String.IsNullOrEmpty(propertyValue) && !Regex.IsMatch(propertyValue, this.RegexPattern))
                {
                    i.Error = this.ErrorMessage;
                }
            });
        }
    }
}