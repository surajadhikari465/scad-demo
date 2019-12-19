using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class BrandExporter : BaseHierarchyClassExporter<BrandExportViewModel>
    {
        private const int BrandNameIndex = 0;
        private const int BrandAbbreviationIndex = 1;
        private const int DesignationIndex = 2;
        private const int ParentCompanyIndex = 3;
        private const int ZipCodeIndex = 4;
        private const int LocalityIndex = 5;

        public BrandExporter()
            : base()
        {
            AddSpreadsheetColumn(BrandNameIndex,
                "Brand",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandNameIndex].Value =
                    (String.IsNullOrWhiteSpace(hierarchyClass.BrandName) && String.IsNullOrWhiteSpace(hierarchyClass.BrandId))
                        ? String.Empty
                        : String.Format("{0}|{1}", hierarchyClass.BrandName, hierarchyClass.BrandId));

            AddSpreadsheetColumn(BrandAbbreviationIndex,
                "Brand Abbreviation",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandAbbreviationIndex].Value = hierarchyClass.BrandAbbreviation);

            AddSpreadsheetColumn(DesignationIndex,
                "Designation",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[DesignationIndex].Value = hierarchyClass.Designation);

            AddSpreadsheetColumn(ParentCompanyIndex,
                "Parent Company",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ParentCompanyIndex].Value = hierarchyClass.ParentCompany);

            AddSpreadsheetColumn(ZipCodeIndex,
                "Zip Code",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ZipCodeIndex].Value = hierarchyClass.ZipCode);

            AddSpreadsheetColumn(LocalityIndex,
                "Locality",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[LocalityIndex].Value = hierarchyClass.Locality);
        }

        protected override List<BrandExportViewModel> ConvertExportDataToExportHierarchyClassModel()
        {
            List<BrandExportViewModel> exportBrands = ExportData.Select(d => new BrandExportViewModel
                {
                    BrandId = d.BrandId,
                    BrandName = d.BrandName,
                    BrandAbbreviation = d.BrandAbbreviation,
                    Designation = d.Designation,
                    ParentCompany = d.ParentCompany,
                    ZipCode = d.ZipCode,
                    Locality = d.Locality
            })
                .ToList();

            return exportBrands;
        }
    }
}