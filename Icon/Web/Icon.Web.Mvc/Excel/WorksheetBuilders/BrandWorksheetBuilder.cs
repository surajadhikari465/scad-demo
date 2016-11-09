using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Infragistics.Documents.Excel;
using Icon.Web.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Common;

namespace Icon.Web.Mvc.Excel.WorksheetBuilders
{
    public class BrandWorksheetBuilder : WorksheetBuilder<HierarchyClassExcelModel>
    {
        private const string WorksheetName = "Brands";
        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler;

        public override IReadOnlyCollection<string> Columns
        {
            get
            {
                return new List<string> { "Hierarchy Class Lineage" };
            }
        }

        public BrandWorksheetBuilder(IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQueryHandler)
            : base(WorksheetName, false)
        {
            this.getHierarchyLineageQueryHandler = getHierarchyLineageQueryHandler;
        }

        protected override IEnumerable<HierarchyClassExcelModel> GetExcelModels()
        {
            return getHierarchyLineageQueryHandler.Search(new GetHierarchyLineageParameters())
                .BrandHierarchyList
                .Select(hc => new HierarchyClassExcelModel { HierarchyClassLineage = hc.HierarchyClassName + "|" + hc.HierarchyClassId });
        }
    }
}