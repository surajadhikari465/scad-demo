using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Icon.Common;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public class NonGmoWorksheetBuilder : WorksheetBuilder<CertificationAgencyExcelModel>
    {
        private const string WorksheetName = "NonGMO";
        private IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler;

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                return new ReadOnlyCollection<string>(new List<string>
                {
                    "Agency"
                });
            }
        }

        public NonGmoWorksheetBuilder(IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler)
            : base(WorksheetName, false)
        {
            this.getCertificationAgenciesQueryHandler = getCertificationAgenciesQueryHandler;
        }

        protected override IEnumerable<CertificationAgencyExcelModel> GetExcelModels()
        {
            return getCertificationAgenciesQueryHandler.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.NonGmo })
                .Select(hc => new CertificationAgencyExcelModel { CertificationAgencyLineage = hc.hierarchyClassName + "|" + hc.hierarchyClassID })
                .Concat(new List<CertificationAgencyExcelModel> { new CertificationAgencyExcelModel { CertificationAgencyLineage = Constants.ExcelImportRemoveValueKeyword } });
        }
    }
}