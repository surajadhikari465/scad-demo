using GlobalEventController.Common;
using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System.Collections.Generic;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemsByScanCodeQuery : IQuery<List<IrmaItemModel>>
    {
        public IEnumerable<string> ScanCodes { get; set; }
    }
}
