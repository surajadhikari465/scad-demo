using GlobalEventController.DataAccess.Infrastructure;
using Icon.DbContextFactory;
using Irma.Framework;
using System.Linq;

namespace GlobalEventController.DataAccess.Commands
{
    public class BrandDeleteCommandHandler : ICommandHandler<BrandDeleteCommand>
    {
        private IDbContextFactory<IrmaContext> contextFactory;

        public BrandDeleteCommandHandler(IDbContextFactory<IrmaContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Handle(BrandDeleteCommand command)
        {
            using (var context = contextFactory.CreateContext())
            {
                // find the ValidatedBrand in the database
                var validatedBrandToDelete = context.ValidatedBrand
                    .SingleOrDefault(vb => vb.IconBrandId == command.IconBrandId);

                // validate that it was found
                if (validatedBrandToDelete == null)
                {
                    // brand not found, nothing deleted
                    command.Result = BrandDeleteCommand.BrandDeleteResult.NothingDeleted;
                }
                else
                {
                    //remove the ValidatedBrand
                    context.ValidatedBrand.Remove(validatedBrandToDelete);
                    command.Result |= BrandDeleteCommand.BrandDeleteResult.ValidatedBrandDeleted;

                    //is brand associated with any items?
                    if (context.Item.Any(i => i.Brand_ID == validatedBrandToDelete.IrmaBrandId))
                    {
                        // can't delete because item(s) still using Brand_ID
                        command.Result |= BrandDeleteCommand.BrandDeleteResult.ItemBrandAssociatedWithItems;
                    }
                    else
                    {
                        //remove the ItemBrand
                        context.ItemBrand.Remove(context.ItemBrand.Single(ib => ib.Brand_ID == validatedBrandToDelete.IrmaBrandId));
                        command.Result |= BrandDeleteCommand.BrandDeleteResult.ItemBrandDeleted;
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
