using GlobalEventController.Common;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Common.Email;
using Icon.Logging;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GlobalEventController.DataAccess.Commands.BrandDeleteCommand;

namespace GlobalEventController.Controller.EventServices
{
    public class BrandDeleteEventService : EventServiceBase
    {
        private ICommandHandler<BrandDeleteCommand> brandDeleteCommandHandler;
        private ILogger<BrandDeleteEventService> logger;
        private IEmailClient emailClient;
        private IGlobalControllerSettings gloconSettings;
        private IQueryHandler<GetIrmaBrandQuery, ItemBrand> getIrmaBrandQuery;

        public BrandDeleteEventService(IrmaContext irmaContext,
            ICommandHandler<BrandDeleteCommand> brandDeleteHandler,
            IQueryHandler<GetIrmaBrandQuery, ItemBrand> getIrmaBrandQuery,
            ILogger<BrandDeleteEventService> logger,
            IEmailClient emailClient,
            IGlobalControllerSettings brandDeleteAlertConfiguration)
                : base(irmaContext)
        {
            this.brandDeleteCommandHandler = brandDeleteHandler;
            this.getIrmaBrandQuery = getIrmaBrandQuery;
            this.logger = logger;
            this.emailClient = emailClient;
            this.gloconSettings = brandDeleteAlertConfiguration;
        }

        public override void Run()
        {
            base.VerifyEventParameters(nameof(BrandDeleteEventService), ReferenceId, Message, Region);

            //create the command parameters
            var deleteBrandCommand = new BrandDeleteCommand()
            {
                IconBrandId = ReferenceId,
                Region = Region
            };

            //execute the command
            brandDeleteCommandHandler.Handle(deleteBrandCommand);
            irmaContext.SaveChanges();

            // examine the command result
            if (deleteBrandCommand.Result == BrandDeleteResult.NothingDeleted)
            {
                logger.Error(BuildMessageForBrandNotFound(deleteBrandCommand));
            }
            else if (deleteBrandCommand.Result.HasFlag(BrandDeleteResult.ItemBrandAssociatedWithItems))
            {
                var irmaItemBrandQuery = new GetIrmaBrandQuery { IconBrandId = deleteBrandCommand.IconBrandId.Value };
                ItemBrand irmaItemBrand = getIrmaBrandQuery.Handle(irmaItemBrandQuery);

                string brandAssociatedWithItemsMessage = BuildMessageForBrandAssociatedWithItem(
                    deleteBrandCommand, irmaItemBrandQuery, irmaItemBrand);

                logger.Warn(brandAssociatedWithItemsMessage);

                if (gloconSettings.BrandDeleteEmailAlertsEnabled)
                {
                    emailClient.Send(brandAssociatedWithItemsMessage, gloconSettings.BrandDeleteEmailSubject);
                }
            }
        }

        private string BuildMessageForBrandNotFound(BrandDeleteCommand command)
        {
            var brandNotFoundMsg = "This brand was not found in the IRMA ValidatedBrand table so it will not be deleted: " +
                    $"IconBrandId = {command.IconBrandId}, Region = '{command.Region}'";
            return brandNotFoundMsg;
        }

        private string BuildMessageForBrandAssociatedWithItem(
            BrandDeleteCommand command, GetIrmaBrandQuery query, ItemBrand brand)
        {
            var msgBuilder = new StringBuilder();
            msgBuilder.Append("Brand \"");
            msgBuilder.Append(brand?.Brand_Name);
            msgBuilder.Append("\" in Region ");
            msgBuilder.Append(command.Region);
            msgBuilder.Append(" could not be deleted because it is still associated with ");
            msgBuilder.Append(query.ResultItemCount);
            if (query.ResultItemCount > 1)
            {
                msgBuilder.Append(" items in the IRMA database.");
            }
            else
            {
                msgBuilder.Append(" item in the IRMA database.");
            }
            msgBuilder.Append(" (IconBrandId = ");
            msgBuilder.Append(command.IconBrandId);
            msgBuilder.Append(" IrmaBrandId = ");
            msgBuilder.Append(brand?.Brand_ID);
            msgBuilder.Append(").");

            return msgBuilder.ToString();
        }
    }
}

