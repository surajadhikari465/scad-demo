using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Services.NewItem.Infrastructure;
using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Services.NewItem.Queries
{
    public class GetItemIdsQueryHandler : IQueryHandler<GetItemIdsQuery, Dictionary<string, int>>
    {
        private IRenewableContext<IconContext> context;

        public GetItemIdsQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public Dictionary<string, int> Search(GetItemIdsQuery parameters)
        {
            return context.Context.ScanCode
                .Where(sc => parameters.ScanCodes.Contains(sc.scanCode))
                .ToDictionary(
                    sc => sc.scanCode,
                    sc => sc.itemID);
        }
    }
}
