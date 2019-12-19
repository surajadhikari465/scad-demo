using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetMessageHistoryQuery : IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>
    {
        private readonly IconContext context;

        public GetMessageHistoryQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageHistory> Search(GetMessageHistoryParameters parameters)
        {
            var messages = context.MessageHistory.Where(h => parameters.MessageHistoriesById.Contains(h.MessageHistoryId)).ToList();

            return messages;
        }
    }
}
