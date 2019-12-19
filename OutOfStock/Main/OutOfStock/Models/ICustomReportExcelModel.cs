using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutOfStock.Models
{
    public interface ICustomReportExcelModel
    {
        string CreateExcelFile(string excelFilename, IEnumerable<CustomReportViewModel> enumerables,
                               string headerMain, string headerStores, string headerTeams, string headerSubTeams);
    }
}
