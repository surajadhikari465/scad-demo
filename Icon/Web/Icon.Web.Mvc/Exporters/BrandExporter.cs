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
        }

        protected override List<BrandExportViewModel> ConvertExportDataToExportHierarchyClassModel()
        {
            List<BrandExportViewModel> exportBrands = ExportData.Select(d => new BrandExportViewModel
                {
                    BrandId = d.BrandId,
                    BrandName = d.BrandName,
                    BrandAbbreviation = d.BrandAbbreviation
                })
                .ToList();

            return exportBrands;
        }
    }
}