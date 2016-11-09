using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using PushController.DataAccess.Interfaces;
using System;
using System.Linq;

namespace PushController.DataAccess.Commands
{
    public class DeleteTemporaryPriceReductionsCommandHandler : ICommandHandler<DeleteTemporaryPriceReductionsCommand>
    {
        private ILogger<DeleteTemporaryPriceReductionsCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public DeleteTemporaryPriceReductionsCommandHandler(
            ILogger<DeleteTemporaryPriceReductionsCommandHandler> logger,
            IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(DeleteTemporaryPriceReductionsCommand command)
        {
            if (command.TemporaryPriceReductions == null || !command.TemporaryPriceReductions.Any())
            {
                logger.Warn("DeleteTemporaryPriceReductionsCommandHandler was called with a null or empty list.");
                return;
            }

            foreach (var tpr in command.TemporaryPriceReductions)
            {
                var itemPrice = context.Context
                    .ItemPrice
                    .SingleOrDefault(ip => 
                        ip.itemID == tpr.ItemId &&
                        ip.localeID == tpr.LocaleId &&
                        ip.itemPriceTypeID == ItemPriceTypes.Tpr);

                if (itemPrice != null)
                {
                    context.Context.ItemPrice.Remove(itemPrice);
                }
            }

            context.Context.SaveChanges();

            logger.Info(String.Format("Successfully removed {0} cancelled TPRs.", command.TemporaryPriceReductions.Count));
        }
    }
}
