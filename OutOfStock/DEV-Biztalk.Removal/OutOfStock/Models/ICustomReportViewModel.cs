using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOS.Model;
using OutOfStock.Classes;
using OutOfStock.Service;
using SharedKernel;

namespace OutOfStock.Models
{
    public interface ICustomReportViewModel
    {

        List<ColumnDataModel> Columns { get;  }


        IEnumerable<ScanWithNoVimData> GetScanswithNoVimData(DateTime? start,
            DateTime? end,
            Dictionary<int, string> StoreInfo,
            Dictionary<string, string> TeamInfo,
            Dictionary<string, string> SubTeamInfo,
            DateTime todaysDate, ref IOOSEntitiesFactory dbfactory);

        
            IEnumerable<ICustomReportExcelModel> RunQuery(RunQueryParameters parameters, ref IOOSEntitiesFactory dbfactory);

    }
}
