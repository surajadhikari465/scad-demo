namespace Icon.Web.Mvc.Excel.Validators
{
    using Icon.Web.Mvc.Excel.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class DuplicateScanCodeValidator : IExcelValidator<ItemExcelModel>
    {
        private const string DuplicateScanCodeMessage = "Scan Code appears multiple times on the spreadsheet.";

        public void Validate(IEnumerable<ItemExcelModel> excelModels)
        {
            var duplicateScanCodes = excelModels.AsParallel()
                .GroupBy(i => i.ScanCode)
                .Where(g => g.Count() > 1)
                .SelectMany(g => g)
                .ToList();

            duplicateScanCodes.ForEach(i => i.Error = DuplicateScanCodeMessage);
        }
    }
}