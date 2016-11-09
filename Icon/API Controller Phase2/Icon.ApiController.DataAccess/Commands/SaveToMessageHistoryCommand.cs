using Icon.Framework;

namespace Icon.ApiController.DataAccess.Commands
{
    public class SaveToMessageHistoryCommand<TMessageHistory>
    {
        public TMessageHistory Message { get; set; }
    }
}
