using Irma.Framework;
using System.Collections.Generic;

namespace TlogController.DataAccess.BulkCommands
{
    public class BulkInsertTlogReprocessRequestsCommand
    {
        public List<TlogReprocessRequest> TlogReprocessRequests { get; set; }
    }
}
