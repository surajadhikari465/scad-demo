using Icon.Framework;
using System;
using System.Collections.Generic;

namespace PushController.DataAccess.Commands
{
    public class UpdateStagingTableDatesForUdmCommand
    {
        public bool ProcessedSuccessfully { get; set; }
        public List<IRMAPush> StagedPosData { get; set; }
        public DateTime Date { get; set; }
    }
}
