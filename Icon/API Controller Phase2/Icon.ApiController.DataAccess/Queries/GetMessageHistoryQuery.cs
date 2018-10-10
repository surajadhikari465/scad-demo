using Icon.ApiController.Common;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System.Collections.Generic;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetMessageHistoryQuery : IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>>
    {
        private ILogger<GetMessageHistoryQuery> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public GetMessageHistoryQuery(ILogger<GetMessageHistoryQuery> logger, IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public List<MessageHistory> Search(GetMessageHistoryParameters parameters)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                var messages = context.MessageHistory
                    .Where(mh => mh.InProcessBy == parameters.Instance && mh.MessageTypeId == parameters.MessageTypeId)
                    .ToList();
      
                logger.Info(messages.Count > 0 ? string.Format("Controller {0} found {1} MessageHistory entries ready for processing.  Starting with MessageHistoryId {2}.", ControllerType.Instance, messages.Count, messages[0].MessageHistoryId)
                                               : string.Format("Controller {0} found 0 MessageHistory entries ready for processing.", ControllerType.Instance));

                return messages;
            }
        }
    }
}
