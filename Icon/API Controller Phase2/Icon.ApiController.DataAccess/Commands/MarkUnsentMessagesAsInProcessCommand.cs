namespace Icon.ApiController.DataAccess.Commands
{
    public class MarkUnsentMessagesAsInProcessCommand
    {
        public int MiniBulkLimitMessageHistory { get; set; }
        public int MessageTypeId { get; set; }
        public int Instance { get; set; }
    }
}
