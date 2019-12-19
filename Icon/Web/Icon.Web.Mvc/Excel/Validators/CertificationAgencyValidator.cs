using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Icon.Common;

namespace Icon.Web.Mvc.Excel.Validators
{
    public class CertificationAgencyValidator<T> : IExcelValidator<T> where T : ExcelModel, new()
    {
        private IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgencyQuery;

        public string ErrorMessage { get; private set; }
        public string PropertyName { get; private set; }
        public string TraitCode { get; private set; }

        public CertificationAgencyValidator(Expression<Func<ItemExcelModel, string>> propertyExpression,
            string traitCode,
            IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>> getCertificationAgencyQuery)
        {
            this.PropertyName = PropertyUtility.GetPropertyName(propertyExpression);
            this.TraitCode = traitCode;
            this.getCertificationAgencyQuery = getCertificationAgencyQuery;
        }

        public void Validate(IEnumerable<T> excelModels)
        {
            var validAgencyHierarchyClasses = getCertificationAgencyQuery.Search(
                new GetCertificationAgenciesByTraitParameters { AgencyTypeTraitCode = this.TraitCode });

            var validAgencies = validAgencyHierarchyClasses.Select(x => x.hierarchyClassID.ToString())
                .Union(new[] { string.Empty, Constants.ExcelRemoveFieldIndicator });

            this.ErrorMessage = string.Format(
                "{0} agency is not recognized. Valid entries are {1}.", 
                typeof(ItemExcelModel).GetProperty(PropertyName).GetCustomAttribute<ExcelColumnAttribute>().Column,
                string.Join(", ", validAgencyHierarchyClasses.Select(a => a.hierarchyClassName)));

            var propertyInfo = typeof(ItemExcelModel).GetProperty(this.PropertyName);

            Parallel.ForEach(excelModels, i =>
            {
                var agency = (propertyInfo.GetValue(i) as string ?? string.Empty)
                    .GetIdFromCellText();

                if(agency != Constants.ExcelImportRemoveValueKeyword && !validAgencies.Contains(agency))
                {
                    i.Error = this.ErrorMessage;
                }
            });
        }
    }
}