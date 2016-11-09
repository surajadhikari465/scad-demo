using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Commands
{
    public class SaveToMessageHistoryCommand
    {
        public List<MessageHistory> Messages { get; set; }
    }
}
