using Icon.RenewableContext;
using Icon.Framework;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetIrmaItemSubscriptionsQueryHandler : IQueryHandler<GetIrmaItemSubscriptionsQuery, List<IRMAItemSubscription>>
    {
        private IRenewableContext<IconContext> context;

        public GetIrmaItemSubscriptionsQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public List<IRMAItemSubscription> Execute(GetIrmaItemSubscriptionsQuery parameters)
        {
            SqlParameter messagesParameter = new SqlParameter("IrmaSubscriptionList", SqlDbType.Structured);
            messagesParameter.TypeName = "app.IRMAItemSubscriptionType";

            messagesParameter.Value = parameters.IrmaPushData.ConvertAll(push => new
            {
               Regioncode = push.RegionCode,
               Identifier = push.Identifier
            }).ToDataTable();

            string sql = "EXEC app.GetIrmaSubscriptions @IrmaSubscriptionList";

            var queryResults = context.Context.Database.SqlQuery<IRMAItemSubscription>(sql, messagesParameter).ToList();

            return queryResults;
        }
    }
}
