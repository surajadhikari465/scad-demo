using Icon.RenewableContext;
using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetPriceUomQueryHandler : IQueryHandler<GetPriceUomQuery, UOM>
    {
        private IRenewableContext<IconContext> context;

        public GetPriceUomQueryHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public UOM Execute(GetPriceUomQuery parameters)
        {
            var uom = context.Context.UOM.SingleOrDefault(u => u.uomID == parameters.PriceUomId);

            if (uom == null)
            {
                throw new ArgumentException(String.Format("No matching UOM could be found for UOM ID: {0}", parameters.PriceUomId));
            }
            else
            {
                return uom;
            }
        }
    }
}
