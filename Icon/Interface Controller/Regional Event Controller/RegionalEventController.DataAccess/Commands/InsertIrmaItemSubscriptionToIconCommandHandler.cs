using Icon.Logging;
using Icon.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RegionalEventController.DataAccess.Commands
{
    public class InsertIrmaItemSubscriptionToIconCommandHandler : ICommandHandler<InsertIrmaItemSubscriptionToIconCommand>
    {
        private ILogger<InsertIrmaItemSubscriptionToIconCommandHandler> logger;
        private IconContext context;
        public InsertIrmaItemSubscriptionToIconCommandHandler(ILogger<InsertIrmaItemSubscriptionToIconCommandHandler> logger, IconContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(InsertIrmaItemSubscriptionToIconCommand command)
        {
            List<IRMAItemSubscription> deletedSubscriptions = context.IRMAItemSubscription
                                .Where(s => s.identifier == command.irmaNewItemSubscription.identifier && s.regioncode == command.irmaNewItemSubscription.regioncode && s.deleteDate != null)
                                .ToList();

            if (deletedSubscriptions != null && deletedSubscriptions.Count > 0)
            { 
                context.IRMAItemSubscription.RemoveRange(deletedSubscriptions);
                context.SaveChanges();
            }   
   
            bool hasActiveSubscription = context.IRMAItemSubscription.Any(s =>
                 s.identifier == command.irmaNewItemSubscription.identifier && s.regioncode == command.irmaNewItemSubscription.regioncode && s.deleteDate == null);

            if (!hasActiveSubscription)
            {
                command.irmaNewItemSubscription.deleteDate = null;
                context.IRMAItemSubscription.Add(command.irmaNewItemSubscription);
                context.SaveChanges();
            }
        }
    }
}
