using Icon.Web.DataAccess.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class BulkBrandExporter : BaseHierarchyClassExporter<BulkImportBrandModel>
    {
        private const int BrandNameIndex = 0;
        private const int BrandAbbreviationIndex = 1;
        private const int BrandErrorIndex = 2;

        public BulkBrandExporter()
            : base()
        {
            AddSpreadsheetColumn(BrandNameIndex,
                "Brand",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandNameIndex].Value = String.Format("{0}|{1}", hierarchyClass.BrandName, hierarchyClass.BrandId));

            AddSpreadsheetColumn(BrandAbbreviationIndex,
                "Brand Abbreviation",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandAbbreviationIndex].Value = hierarchyClass.BrandAbbreviation);

            AddSpreadsheetColumn(BrandErrorIndex,
                "Error",
                5000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[BrandErrorIndex].Value = hierarchyClass.Error);
        }

        protected override List<BulkImportBrandModel> ConvertExportDataToExportHierarchyClassModel()
        {
            List<BulkImportBrandModel> exportBrands = ExportData.Select(d => new BulkImportBrandModel
                {
                    BrandId = d.BrandId,
                    BrandName = d.BrandName,
                    BrandAbbreviation = d.BrandAbbreviation,
                    Error = d.Error
                })
                .ToList();

            return exportBrands;
        }
    }
}