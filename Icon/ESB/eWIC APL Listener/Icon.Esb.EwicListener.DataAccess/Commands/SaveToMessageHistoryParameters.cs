using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class SaveToMessageHistoryParameters
    {
        public List<MessageHistory> Messages { get; set; }
    }
}
