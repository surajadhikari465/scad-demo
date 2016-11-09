using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetFailedMessagesQuery : IQueryHandler<GetFailedMessagesParameters, List<MessageModel>>
    {
        private IconContext context;

        public GetFailedMessagesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<MessageModel> Search(GetFailedMessagesParameters parameters)
        {
            return context.MessageHistory.Where(mh => mh.MessageStatusId == MessageStatusTypes.Failed)
                .Select(mh => new MessageModel
                {
                    Id = mh.MessageHistoryId,
                    InsertDate = mh.InsertDate,
                    MessageStatusId = mh.MessageStatusId,
                    MessageTypeId = mh.MessageTypeId
                }).ToList();
        }
    }
}
