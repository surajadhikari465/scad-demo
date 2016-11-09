using Infragistics.Documents.Excel;
using MessageGenerationWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessageGenerationWeb.Excel
{
    public class ItemPriceDeleteSpreadsheetManager : ISpreadsheetManager<ItemPriceDeleteModel>
    {
        private ItemPriceDeleteSpreadsheetHeader header;

        public Workbook Workbook { get; set; }

        public ItemPriceDeleteSpreadsheetManager()
        {
            header = new ItemPriceDeleteSpreadsheetHeader();
        }

        public List<ItemPriceDeleteModel> ConvertToModel()
        {
            return Workbook.Worksheets[0].Rows
                .Skip(1)
                .Where(r => r.Cells.Any())
                .Select(r => new ItemPriceDeleteModel
                    {
                        ScanCode = r.Cells[header.ScanCodeColumnIndex].Value.ToString(),
                        BusinessUnit = r.Cells[header.BusinessUnitColumnIndex].Value.ToString(),
                        Price = decimal.Parse(r.Cells[header.PriceColumnIndex].Value.ToString()),
                        StartDate = DateTime.Parse(r.Cells[header.StartDateColumnIndex].Value.ToString()),
                        EndDate = DateTime.Parse(r.Cells[header.EndDateColumnIndex].Value.ToString()),
                        Uom = r.Cells[header.UomColumnIndex].Value.ToString()
                    })
                .ToList();
        }
    }
}