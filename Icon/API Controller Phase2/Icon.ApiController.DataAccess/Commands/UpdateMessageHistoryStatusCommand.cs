using Icon.Framework;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommand<TMessageHistory>
    {
        public TMessageHistory Message { get; set; }
        public int MessageStatusId { get; set; }
    }
}
