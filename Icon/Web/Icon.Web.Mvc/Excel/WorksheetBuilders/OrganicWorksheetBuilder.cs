using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public class OrganicWorksheetBuilder : WorksheetBuilder<CertificationAgencyExcelModel>
    {
        private const string WorksheetName = "Organic";
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

        public OrganicWorksheetBuilder(IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgenciesQueryHandler)
            : base(WorksheetName, false)
        {
            this.getCertificationAgenciesQueryHandler = getCertificationAgenciesQueryHandler;
        }

        protected override IEnumerable<CertificationAgencyExcelModel> GetExcelModels()
        {
            return getCertificationAgenciesQueryHandler.Search(new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = Traits.Codes.Organic })
                .Select(hc => new CertificationAgencyExcelModel { CertificationAgencyLineage = hc.hierarchyClassName + "|" + hc.hierarchyClassID })
                .Concat(new List<CertificationAgencyExcelModel> { new CertificationAgencyExcelModel { CertificationAgencyLineage = Constants.ExcelImportRemoveValueKeyword } });
        }
    }
}