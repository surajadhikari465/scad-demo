using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class TaxClassWithoutAbbreviationValidator : IExcelValidator<ItemExcelModel>
    {
        private const string Error = "Tax class has no abbreviation.";

        private IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>> taxHierarchyClassesWithoutAbbrevQuery;

        public TaxClassWithoutAbbreviationValidator(
            IQueryHandler<GetTaxHierarchyClassesWithNoAbbreviationParameters, List<HierarchyClass>> taxHierarchyClassesWithoutAbbrevQuery)
        {
            this.taxHierarchyClassesWithoutAbbrevQuery = taxHierarchyClassesWithoutAbbrevQuery;
        }

        public void Validate(IEnumerable<ItemExcelModel> excelModels)
        {
            var invalidTaxClasses = this.taxHierarchyClassesWithoutAbbrevQuery.Search(
                new GetTaxHierarchyClassesWithNoAbbreviationParameters())
                .Select(t => t.hierarchyClassID.ToString());

            invalidTaxClasses.AsParallel()
                .Join(excelModels.AsParallel(),
                    itc => itc,
                    iem => iem.Tax.GetIdFromCellText(),
                    (itc, iem) => iem)
                .ForAll(iem => iem.Error = Error);
        }
    }
}