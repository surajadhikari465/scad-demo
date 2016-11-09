using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdatePluRequestCommandHandler : ICommandHandler<UpdatePluRequestCommand>
    {
        private IconContext context;

        public UpdatePluRequestCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdatePluRequestCommand data)
        {
            PLURequest updatedItem = context.PLURequest.Find(data.PLURequest.pluRequestID);
            updatedItem.brandName = data.PLURequest.brandName;
            updatedItem.itemDescription = data.PLURequest.itemDescription;
            updatedItem.lastModifiedUser = data.PLURequest.lastModifiedUser;
            updatedItem.lastModifiedDate = DateTime.Now;
            updatedItem.merchandiseClassID = data.PLURequest.merchandiseClassID;
            updatedItem.nationalClassID = data.PLURequest.nationalClassID;
            updatedItem.packageUnit = data.PLURequest.packageUnit;
            updatedItem.posDescription = data.PLURequest.posDescription;
            updatedItem.requestStatus = data.PLURequest.requestStatus;
            updatedItem.retailSize = data.PLURequest.retailSize;
            updatedItem.retailUom = data.PLURequest.retailUom;
            updatedItem.scanCodeTypeID = data.PLURequest.scanCodeTypeID;
            updatedItem.requestStatus = data.PLURequest.requestStatus;
            updatedItem.FinancialClassID = data.PLURequest.FinancialClassID;

            try
            {
                context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("Error updating PLU Request for national PLU {0}.", data.PLURequest.nationalPLU), exception);
            }
        }
    }
}
