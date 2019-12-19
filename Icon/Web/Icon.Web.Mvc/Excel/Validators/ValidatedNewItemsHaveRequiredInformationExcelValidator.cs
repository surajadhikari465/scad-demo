using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class ValidatedNewItemsHaveRequiredInformationExcelValidator : IExcelValidator<NewItemExcelModel>
    {
        public void Validate(IEnumerable<NewItemExcelModel> excelModels)
        {
            Parallel.ForEach(excelModels.Where(m => m.Validated == "Y"), m =>
            {
                if (string.IsNullOrWhiteSpace(m.ScanCode)
                    || string.IsNullOrWhiteSpace(m.ProductDescription)
                    || string.IsNullOrWhiteSpace(m.PosDescription)
                    || string.IsNullOrWhiteSpace(m.Brand)
                    || string.IsNullOrWhiteSpace(m.ProductDescription)
                    || string.IsNullOrWhiteSpace(m.PosDescription)
                    || string.IsNullOrWhiteSpace(m.PackageUnit)
                    || string.IsNullOrWhiteSpace(m.FoodStampEligible)
                    || string.IsNullOrWhiteSpace(m.Tax)
                    || string.IsNullOrWhiteSpace(m.Merchandise)
                    || string.IsNullOrWhiteSpace(m.NationalClass)
                    || string.IsNullOrWhiteSpace(m.Size)
                    || string.IsNullOrWhiteSpace(m.Uom))
                {
                    m.Error = "Rows marked for validation must have all canonical information in the spreadsheet.";
                }
            });
        }
    }
}