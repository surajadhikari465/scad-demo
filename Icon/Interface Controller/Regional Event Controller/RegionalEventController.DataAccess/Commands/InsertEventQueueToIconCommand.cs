using Icon.Framework;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertEventQueueToIconCommand
    {
        public EventQueue eventQueueEntry { get; set; }
        public readonly string eventName = "New IRMA Item";
    }
}
