using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateMessageHistoryStatusCommand
    {
        public List<int> MessageHistoriesById { get; set; }
        public int MessageStatusId { get; set; }
    }
}
