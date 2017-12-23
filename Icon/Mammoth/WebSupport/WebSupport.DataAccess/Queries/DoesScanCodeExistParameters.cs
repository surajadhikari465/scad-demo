using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Queries
{
    public class DoesScanCodeExistParameters : IQuery<bool>
    {
        public string ScanCode { get; set; }
    }
}
