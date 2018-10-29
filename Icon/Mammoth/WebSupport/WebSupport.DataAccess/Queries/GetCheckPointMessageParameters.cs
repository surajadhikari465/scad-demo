using Icon.Common.DataAccess;
using System.Collections.Generic;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetCheckPointMessageParameters: IQuery<IEnumerable<CheckPointMessageModel>>
    {
        public string Region { get; set; }
        public List<int> BusinessUnitIds { get; set; }
        public List<string> ScanCodes { get; set; }
    }
}