using Infragistics.Documents.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGenerationWeb.Excel
{
    public interface ISpreadsheetManager<T>
    {
        Workbook Workbook {get;set;}
        List<T> ConvertToModel();
    }
}
