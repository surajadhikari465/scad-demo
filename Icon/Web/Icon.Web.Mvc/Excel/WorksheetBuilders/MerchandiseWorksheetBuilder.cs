using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public class MerchandiseWorksheetBuilder : WorksheetBuilder<HierarchyClassExcelModel>
    {
        private const string WorksheetName = "Merchandise";
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                return new List<string> { "Hierarchy Class Lineage" };
            }
        }

        public MerchandiseWorksheetBuilder(IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler)
            : base(WorksheetName, false)
        {
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
        }

        protected override IEnumerable<HierarchyClassExcelModel> GetExcelModels()
        {
            return getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters())
                .MerchandiseHierarchyList
                .Select(hc => new HierarchyClassExcelModel { HierarchyClassLineage = hc.HierarchyLineage + "|" + hc.HierarchyClassId });
        }
    }
}