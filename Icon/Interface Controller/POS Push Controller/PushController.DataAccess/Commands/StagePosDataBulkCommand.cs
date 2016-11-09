using Icon.Framework;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class StagePosDataBulkCommand
    {
        public List<IRMAPush> PosData { get; set; }
    }
}
