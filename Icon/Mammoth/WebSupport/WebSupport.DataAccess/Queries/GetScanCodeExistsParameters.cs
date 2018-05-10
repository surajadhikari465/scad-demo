using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetScanCodeExistsParameters : IQuery<List<ScanCodeExistsModel>>
    {
        public List<string> ScanCodes { get; set; }
    }
}
