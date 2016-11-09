
namespace PushController.DataAccess.Commands
{
    public class MarkStagedRecordsAsInProcessForUdmCommand
    {
        public int Instance { get; set; }
        public int MaxRecordsToProcess { get; set; }
    }
}
