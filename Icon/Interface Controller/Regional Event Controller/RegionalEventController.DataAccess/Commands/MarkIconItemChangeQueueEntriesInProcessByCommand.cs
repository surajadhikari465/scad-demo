namespace RegionalEventController.DataAccess.Commands
{
    public class MarkIconItemChangeQueueEntriesInProcessByCommand
    {
        public string Instance { get; set; }
        public int MaxQueueEntriesToProcess { get; set; }
    }
}
