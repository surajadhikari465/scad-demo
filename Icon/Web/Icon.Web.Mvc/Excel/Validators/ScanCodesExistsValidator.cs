using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class ScanCodesExistsValidator : IExcelValidator<ItemExcelModel>
    {
        private const string Error = "Scan Code does not exist in Icon.";

        private IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>> getNewScanCodesUploadQuery;

        public ScanCodesExistsValidator(
            IQueryHandler<GetNewScanCodeUploadsParameters, List<ScanCodeModel>> getNewScanCodesUploadQuery)
        {
            this.getNewScanCodesUploadQuery = getNewScanCodesUploadQuery;
        }

        public void Validate(IEnumerable<ItemExcelModel> excelModels)
        {
            var newScanCodes = getNewScanCodesUploadQuery.Search(new GetNewScanCodeUploadsParameters()
                {
                    ScanCodes = excelModels.Select(i => new ScanCodeModel{ ScanCode = i.ScanCode }).ToList()
                });

            if(newScanCodes.Any())
            {
                newScanCodes.AsParallel()
                    .Join(
                        excelModels.AsParallel(),
                        scm => scm.ScanCode,
                        iem => iem.ScanCode,
                        (scm, iem) => iem)
                    .ForAll(iem => iem.Error = Error);
            }
        }
    }
}