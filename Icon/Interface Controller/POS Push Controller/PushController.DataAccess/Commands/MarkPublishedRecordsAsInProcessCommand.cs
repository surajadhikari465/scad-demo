using Irma.Framework;

namespace PushController.DataAccess.Commands
{
    public class MarkPublishedRecordsAsInProcessCommand
    {
        public IrmaContext Context { get; set; }
        public int Instance { get; set; }
        public int MaxRecordsToProcess { get; set; }
    }
}
