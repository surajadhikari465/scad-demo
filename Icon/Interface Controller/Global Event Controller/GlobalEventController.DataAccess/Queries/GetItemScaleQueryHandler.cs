using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace GlobalEventController.DataAccess.Queries
{
    public class GetItemScaleQueryHandler : IQueryHandler<GetItemScaleQuery, ItemScale>
    {
        private IrmaContext context;

        public GetItemScaleQueryHandler(IrmaContext context)
        {
            this.context = context;
        }

        public ItemScale Handle(GetItemScaleQuery parameters)
        {
            IQueryable<ItemScale> query = context.ItemScale.Where(isc => isc.Item_Key == parameters.ItemKey);
            return query.FirstOrDefault();
        }
    }
}
