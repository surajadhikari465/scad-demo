using System;
using System.Linq;
using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using Icon.Logging;


namespace GlobalEventController.DataAccess.Commands
{
    public class BrandDeleteCommandHandler : ICommandHandler<BrandDeleteCommand>
    {
        private readonly IrmaContext context;
        private ILogger<BrandDeleteCommandHandler> logger;

        public BrandDeleteCommandHandler(IrmaContext context, ILogger<BrandDeleteCommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void Handle(BrandDeleteCommand command)
        {
            var validatedBrandToDelete = context.ValidatedBrand.Where(vb => vb.IconBrandId == command.IconBrandId).SingleOrDefault();
            if (validatedBrandToDelete == null)
            {
                logger.Error(String.Format("The following brand was not found in the IRMA ValidatedBrand table, so no update will be performed:  IconBrandId = {0}, Region = {1}",
                    command.IconBrandId, command.Region));
                command.Result = BrandDeleteCommand.BrandDeleteResult.NothingDeleted;
                return;
            }

            //remove from ValidatedBrand
            context.ValidatedBrand.Remove(validatedBrandToDelete);
            var irmaBrandIdToDelete = validatedBrandToDelete.IrmaBrandId;
            command.Result = BrandDeleteCommand.BrandDeleteResult.ValidatedBrandDeleted;

            //is brand associated with any items?
            if (!context.Item.Any(i => i.Brand_ID == irmaBrandIdToDelete))
            {
                //remove from ItemBrand
                context.ItemBrand.Remove(context.ItemBrand.Single(ib => ib.Brand_ID == irmaBrandIdToDelete));
                command.Result = BrandDeleteCommand.BrandDeleteResult.ItemBrandAndValidatedBrandDeleted;
            }
        }
    }
}
