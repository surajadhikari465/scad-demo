using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Exporters
{
    public class BrandTemplateExporter : BaseHierarchyClassExporter<BrandExportViewModel>
    {
        private const int BrandNameIndex = 0;
        private const int BrandAbbreviationIndex = 1;

        public BrandTemplateExporter()
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

        public override void Export()
        {
            BuildSpreadsheet();
        }

        protected override List<BrandExportViewModel> ConvertExportDataToExportHierarchyClassModel()
        {
            throw new NotImplementedException();
        }
    }
}