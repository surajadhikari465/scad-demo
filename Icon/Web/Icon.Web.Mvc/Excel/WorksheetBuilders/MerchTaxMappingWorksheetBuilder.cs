using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public class MerchTaxMappingWorksheetBuilder : WorksheetBuilder<MerchTaxMappingExcelModel>
    {
        private const string WorksheetName = "MerchTaxMapping";
        private IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingsQueryHandler;

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                return new List<string> { "Merchandise", "Tax" };
            }
        }

        public MerchTaxMappingWorksheetBuilder(IQueryHandler<GetMerchTaxMappingsParameters, List<MerchTaxMappingModel>> getMerchTaxMappingsQueryHandler)
            : base(WorksheetName)
        {
            this.getMerchTaxMappingsQueryHandler = getMerchTaxMappingsQueryHandler;
        }

        protected override IEnumerable<MerchTaxMappingExcelModel> GetExcelModels()
        {
            return getMerchTaxMappingsQueryHandler.Search(new GetMerchTaxMappingsParameters())
                .Select(mtm => new MerchTaxMappingExcelModel
                {
                    MerchandiseLineage = mtm.MerchandiseHierarchyClassLineage,
                    TaxLineage = mtm.TaxHierarchyClassLineage
                });
        }
    }
}