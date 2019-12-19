using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public class BrowsingWorksheetBuilder : WorksheetBuilder<HierarchyClassExcelModel>
    {
        private const string WorksheetName = "Browsing";
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                return new List<string> { "Hierarchy Class Lineage" };
            }
        }

        public BrowsingWorksheetBuilder(IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler)
            : base(WorksheetName, false)
        {
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
        }

        protected override IEnumerable<HierarchyClassExcelModel> GetExcelModels()
        {
            return getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters())
                .BrowsingHierarchyList
                .Select(hc => new HierarchyClassExcelModel { HierarchyClassLineage = hc.HierarchyLineage + "|" + hc.HierarchyClassId });
        }
    }
}