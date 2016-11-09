using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class HierarchyClassValidator : IExcelValidator<ItemExcelModel>
    {
        private const string BrandError = "Brand is invalid.  {0} does not exist.";
        private const string MerchandiseError = "Merchandise is invalid.  {0} does not exist.";
        private const string TaxError = "Tax is invalid.  {0} does not exist.";
        private const string BrowsingError = "Browsing is invalid.  {0} does not exist.";
        private const string NationalError = "National is invalid.  {0} does not exist.";

        private IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery;

        public HierarchyClassValidator(
            IQueryHandler<GetHierarchyLineageParameters, HierarchyClassListModel> getHierarchyLineageQuery)
        {
            this.getHierarchyLineageQuery = getHierarchyLineageQuery;
        }

        public void Validate(IEnumerable<ItemExcelModel> excelModels)
        {
            var hierarchyLineage = getHierarchyLineageQuery.Search(new GetHierarchyLineageParameters());
            var brandIds = new HashSet<string>(hierarchyLineage.BrandHierarchyList.Select(b => b.HierarchyClassId.ToString()));
            var merchIds = new HashSet<string>(hierarchyLineage.MerchandiseHierarchyList.Select(b => b.HierarchyClassId.ToString()));
            var taxIds = hierarchyLineage.TaxHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();
            var browsingIds = hierarchyLineage.BrowsingHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();
            var nationalids = hierarchyLineage.NationalHierarchyList.Select(b => b.HierarchyClassId.ToString()).ToList();

            Parallel.ForEach(excelModels, row =>
            {
                var brandId = row.Brand.GetIdFromCellText();
                var merchId = row.Merchandise.GetIdFromCellText();
                var taxId = row.Tax.GetIdFromCellText();
                var browsingId = row.Browsing.GetIdFromCellText();
                var nationalId = row.NationalClass.GetIdFromCellText();

                if(HasHierarchyClass(brandId) && !brandIds.Contains(brandId))
                {
                    row.Error = string.Format(BrandError, row.Brand);
                }

                if (HasHierarchyClass(merchId) && !merchIds.Contains(merchId))
                {
                    row.Error = string.Format(MerchandiseError, row.Merchandise);
                }

                if (HasHierarchyClass(taxId) && !taxIds.Contains(taxId))
                {
                    row.Error = string.Format(TaxError, row.Tax);
                }

                if (HasHierarchyClass(browsingId) && !browsingIds.Contains(browsingId))
                {
                    row.Error = string.Format(BrowsingError, row.Browsing);
                }

                if (HasHierarchyClass(nationalId) && !nationalids.Contains(nationalId))
                {
                    row.Error = string.Format(NationalError, row.NationalClass);
                }
            });
        }

        /// <summary>
        /// Checks if there is an existing hierarchy class ID.
        /// </summary>
        /// <param name="id">Hierarchy Class ID</param>
        /// <returns>True if there is an existing hierarchy class id.</returns>
        private bool HasHierarchyClass(string id)
        {
            // Has a non-zero value.
            return !string.IsNullOrEmpty(id) && id != "0";
        }
    }
}