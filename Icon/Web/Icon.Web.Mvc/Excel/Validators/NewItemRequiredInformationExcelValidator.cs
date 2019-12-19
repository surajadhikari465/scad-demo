using Icon.Web.Common.Utility;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class NewItemRequiredInformationExcelValidator : IExcelValidator<NewItemExcelModel>
    {
        private List<string> requiredNewItemPropertyNames;

        public NewItemRequiredInformationExcelValidator()
        {
            requiredNewItemPropertyNames = new List<string>
            {
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.ScanCode),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.Brand),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.ProductDescription),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.PosDescription),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.FoodStampEligible),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.PackageUnit),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.Size),
                PropertyUtility.GetPropertyName<NewItemExcelModel, string>(x => x.Uom)
            };
        }

        public void Validate(IEnumerable<NewItemExcelModel> excelModels)
        {
            var properties = typeof(NewItemExcelModel)
                .GetProperties()
                .Where(p => requiredNewItemPropertyNames.Contains(p.Name));

            Parallel.ForEach(excelModels, m =>
            {
                foreach (var property in properties)
                {
                    var value = property.GetValue(m)?.ToString();
                    if(string.IsNullOrWhiteSpace(value))
                    {
                        m.Error = "The row does not contain all information required to add the item.";
                    }
                }
            });
        }
    }
}