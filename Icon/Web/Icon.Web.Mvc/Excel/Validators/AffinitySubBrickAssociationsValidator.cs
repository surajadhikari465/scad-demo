using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class AffinitySubBrickAssociationsValidator : IExcelValidator<ItemExcelModel>
    {
        private const string Error = "Items cannot be associated to an Affinity sub-brick.";

        private IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>> getAffinitySubBricksQuery;

        public AffinitySubBrickAssociationsValidator(
            IQueryHandler<GetAffinitySubBricksParameters, List<HierarchyClass>> getAffinitySubBricksQuery)
        {
            this.getAffinitySubBricksQuery = getAffinitySubBricksQuery;
        }

        public void Validate(IEnumerable<ItemExcelModel> excelModels)
        {
            var invalidSubBricks = this.getAffinitySubBricksQuery
                .Search(new GetAffinitySubBricksParameters())
                .Select(hc => hc.hierarchyClassID.ToString())
                .AsParallel();

            var invalidRows = excelModels.AsParallel()
                .Join(invalidSubBricks.AsParallel(),
                    iem => iem.Merchandise.GetIdFromCellText(),
                    isb => isb,
                    (iem, isb) => iem);

            invalidRows.ForAll(ir => ir.Error = Error);
        }
    }
}