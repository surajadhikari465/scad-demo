using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class BrandExporter : BaseHierarchyClassExporter<BrandExportViewModel>
    {
        private const int BrandIdIndex = 0;
        private const int BrandNameIndex = 1;
        private const int BrandAbbreviationIndex = 2;
        private const int DesignationIndex = 3;
        private const int ParentCompanyIndex = 4;
        private const int ZipCodeIndex = 5;
        private const int LocalityIndex = 6;

        public BrandExporter()
            : base()
        {
            HierarchyWorksheetName = "Brands";

            AddSpreadsheetColumn(BrandIdIndex,
                "Brand ID",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandIdIndex].Value =
                    (string.IsNullOrWhiteSpace(hierarchyClass.BrandId))
                        ? string.Empty
                        : hierarchyClass.BrandId);

            AddSpreadsheetColumn(BrandNameIndex,
                "Brand Name",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandNameIndex].Value =
                    (string.IsNullOrWhiteSpace(hierarchyClass.BrandName))
                        ? string.Empty
                        : hierarchyClass.BrandName);

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