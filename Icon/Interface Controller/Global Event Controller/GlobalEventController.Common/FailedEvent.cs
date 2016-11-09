using Icon.Framework;

namespace GlobalEventController.Common
{
    public class FailedEvent
    {
        public FailedEvent() { }
        public FailedEvent(EventQueue failedEvent, string failureReason)
        {
            this.Event = failedEvent;
            this.FailureReason = failureReason;
        }

        public EventQueue Event { get; set; }
        public string FailureReason { get; set; }
    }
}
