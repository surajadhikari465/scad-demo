
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetItemByScanCodeParameters : IQuery<Item>
    {
        public string ScanCode { get; set; }
    }
}
