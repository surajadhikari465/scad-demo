
namespace PushController.DataAccess.Commands
{
    public class MarkStagedRecordsAsInProcessForEsbCommand
    {
        public int Instance { get; set; }
        public int MaxRecordsToProcess { get; set; }
    }
}
