using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class ScanCodeAlreadyExistsValidator : IExcelValidator<NewItemExcelModel>
    {
        private IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>> getExistingScanCodeUploadsQuery;

        public ScanCodeAlreadyExistsValidator(IQueryHandler<GetExistingScanCodeUploadsParameters, List<ScanCodeModel>> getExistingScanCodeUploadsQuery)
        {
            this.getExistingScanCodeUploadsQuery = getExistingScanCodeUploadsQuery;
        }

        public void Validate(IEnumerable<NewItemExcelModel> excelModels)
        {
            var parameters = new GetExistingScanCodeUploadsParameters
            {
                ScanCodes = excelModels.Select(row => new ScanCodeModel { ScanCode = row.ScanCode }).ToList()
            };

            var alreadyExistingScanCodes = getExistingScanCodeUploadsQuery.Search(parameters).Select(sc => sc.ScanCode).ToList();

            if (alreadyExistingScanCodes.Count > 0)
            {
                Parallel.ForEach(excelModels.AsParallel().Where(m => alreadyExistingScanCodes.Contains(m.ScanCode)), m =>
                {
                    m.Error = "Scan Code already exists.";
                });
            }
        }
    }
}