using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.ApiController.DataAccess.Queries
{
    public class IsMessageHistoryANonRetailProductMessageQuery : IQueryHandler<IsMessageHistoryANonRetailProductMessageParameters, bool>
    {
        private IDbContextFactory<IconContext> iconContextFactory;

        public IsMessageHistoryANonRetailProductMessageQuery(IDbContextFactory<IconContext> iconContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
        }

        public bool Search(IsMessageHistoryANonRetailProductMessageParameters parameters)
        {
            if (parameters.Message.MessageTypeId == MessageTypes.Product)
            {
                using (var context = iconContextFactory.CreateContext())
                {
                    context.MessageHistory.Attach(parameters.Message);

                    return (parameters.Message.MessageQueueProduct.FirstOrDefault() != default(MessageQueueProduct)
                        && parameters.Message.MessageQueueProduct.FirstOrDefault().ItemTypeCode == ItemTypeCodes.NonRetail);
                }
            }

            return false;
        }
    }
}
