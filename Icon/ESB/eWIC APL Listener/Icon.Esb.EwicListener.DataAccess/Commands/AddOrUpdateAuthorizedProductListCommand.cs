using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class AddOrUpdateAuthorizedProductListCommand : ICommandHandler<AddOrUpdateAuthorizedProductListParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public AddOrUpdateAuthorizedProductListCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(AddOrUpdateAuthorizedProductListParameters data)
        {
            var existingApl = globalContext.Context.AuthorizedProductList.SingleOrDefault(apl => apl.AgencyId == data.Apl.AgencyId && apl.ScanCode == data.Apl.ScanCode);

            if (existingApl == null)
            {
                data.Apl.InsertDate = DateTime.Now;
                globalContext.Context.AuthorizedProductList.Add(data.Apl);
            }
            else
            {
                existingApl.ItemDescription = data.Apl.ItemDescription;
                existingApl.PackageSize = data.Apl.PackageSize;
                existingApl.UnitOfMeasure = data.Apl.UnitOfMeasure;
                existingApl.BenefitQuantity = data.Apl.BenefitQuantity;
                existingApl.BenefitUnitDescription = data.Apl.BenefitUnitDescription;
                existingApl.ItemPrice = data.Apl.ItemPrice;
                existingApl.PriceType = data.Apl.PriceType;
                existingApl.ModifiedDate = DateTime.Now;
            }

            globalContext.Context.SaveChanges();
        }
    }
}
