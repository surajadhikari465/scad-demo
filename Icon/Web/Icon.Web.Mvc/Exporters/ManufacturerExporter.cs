using Icon.Web.Mvc.Models;
using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Mvc.Exporters
{
    public class ManufacturerExporter : BaseHierarchyClassExporter<ManufacturerExportViewModel>
    {
        private const int ManufacturerNameIndex = 0;
        private const int ManufacturerZipCodeIndex = 1;
        private const int ManufacturerArCustomerIdIndex = 2;

        public ManufacturerExporter()
            : base()
        {
            AddSpreadsheetColumn(ManufacturerNameIndex,
                "Manufacturer",
                15000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ManufacturerNameIndex].Value =
                    (String.IsNullOrWhiteSpace(hierarchyClass.ManufacturerName) && String.IsNullOrWhiteSpace(hierarchyClass.ManufacturerId))
                        ? String.Empty
                        : $"{hierarchyClass.ManufacturerName}|{hierarchyClass.ManufacturerId}");

            AddSpreadsheetColumn(ManufacturerZipCodeIndex,
                "Zip Code",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ManufacturerZipCodeIndex].Value = hierarchyClass.ZipCode);

            AddSpreadsheetColumn(ManufacturerArCustomerIdIndex,
                "AR Customer ID",
                4000,
                HorizontalCellAlignment.Left,
                (row, hierarchyClass) => row.Cells[ManufacturerArCustomerIdIndex].Value = hierarchyClass.ArCustomerId);
        }

        protected override List<ManufacturerExportViewModel> ConvertExportDataToExportHierarchyClassModel()
        {
            List<ManufacturerExportViewModel> exportManufacturers = ExportData.Select(d => new ManufacturerExportViewModel
            {
                ManufacturerId = d.ManufacturerId,
                ManufacturerName = d.ManufacturerName,
                ZipCode = d.ZipCode,
                ArCustomerId = d.ArCustomerId
            }).ToList();

            return exportManufacturers;
        }
    }
}