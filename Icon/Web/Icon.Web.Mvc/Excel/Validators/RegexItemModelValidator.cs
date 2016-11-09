namespace Icon.Web.Mvc.Excel.Validators
{
    using Common.Utility;
    using Icon.Web.Mvc.Excel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class RegexItemModelValidator<T> : IExcelValidator<T> where T : ExcelModel, new()
    {
        public string RegexPattern { get; private set; }
        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }

        public RegexItemModelValidator(
            Expression<Func<ItemExcelModel, string>> propertyExpression,
            string regexPattern,
            string errorMessage)
        {
            this.PropertyName = PropertyUtility.GetPropertyName(propertyExpression);
            this.RegexPattern = regexPattern;
            this.ErrorMessage = errorMessage;
        }

        public virtual void Validate(IEnumerable<T> excelModels)
        {
            var propertyInfo = typeof(T)
                    .GetProperty(this.PropertyName);

            Parallel.ForEach(excelModels, i =>
            {
                var propertyValue = propertyInfo.GetValue(i) as string ?? string.Empty;
                if  (!Regex.IsMatch(propertyValue, this.RegexPattern))
                {
                    i.Error = this.ErrorMessage;
                }
            });
        }
    }
}