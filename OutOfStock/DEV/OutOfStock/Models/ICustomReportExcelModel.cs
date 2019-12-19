using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutOfStock.Classes;

namespace OutOfStock.Models
{
    public interface ICustomReportExcelModel
    {
        string CreateExcelFile(string excelFilename, List<ColumnDataModel> columnNames,
                               ref IEnumerable<ICustomReportExcelModel> reportData, ref ReportHeaderDataModel headerData, ref IEnumerable<ScanWithNoVimData> ScansWithNoVimData );
    }
}
